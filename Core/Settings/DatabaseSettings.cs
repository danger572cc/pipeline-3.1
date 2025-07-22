using System;
using System.Collections.Generic;
using System.Text;

namespace WIS.Database.Setup.Core.Settings
{
    public sealed class DatabaseSettings
    {
        public string  Schema { get; set; }

        public string ConnectionString { get; set; }

        public string AdoDataSource { get; set; }
    }
}
