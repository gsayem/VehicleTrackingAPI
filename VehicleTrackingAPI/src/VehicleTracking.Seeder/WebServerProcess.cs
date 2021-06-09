using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using static VehicleTracking.Seeder.Utils;

namespace VehicleTracking.Seeder {


    /// <summary>
    /// Wrapper class to handle starting and stopping the Identity Server web process.
    /// </summary>
    public class WebServerProcess : IDisposable {
        private const string ExceptionString = "Unhandled Exception";

        private string _projectPath;
        private string _idServerUrl;
        private Process _process;

        public ServerStatus Status { get; private set; }

        public WebServerProcess(string projectPath, string idServerUrl) {
            _projectPath = projectPath;
            _idServerUrl = idServerUrl;
        }

        /// <summary>
        /// Starts the Identity Server web server process.
        /// </summary>
        public void Start() {
            Status = ServerStatus.Starting;

            _process = new Process() {
                StartInfo = new ProcessStartInfo() {
                    FileName = "dotnet",
                    Arguments = "run",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = _projectPath
                },
                EnableRaisingEvents = true
            };

            // Log anything on the error stream. If an exception occurs, stop the process.
            _process.ErrorDataReceived += (sender, e) => {
                Log.Error(e.Data);

                if (e.Data != null && e.Data.Contains(ExceptionString)) {
                    Dispose();
                }
            };

            // Clean up if the process stops unexpectedly
            _process.Exited += (sender, e) => {
                Dispose();
            };

            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            ListenForServer();
        }

        public void Stop() {
            Dispose();
        }

        public void Dispose() {
            if (_process != null) {
                // Need a lock as this may be called from both Exited and ErrorDataReceived at the same time
                lock (_process) {
                    Status = ServerStatus.Finished;
                    EndProcessTree(_process.Id);
                    _process.Dispose();
                    _process = null;
                }
            }
        }

        /// <summary>
        /// Listens for a response from the ID server and updates Status as appropriate.
        /// </summary>
        private async void ListenForServer() {
            int delayTime = 0;
            while (Status != ServerStatus.Running && !(delayTime > 10000)) {
                try {
                    System.Threading.Thread.Sleep(delayTime);
                    using (var httpClient = new HttpClient()) {
                        httpClient.Timeout = TimeSpan.FromMinutes(3);
                        var response = await httpClient.GetAsync(_idServerUrl);

                        Status = response.IsSuccessStatusCode ? ServerStatus.Running : ServerStatus.NotStarted;
                    }
                }
                catch (Exception) {
                    delayTime += 1000;
                }
            }
        }

        private void EndProcessTree(int processId) {
            // Killing a dotnet process doesn't seem to kill its children correctly, which
            // causes the program to hang. We can use taskkill to kill the entire process tree.
            ProcessStartInfo info = new ProcessStartInfo() {
                FileName = "taskkill",
                Arguments = $"/PID {processId} /T /F",
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            Process.Start(info).WaitForExit();
        }
    }
}
