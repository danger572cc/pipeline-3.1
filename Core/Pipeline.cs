using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIS.Database.Setup.Core
{
    public class Pipeline<T>
    {
        private IStep<T> _root;

        public Pipeline<T> Register(IStep<T> filter)
        {
            if (_root == null)
            {
                _root = filter;
            }
            else
            {
                _root.Register(filter);
            }
            return this;
        }

        public Task Execute(T context)
        {
            return _root.Execute(context);
        }
    }

    public class PipelineBuilder<T>
    {
        private List<Func<IStep<T>>> _filters = new List<Func<IStep<T>>>();

        public PipelineBuilder<T> Register(Func<IStep<T>> filter)
        {
            _filters.Add(filter);
            return this;
        }

        public PipelineBuilder<T> Register(IStep<T> filter)
        {
            _filters.Add(() => filter);
            return this;
        }

        public IStep<T> Build()
        {
            var root = _filters.First().Invoke();
            foreach (var filter in _filters.Skip(1))
            {
                root.Register(filter.Invoke());
            }
            return root;
        }
    }

    public static class PipelineStatus 
    {
        public static string WAITING = "En espera";

        public static string RUNNING = "Ejecutándose";

        public static string PASSED = "OK";

        public static string FAILED = "Error";
    }
}
