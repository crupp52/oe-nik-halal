using Halal.Problems.WorkAssignment;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Halal.Solvers.NSGA
{
    class NSGA
    {
        private WorkAssignment workAssignment;
        private Stopwatch stopwatch;

        public List<Person> People { get; set; }

        public NSGA()
        {
            stopwatch = new Stopwatch();

            workAssignment = new WorkAssignment();
            workAssignment.LoadFromFile("Salary.txt");

            People = workAssignment.People;
        }

        public void Solve()
        {
            stopwatch.Start();
            var result = NsgaSelection(People, 0.31f);
            stopwatch.Stop();

            ToLog(People, result);
        }

        private List<Person> NsgaSelection(List<Person> population, float fitnessDeg)
        {
            List<Person> matingPool = new List<Person>();
            float fitnessBase = 1;

            while(population.Count > 0)
            {
                List<Person> paretoFront = new List<Person>();

                foreach (Person person in population)
                {
                    if (population.Except(new List<Person> { person }).Where(x => x.Quality >= person.Quality && x.Salary <= person.Salary).Count() == 0)
                    {
                        paretoFront.Add(person);
                    }
                }

                population = population.Except(paretoFront).ToList();

                foreach (Person person in paretoFront)
                {
                    person.SharingValue = paretoFront.Except(new List<Person> { person }).ToArray().Sum(x => Sharing(Distance(x, person)));
                }

                float sumSharing = paretoFront.Sum(x => x.SharingValue);

                foreach (Person person in paretoFront)
                {
                    person.Fitness = fitnessBase * (1 - (person.SharingValue / sumSharing));
                }

                matingPool.AddRange(paretoFront);

                fitnessBase = paretoFront.Min(x => x.Fitness) * fitnessDeg;
            }

            matingPool = matingPool.OrderByDescending(x => x.Fitness).ThenBy(x => x.SharingValue).ToList();

            return matingPool;
        }

        private float Distance(Person p1, Person p2) => Math.Abs(p1.Salary - p2.Salary) + Math.Abs(p1.Quality - p2.Quality);

        private float Sharing(float distance) => 1 / distance;

        private void ToLog(List<Person> oldList, List<Person> newList)
        {
            for (int i = 0; i < oldList.Count; i++)
            {
                Console.WriteLine($"({oldList[i].Quality}; {oldList[i].Salary}) -> ({newList[i].Quality}; {newList[i].Salary})");
            }
        }
    }
}
