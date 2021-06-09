using System;
using System.Diagnostics;

namespace VehicleTracking.Seeder
{
    /// <summary>
    /// Provides functionality to drop and migrate databases controlled by Entity Framework.
    /// </summary>
    public class DatabaseMigrator
    {
        private readonly string _projectPath;

        /// <summary>
        /// Instantiates a DatabaseMigration.
        /// </summary>
        /// <param name="projectPath">Path to the root folder of a project containing EF migration data.</param>
        public DatabaseMigrator(string projectPath)
        {
            if (projectPath == null)
                throw new ArgumentNullException(nameof(projectPath));

            _projectPath = projectPath;
        }

        public void DropDatabase()
        {
            try
            {
                using (Process process = GetProcess("dotnet", "ef database drop", _projectPath))
                {
                    process.Start();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void MigrateDatabase()
        {
            using (Process process = GetProcess("dotnet", "ef database update", _projectPath))
            {
                process.Start();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }

        protected Process GetProcess(string fileName, string args, string workingDirectory) {
            Process process = new Process();

            process.StartInfo = new ProcessStartInfo {
                FileName = fileName,
                Arguments = args,
                RedirectStandardError = true,
                WorkingDirectory = workingDirectory
            };

            process.ErrorDataReceived += (sender, e) => {
                Log.Error(e.Data);
            };

            return process;
        }
    }
}
