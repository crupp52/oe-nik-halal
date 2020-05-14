using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Halal.Problems.TravellingSalesmanProblem;
using Halal.Solvers.GeneticAlgorithm.Models;

namespace Halal.Solvers.GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        private static Random rnd = new Random();
        private static object _lock = new object();
        private static Stopwatch stopwatch = new Stopwatch();

        private int iteration;
        private int didntNewBest;
        private Chromosome pBest;
        private StringBuilder logStringBuilder;

        public TravellingSalesmanProblem TSP { get; set; }
        public int NumberOfPopulation { get; set; }
        public double RateOfElitism { get; set; }
        public double RateOfMutation { get; set; }
        public List<Chromosome> Result { get; set; }

        public GeneticAlgorithm(TravellingSalesmanProblem tsp, string filename = "Towns.txt")
        {
            TSP = tsp;

            tsp.LoadTownsFromFile(filename);

            NumberOfPopulation = 50;
            RateOfMutation = 0.01;
            RateOfElitism = 0.1;

            Result = new List<Chromosome>();

            logStringBuilder = new StringBuilder();
        }

        public void Solve()
        {
            stopwatch.Start();

            Population population = InitializePopulation(new Chromosome() { Towns = TSP.Towns });
            pBest = Evaluation(population);
            Result.Add((Chromosome)pBest.Clone());
            iteration = 0;

            Task.Run(() => GetInformation());
            do
            {
                while (!Console.KeyAvailable)
                {
                    if (pBest.Fitness < Result[Result.Count - 1].Fitness)
                    {
                        Result.Add((Chromosome)pBest);
                        ToLog();
                    }

                    Population newPopulation = SelectParents(population);

                    while (newPopulation.Chromosomes.Count <= NumberOfPopulation)
                    {
                        Population matingPopulation = Selection(population);
                        Chromosome newChromosome = CrossOver(matingPopulation);
                        newChromosome = Mutate(newChromosome);
                        newPopulation.Chromosomes.Add(newChromosome);
                    }

                    population = newPopulation;
                    pBest = Evaluation(population);

                    iteration++;
                }

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            stopwatch.Stop();

            TSP.Towns = pBest.Towns;
            SaveLog();
            TSP.SaveTownsToFile("output.log");
        }

        private Chromosome Mutate(Chromosome chromosome)
        {
            for (int i = 0; i < chromosome.Towns.Count * RateOfMutation; i++)
            {
                int a = rnd.Next(0, chromosome.Towns.Count);
                int b = rnd.Next(0, chromosome.Towns.Count);

                Town town = new Town
                {
                    X = chromosome.Towns[a].X,
                    Y = chromosome.Towns[a].Y,
                };

                chromosome.Towns[a] = new Town 
                { 
                    X = chromosome.Towns[b].X,
                    Y = chromosome.Towns[b].Y
                };

                chromosome.Towns[b] = new Town 
                {
                    X = town.X,
                    Y = town.Y
                };
            }

            return chromosome;
        }

        private Chromosome CrossOver(Population population)
        {
            Chromosome child = new Chromosome();

            Chromosome parent1 = population.Chromosomes[rnd.Next(0, population.Chromosomes.Count)];
            Chromosome parent2 = population.Chromosomes[rnd.Next(0, population.Chromosomes.Count)];

            int cutIndex = rnd.Next(0, parent1.Towns.Count);

            int i = 0;

            while (i < cutIndex)
            {
                child.Towns.Add(parent1.Towns[i]);
                i++;
            }

            foreach (Town town in parent2.Towns)
            {
                if (child.Towns.Where(x => (x.X == town.X && x.Y == town.Y)).FirstOrDefault() == null)
                {
                    child.Towns.Add(town);
                }
            }

            child.Fitness = TSP.Objective(child.Towns);

            return child;
        }

        private Population Selection(Population population)
        {
            Population matingPoppulation = new Population();

            for (int i = 0; i < rnd.Next(0, population.Chromosomes.Count); i++)
            {
                matingPoppulation.Chromosomes.Add(population.Chromosomes.OrderBy(x => x.Fitness).ToList()[i]);
            }

            while (matingPoppulation.Chromosomes.Count != population.Chromosomes.Count)
            {
                matingPoppulation.Chromosomes.Add(population.Chromosomes[rnd.Next(0, population.Chromosomes.Count)]);
            }

            return matingPoppulation;
        }

        private Population SelectParents(Population population)
        {
            population.Chromosomes = population.Chromosomes.OrderBy(x => x.Fitness).ToList();

            Population newPopulation = new Population();

            for (int i = 0; i < NumberOfPopulation * RateOfElitism / 4; i++)
            {
                newPopulation.Chromosomes.Add(population.Chromosomes[i]);
            }

            return newPopulation;
        }

        private Population InitializePopulation(Chromosome chromosome)
        {
            Population population = new Population();
            Chromosome newChromosome = chromosome;

            for (int i = 0; i < NumberOfPopulation; i++)
            {
                int n = newChromosome.Towns.Count;
                while (n > 1)
                {
                    n--;
                    int k = rnd.Next(n + 1);
                    Town value = newChromosome.Towns[k];
                    newChromosome.Towns[k] = newChromosome.Towns[n];
                    newChromosome.Towns[n] = value;
                }
                newChromosome.Fitness = TSP.Objective(newChromosome.Towns);
                population.Chromosomes.Add(newChromosome);
            }

            return population;
        }

        private Chromosome Evaluation(Population population)
        {
            Chromosome pBest = null;

            foreach (Chromosome chromosome in population.Chromosomes)
            {
                chromosome.Fitness = TSP.Objective(chromosome.Towns);
                if (pBest == null || chromosome.Fitness < pBest.Fitness)
                {
                    pBest = (Chromosome)chromosome.Clone();
                }
            }

            return pBest;
        }

        private void ToLog()
        {
            logStringBuilder.AppendLine("Clear");
            logStringBuilder.AppendLine($"Iteration\t{iteration}");
            logStringBuilder.AppendLine($"Fitness\t{pBest.Fitness}");
            foreach (Town town in pBest.Towns)
            {
                logStringBuilder.AppendLine($"Point\t{town.X}\t{town.Y}\tBlue");
            }

            for (int i = 0; i < pBest.Towns.Count - 1; i++)
            {
                logStringBuilder.AppendLine($"Arrow\t{pBest.Towns[i].X}\t{pBest.Towns[i].Y}\t{pBest.Towns[i + 1].X}\t{pBest.Towns[i + 1].Y}\tred");
            }
        }

        private void SaveLog()
        {
            File.WriteAllText($"TSP_GA_{DateTime.Now.ToString("yyyyddMM")}.log", logStringBuilder.ToString());
        }

        private void GetInformation()
        {
            while (true)
            {
                lock (_lock)
                {
                    Console.SetCursorPosition(0, 2);
                    Console.WriteLine($"Iteration: {iteration}\tFitness: {Result[Result.Count - 1].Fitness}\tElapsed time: {stopwatch.Elapsed}");
                }

                Thread.Sleep(300);
            }
        }

        public override string ToString()
        {
            return $"Iteration: {iteration}, Fitness: {Result[Result.Count - 1].Fitness}";
        }
    }
}