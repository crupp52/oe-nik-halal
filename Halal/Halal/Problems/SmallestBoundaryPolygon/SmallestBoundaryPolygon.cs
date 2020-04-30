using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Problems.SmallestBoundaryPolygon
{
    class SmallestBoundaryPolygon
    {
        public List<Point> Points { get; set; }

        public void LoadTownsFromFile(string filename)
        {
            Points = new List<Point>();
            string[] lines = File.ReadAllLines(filename);

            foreach (string line in lines)
            {
                string[] columns = line.Split('\t');
                Points.Add(new Point
                {
                    X = float.Parse(columns[0]),
                    Y = float.Parse(columns[1])
                });
            }
        }

        public void SaveTownsToFile(string filename)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Point town in Points)
            {
                sb.AppendLine($"{town.X}\t{town.Y}");
            }

            File.WriteAllText(filename, sb.ToString(), Encoding.UTF8);
        }

        public float DistanceFromLine(Point lp1, Point lp2, Point p)
        {
            return ((lp2.Y - lp1.Y) * p.X - (lp2.X - lp1.X) * p.Y + lp2.X * lp1.Y - lp2.Y * lp1.X) /
                (float)Math.Sqrt(
                    Math.Pow(lp2.Y - lp1.Y, 2) + 
                    Math.Pow(lp2.X - lp1.X, 2));
        }

        public float OuterDistanceToBoundary(List<Point> solution)
        {
            float sumMinDistance = 0;

            for (int i = 0; i < Points.Count; i++)
            {
                float minDistance = 0;
                for (int j = 0; j < solution.Count; j++)
                {
                    float actualDistance = DistanceFromLine(solution[j], solution[(j + 1) % solution.Count], Points[i]);
                    if (j == 0 || actualDistance < minDistance)
                    {
                        minDistance = actualDistance;
                    }
                }

                if (minDistance < 0)
                {
                    sumMinDistance += -minDistance;
                }
            }

            return sumMinDistance;
        }

        public float LengthOfBoundary(List<Point> solution)
        {
            float sumLength = 0f;

            for (int i = 0; i < solution.Count; i++)
            {
                Point p1 = solution[i];
                Point p2 = solution[(i + 1) % solution.Count];
                sumLength += (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
            }

            return sumLength;
        }

        public float Objective(List<Point> solution)
        {
            return LengthOfBoundary(solution);
        }

        public float Contraint(List<Point> solution)
        {
            return -OuterDistanceToBoundary(solution);
        }
    }
}
