using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Problems.WorkAssignment
{
    class WorkAssignment
    {
        public List<Person> People { get; set; }
        public float RequestedTime { get; set; }

        public void LoadFromFile(string filename)
        {
            string[] input = File.ReadAllLines(filename, Encoding.UTF8);

            People = new List<Person>();

            RequestedTime = float.Parse(input[0]);

            for (int i = 1; i < input.Length; i++)
            {
                string[] parameters = input[i].Split('\t');

                People.Add(new Person
                {
                    Salary = float.Parse(parameters[0].Replace('.', ',')),
                    Quality = float.Parse(parameters[1].Replace('.', ','))
                });
            }
        }

        public float SumSalary(List<float> solution)
        {
            float sum = 0;

            for (int i = 0; i < solution.Count; i++)
            {
                sum += solution[i] * People[i].Salary;
            }

            return sum;
        }

        public float AvgQuality(List<float> solution)
        {
            float sum = 0;

            for (int i = 0; i < solution.Count; i++)
            {
                sum += solution[i] * People[i].Quality;
            }

            return sum / RequestedTime;
        }
    }
}
