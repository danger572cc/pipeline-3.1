using System;
using System.Collections.Generic;
using System.Text;

namespace WIS.Database.Setup.Models
{
    public class WMSVersionInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string RequiredVersion { get; set; }
    }

    public class WMSReleaseInfo 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public int VersionId { get; set; }
    }

    public class WMSReleaseScripts 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Script { get; set; }

        public int ReleaseId { get; set; }
    }
}
