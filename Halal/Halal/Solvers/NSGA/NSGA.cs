using Halal.Problems.WorkAssignment;
using Halal.Solvers.GeneticAlgorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Solvers.NSGA
{
    class NSGA
    {
        private WorkAssignment workAssignment;

        public List<Person> People { get; set; }

        public NSGA()
        {
            workAssignment = new WorkAssignment();
            workAssignment.LoadFromFile("Salary.txt");

            People = workAssignment.People;
        }

        public void Solve()
        {
            var result = NsgaSelection(People, 0.1f);
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
                    if (population.Except(new List<Person> { person }).Where(x => x.Quality > person.Quality && x.Salary < person.Salary).Count() == 0)
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
            
            return matingPool;
        }

        private float Distance(Person p1, Person p2) => Math.Abs(p1.Salary - p2.Salary) + Math.Abs(p1.Quality - p2.Quality);

        private float Sharing(float distance) => 1 / distance;
    }
}
