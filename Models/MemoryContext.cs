using NMemory.Indexes;
using NMemory.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace WIS.Database.Setup.Models
{
    public class MemoryContext : NMemory.Database
    {
        public ITable<WMSReleaseInfo> Releases { get; set; }

        public ITable<WMSVersionInfo> Versions { get; set; }

        public ITable<WMSReleaseScripts> Scripts { get; set; }

        public MemoryContext()
        {
            var versionTable = Tables.Create(x => x.Id, new IdentitySpecification<WMSVersionInfo>(x => x.Id, 1, 1));
            var releaseTable = Tables.Create(x => x.Id, new IdentitySpecification<WMSReleaseInfo>(x => x.Id, 1, 1));
            var scriptsTable = Tables.Create(x => x.Id, new IdentitySpecification<WMSReleaseScripts>(x => x.Id, 1, 1));

            Releases = releaseTable;
            Versions = versionTable;
            Scripts = scriptsTable;

            RelationOptions options = new RelationOptions(
                cascadedDeletion: true);

            var releasesByVersionIndex = releaseTable.CreateIndex(
                new RedBlackTreeIndexFactory(),
                p => p.VersionId);

            var scriptsByReleasesIndex = scriptsTable.CreateIndex(
                new RedBlackTreeIndexFactory(),
                p => p.ReleaseId);

            this.Tables.CreateRelation(
                versionTable.PrimaryKeyIndex,
                releasesByVersionIndex,
                x => x,
                x => x,
                options);

            this.Tables.CreateRelation(
                releaseTable.PrimaryKeyIndex,
                scriptsByReleasesIndex,
                x => x,
                x => x,
                options);

        }
    }
}
