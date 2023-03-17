namespace Kse.Algorithms.Samples
{
    using System;
    using System.Collections.Generic;

    
    public class MapPrinter
    {
        public bool IsEqual(Point a, Point b)
         {
             return a.Column == b.Column && a.Row == b.Row;
         }
        public void Print(string[,] map, List<Point> path)
        {
            Point start = path[0];
            Point end = path[^1];

            foreach (Point p in path)
            {
                if (IsEqual(p, start))
                {
                    map[p.Column, p.Row] = "A";
                }
                else if (IsEqual(p, end))
                {
                    map[p.Column, p.Row] = "B";
                }
                else
                {
                    map[p.Column, p.Row] = ".";
                }
            }

            PrintTopLine();
            for (var row = 0; row < map.GetLength(1); row++)
            {
                Console.Write($"{row}\t");
                for (var column = 0; column < map.GetLength(0); column++)
                {
                    Console.Write(map[column, row]);
                }

                Console.WriteLine();
            }
        

            void PrintTopLine()
            {
                Console.Write($" \t");
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    Console.Write(i % 10 == 0? i / 10 : " ");
                }
    
                Console.Write($"\n \t");
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    Console.Write(i % 10);
                }
    
                Console.WriteLine("\n");
            }
        }
    }
}