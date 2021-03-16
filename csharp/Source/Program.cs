using System;
using System.IO;
using System.Linq;

namespace csharp
{
    static class Program
    {
        static int[,] nodes;
        

        static void Main()
        {

            for (var y = 0; y < lines.Length; y++)
            {
                var line = "";
                
                for (var x = 0; x < lines.First().Length; x++)
                {
                    line += lines[y][x];
                }

                
            }
        }

        static void CreateGrid()
        {
            var lines = File.ReadAllLines($"{Directory.GetCurrentDirectory()}/../../../../input/1.txt");

            nodes = new int[,1];

        }
        
    }

}
