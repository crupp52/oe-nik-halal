using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Halal.Problems.TravellingSalesmanProblem
{
    public class TravellingSalesmanProblem
    {
        public List<Town> Towns { get; set; }

        public void LoadTownsFromFile(string filename)
        {
            Towns = new List<Town>();
            string[] lines = File.ReadAllLines(filename);

            foreach (string line in lines)
            {
                string[] columns = line.Split('\t');
                Towns.Add(new Town
                {
                    X = float.Parse(columns[0]),
                    Y = float.Parse(columns[1])
                });
            }
        }

        public void SaveTownsToFile(string filename)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Town town in Towns)
            {
                sb.AppendLine($"{town.X}\t{town.Y}");
            }

            File.WriteAllText(filename, sb.ToString(), Encoding.UTF8);
        }

        public float Objective(List<Town> route)
        {
            float sumLength = 0;

            for (int i = 0; i < route.Count - 1; i++)
            {
                Town t1 = route[i];
                Town t2 = route[i + 1];

                sumLength += (float) Math.Sqrt(Math.Pow(t1.X - t2.X, 2) + Math.Pow(t1.Y - t2.X, 2));
            }

            return sumLength;
        }
    }
}