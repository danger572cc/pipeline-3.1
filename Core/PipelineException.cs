using System;
using System.Collections.Generic;
using System.Text;

namespace WIS.Database.Setup.Core
{
    public class PipelineException : Exception
    {
        public int Step { get; set; }

        public PipelineException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
