using Halal.Problems.WorkAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Halal.Solvers.NSGAII
{
    class NSGAII
    {
        public List<Person> Solve()
        {
            List<Person> p = InitializePopulation();

            List<Person> q = new List<Person>();
            var pBest = new List<Person>();

            int i = 0;

            while (i < 10)
            {
                Selection(p, q);

                pBest = p.Where(x => x.Rank == 1).ToList();
                q = MakeNewPopulation(p);

                i++;
            }

            return pBest.Distinct().ToList();
        }

        private void NonDominatedSort(List<Person> people)
        {
            List<Person> paretoFront = new List<Person>();
            foreach (Person person in people)
            {
                person.N = 0;
                person.S = new List<Person>();
            }

            foreach (Person p in people)
            {
                foreach (Person q in people.Except(new List<Person> { p }).ToList())
                {
                    if (q.Quality >= p.Quality && q.Salary <= p.Salary)
                    {
                        p.N++;
                        q.S.Add(p);
                    }
                }

                if (p.N == 0)
                {
                    paretoFront.Add(p);
                }
            }

            int pfi = 1;


            while (paretoFront.Count != 0)
            {
                List<Person> newParetoFront = new List<Person>();

                foreach (Person p in paretoFront)
                {
                    p.Rank = pfi;

                    foreach (Person q in p.S)
                    {
                        q.N--;

                        if (q.N == 0)
                        {
                            newParetoFront.Add(q);
                        }
                    }
                }

                pfi++;

                paretoFront = newParetoFront;
            }
        }

        public void CrowdingDistance(List<Person> people)
        {
            foreach (Person p in people)
            {
                p.Distance = 0;
            }

            people = people.OrderByDescending(x => x.Quality).ToList();
            people[0].Distance = float.MaxValue;
            people[people.Count - 1].Distance = float.MaxValue;
            for (int i = 1; i < people.Count - 1; i++)
            {
                people[i].Distance = people[i].Distance + (people[i + 1].Quality - people[i - 1].Quality) / (people[people.Count - 1].Quality - people[0].Quality); 
            }

            people = people.OrderBy(x => x.Salary).ToList();
            people[0].Distance = float.MaxValue;
            people[people.Count - 1].Distance = float.MaxValue;
            for (int i = 1; i < people.Count - 1; i++)
            {
                people[i].Distance = people[i].Distance + (people[i + 1].Salary - people[i - 1].Salary) / (people[people.Count - 1].Salary - people[0].Salary);
            }
        }

        public void Selection(List<Person> p, List<Person> q)
        {
            List<Person> matingPool = new List<Person>();
            List<Person> r = p;
            r.AddRange(q);

            NonDominatedSort(r);

            int pfi = 1;

            const int N = 6;

            while (matingPool.Count < N)
            {
                List<Person> paretoFront = r.Where(x => x.Rank == pfi).ToList();

                if (matingPool.Count + paretoFront.Count <= N)
                {
                    matingPool.AddRange(paretoFront);
                }
                else
                {
                    CrowdingDistance(paretoFront);
                    paretoFront = paretoFront.OrderBy(x => x.Distance).ToList();

                    for (int i = 0; i < N - matingPool.Count; i++)
                    {
                        matingPool.Add(paretoFront[i]);
                    }
                }

                pfi++;
            }
        }

        private List<Person> InitializePopulation()
        {
            WorkAssignment wa = new WorkAssignment();
            wa.GeneratePeople(50);

            return wa.People;
        }

        private List<Person> MakeNewPopulation(List<Person> p)
        {
            return p.OrderBy(x => x.Distance).Take(p.Count / 5).ToList();
        }
    }
}
