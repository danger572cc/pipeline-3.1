using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WIS.Database.Setup.Core;
using WIS.Database.Setup.Core.Services;
using WIS.Database.Setup.Core.Settings;
using WIS.Database.Setup.Models;

namespace WIS.Database.Setup.Imp.Services
{
    public class WMSVersionService : IWMSVersionService
    {
        private readonly OracleConnection _db;

        private readonly MemoryContext _memoryDatabse;

        private readonly SetupSettings _setupSettings;

        private readonly List<VersionsSettings> _versionsSettings;

        public WMSVersionService(
            MemoryContext memoryDatabse, 
            DatabaseSettings dbSettings,
            SetupSettings setupSettings,
            List<VersionsSettings> versionsSettings)
        {
            _db = new OracleConnection(dbSettings.ConnectionString);
            _memoryDatabse = memoryDatabse;
            _setupSettings = setupSettings;
            _versionsSettings = versionsSettings;
        }

        public void ExecuteScripts()
        {
            throw new NotImplementedException();
            //TODO: 1. Validar Scripts (En caso de error lanzar una excepción).
            //TODO: 2. Ejecutar Scripts si son validos y en caso de no existir ningún error en la ejecución commit en la operación.
        }

        public void GetDataSource()
        {
            string baseDirectory = !string.IsNullOrEmpty(_setupSettings.Directory) ? _setupSettings.Directory : $"{AppDomain.CurrentDomain.BaseDirectory}Versions";
            var directories = Directory.GetDirectories(baseDirectory);
            foreach (var directoryPath in directories)
            {
                var directoryInfo = new DirectoryInfo(directoryPath);
                Save_VersionInfo(directoryInfo);
            }
        }

        public List<WMSReleaseInfo> GetVersionsToMigrate()
        {
            int lastedNumberVersion = int.Parse(_setupSettings.VersionUpdate.Replace(".", ""));
            var versionsToUpdate = new List<WMSReleaseInfo>();
            var mainQuery = _memoryDatabse.Releases.GroupBy(g => g.VersionId);
            foreach (var version in mainQuery)
            {
                var versionData = _memoryDatabse.Versions.FirstOrDefault(f => f.Id == version.Key);
                int.TryParse(versionData.RequiredVersion, out int releaseVersionRequired);
                var releases = releaseVersionRequired == 0 ? version.OrderBy(o => this.GetNumberVersion(o.Name)).ToList() : version.Where(f => this.GetNumberVersion(f.Name) <= releaseVersionRequired).OrderBy(o => this.GetNumberVersion(o.Name)).ToList();
                versionsToUpdate.AddRange(releases);
            }
            return lastedNumberVersion == 0 ? versionsToUpdate : versionsToUpdate.FindAll(f => GetNumberVersion(f.Name) <= lastedNumberVersion);
        }

        #region private methods
        private int GetNumberVersion(string version) 
        {
            return int.Parse(version.Replace(".", ""));
        }

        private string Remove_Sentences(string scriptContent)
        {
            foreach (var sentence in _setupSettings.ExcludeSentencesSQL)
            {
                scriptContent = scriptContent.Replace(sentence, "");
            }
            return scriptContent;
        }

        private void Save_ReleaseInfo(DirectoryInfo directoryReleaseDetail, string version)
        {
            // Guardo datos del release en memoria
            _memoryDatabse.Releases.Insert(new WMSReleaseInfo
            {
                Name = directoryReleaseDetail.Name,
                VersionId = _memoryDatabse.Versions.FirstOrDefault(f => f.Name == version).Id,
                Status = PipelineStatus.WAITING
            });
            // Se obtiene los scripts SQL a ejecutarse.
            var scriptFiles = Directory.GetFiles(directoryReleaseDetail.FullName, "*.sql", SearchOption.TopDirectoryOnly);
            foreach (var scriptFilePath in scriptFiles)
            {
                var fileInfo = new FileInfo(scriptFilePath);
                var scriptContent = File.ReadAllText(scriptFilePath);
                //Remover sentencia COMMIT o Commit;
                scriptContent = Remove_Sentences(scriptContent);
                _memoryDatabse.Scripts.Insert(new WMSReleaseScripts
                {
                    Name = fileInfo.Name,
                    Script = scriptContent,
                    ReleaseId = _memoryDatabse.Releases.FirstOrDefault(f => f.Name == directoryReleaseDetail.Name).Id
                });
            }
        }

        private void Save_VersionInfo(DirectoryInfo directoryVersionDetail)
        {
            // Guardo datos de la versión en memoria
            var releaseRequired = _versionsSettings.FirstOrDefault(f => f.Name == directoryVersionDetail.Name)?.ReleaseRequired;
            _memoryDatabse.Versions.Insert(new WMSVersionInfo
            {
                Name = directoryVersionDetail.Name,
                RequiredVersion = releaseRequired
            });
            var releaseVersions = Directory.GetDirectories(directoryVersionDetail.FullName);
            foreach (var releaseVersionPath in releaseVersions)
            {
                var directoryReleaseInfo = new DirectoryInfo(releaseVersionPath);
                Save_ReleaseInfo(directoryReleaseInfo, directoryVersionDetail.Name);
            }
        }
        #endregion
    }
}
