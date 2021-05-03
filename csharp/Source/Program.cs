using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace csharp
{
    internal static class Program
    {
        private static Node[,] nodes;

        private static Node start;
        private static Node finish;

        private static int xSize;
        private static int ySize;

        private static void Main()
        {
            var now = DateTime.Now.Millisecond;
            
            InitializeNodes();
            
            AssignValues();
            
            var path =  TraversePath();
            
            Console.WriteLine(DateTime.Now.Millisecond-now);
            
            ShowGrid(path);
            
        }

        private static void InitializeNodes()
        {
            // Get all the lines from the text file, where the maze is located.
            var lines = File.ReadAllLines($"{Directory.GetCurrentDirectory()}../../../../../../input/1.txt");
            // Create the 2-dimensional node array.
            xSize = lines.First().Length;
            ySize = lines.Length;
            
            nodes = new Node[ySize, xSize];
            
            // Loop through the lines and their characters.
            for (var y = 0; y < ySize; y++)
            for (var x = 0; x < xSize; x++)
            {
                // Create a new node.
                nodes[y, x] = new Node
                {
                    X = x,
                    Y = y,
                    Value = lines[y][x] == '#' ? -1 : 0
                };
            }

            start  = nodes[0, 0];
            finish = nodes[ySize-1, xSize-1];
        }

        private static void AssignValues()
        {
            var currentNodes = new List<Node> {start};
            
            AssignValuesRecursive(1, currentNodes);
        }

        private static void AssignValuesRecursive(int value, List<Node> currentNodes)
        {
            if (currentNodes.Any() == false || currentNodes.Contains(finish))
            {
                finish.Value = value;
                
                return;
            }

            var nextNodes = new List<Node>();
            
            foreach (var node in currentNodes)
            {
                node.Value = value;
                
                nextNodes.AddRange(GetNeighNodes(node.X,node.Y).Where(x => x.Value == 0));
            }
            
            AssignValuesRecursive(++value, nextNodes.Distinct().ToList());
        }

        private static List<Node> TraversePath()
        {
            var path = new List<Node> {finish};

            while (path.Last().Value > 1)
            {
                var lastNode = path.Last();

                var nextNode = GetNeighNodes(lastNode.X, lastNode.Y).First(x => x.Value == lastNode.Value - 1);
                
                path.Add(nextNode);
            }

            return path;
        }
        
        private static IEnumerable<Node> GetNeighNodes(int x, int y)
        {
            return new List<(int x, int y)>
            {
                (x + 1, y + 0),
                (x - 1, y + 0),
                (x + 0, y + 1),
                (x + 0, y - 1),
            }.Where(pos =>
                pos.x >= 0 &&
                pos.y >= 0 &&
                pos.x < xSize &&
                pos.y < ySize
            ).Select(pos => nodes[pos.y, pos.x]).ToList();
        }

        private static void ShowGrid(List<Node> path)
        {
            for (var y = 0; y < nodes.GetLength(0); y++)
            {
                var txt = "";

                for (var x = 0; x < nodes.GetLength(1); x++)
                {
                    txt += nodes[y, x].Value == -1 ? "#" : path.Contains(nodes[y, x]) ? "." : " " ;
                }

                Console.WriteLine(txt);
            }
        }
    }

    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Value { get; set; }
    }
}