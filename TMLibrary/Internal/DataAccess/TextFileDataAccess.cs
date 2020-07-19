using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLibrary.Internal.DataAccess
{
    public static class TextFileDataAccess
    {        
        private static readonly string DataPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\TMTextFileDatabase"));
        //private static readonly string DataPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\TournamentManager\TMTextFileDatabase\"));

        public static string FullFilePath(this string fileName)
        {
            return $"{DataPath}\\{fileName}";
        }

        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        public static void SaveFile(this string file, List<string> content)
        {
            
        }

    }
}
