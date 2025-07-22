using System;
using System.Collections.Generic;
using System.Text;
using WIS.Database.Setup.Models;

namespace WIS.Database.Setup.Core.Services
{
    public interface IWMSVersionService
    {
        public void ExecuteScripts();

        public void GetDataSource();

        public List<WMSReleaseInfo> GetVersionsToMigrate();
    }
}
