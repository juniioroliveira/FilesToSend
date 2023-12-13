using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesToSend
{
    public class Directories
    {
        public static string RootPath { get; set; } = "C:\\Drogaleste\\Socket\\Modules\\FilesToSend";
        public static string ProgramPath { get; set; } = Path.Combine(RootPath, "Program");
        public static string LogPath { get; set; } = Path.Combine(ProgramPath, "Logs");

        public static void Check()
        {
            if (!Directory.Exists(RootPath)) Directory.CreateDirectory(RootPath);
            if (!Directory.Exists(LogPath)) Directory.CreateDirectory(LogPath);
            if (!Directory.Exists(ProgramPath)) { Directory.CreateDirectory(ProgramPath); } //else { Directory.Delete(ModulesPath, true); Directory.CreateDirectory(ModulesPath);  };
        }
    }
}
