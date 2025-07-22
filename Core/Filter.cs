using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WIS.Database.Setup.Core
{
    public interface IStep<TContext>
    {
        void Register(IStep<TContext> filter);

        Task Execute(TContext context);
    }

    public abstract class Step<TContext> : IStep<TContext>
    {
        private IStep<TContext> next;

        protected abstract Task Execute(TContext context, Func<TContext, Task> next);

        public void Register(IStep<TContext> filter)
        {
            if (next == null)
            {
                next = filter;
            }
            else
            {
                next.Register(filter);
            }
        }

        Task IStep<TContext>.Execute(TContext context)
        {
            return Execute(context, ctx => next == null
                  ? Task.CompletedTask
                  : next.Execute(ctx));
        }
    }
}
