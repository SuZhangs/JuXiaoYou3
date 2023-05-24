using System.Diagnostics;
using System.IO;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public enum BugLevel : int
    {
        Bug,
        NotImplemented,
        Crash,
    }
    
    public class BugReportVerbose : PendingVerbose
    {
        public string Database { get; set; }
        public string Logs { get; init; }
        public BugLevel Bug { get; set; }
        
        public override void Run()
        {
            var thisAppDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var crashReporter    = Path.Combine(thisAppDirectory, "MigaStudio.Tools.Headerless.exe");
            
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                Arguments       = $"{Database} {(int)Bug} {Logs}",
                FileName        = crashReporter
            });
        }
    }
}