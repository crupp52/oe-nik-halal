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

        public void SaveToFile(string filename = "wa_nsgaii_input.txt")
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("100");

            foreach (Person person in People)
            {
                stringBuilder.AppendLine($"{person.Salary}\t{person.Quality}");
            }

            File.WriteAllText(filename, stringBuilder.ToString());
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

        public void GeneratePeople(int numberOfPeople)
        {
            Random rnd = new Random();

            People = new List<Person>();

            while (People.Count != numberOfPeople)
            {
                Person person = new Person() { Quality = rnd.Next(0, 11) / 10f, Salary = rnd.Next(100, 5001) };

                if (People.Where(x=> x.Salary == person.Salary && x.Quality == person.Quality).Count() == 0)
                {
                    People.Add(person);
                }
            }

            SaveToFile();
        }
    }
}
