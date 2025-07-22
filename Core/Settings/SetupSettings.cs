using System.Collections.Generic;

namespace WIS.Database.Setup.Core.Settings
{
    public sealed class SetupSettings
    {
        public string Directory { get; set; }

        public string InitialVersion { get; set; }

        public string VersionUpdate { get; set; }

        public List<string> ExcludeSentencesSQL { get; set; }
    }
}
