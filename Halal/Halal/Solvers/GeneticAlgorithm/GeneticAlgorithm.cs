using System;
using System.Collections.Generic;
using System.Linq;
using Halal.Problems.TravellingSalesmanProblem;
using Halal.Solvers.GeneticAlgorithm.Models;

namespace Halal.Solvers.GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        private static Random rnd = new Random();

        private int iteration;
        private int didntNewBest;
        private Chromosome pBest;

        public TravellingSalesmanProblem TSP { get; set; }
        public int NumberOfPopulation { get; set; }
        public List<Chromosome> Result { get; set; }

        public GeneticAlgorithm(TravellingSalesmanProblem tsp, string filename = "Towns.txt")
        {
            TSP = tsp;

            tsp.LoadTownsFromFile(filename);

            NumberOfPopulation = 50;

            Result = new List<Chromosome>();
        }

        public void Solve()
        {
            Population population = InitializePopulation(new Chromosome() { Towns = TSP.Towns });
            pBest = Evaluation(population);
            Result.Add((Chromosome)pBest.Clone());
            iteration = 0;
            didntNewBest = 0;

            while (didntNewBest < 5000)
            {
                if (pBest.Fitness < Result[Result.Count - 1].Fitness)
                {
                    Result.Add((Chromosome)pBest);
                    Console.WriteLine(this);
                }
                else
                {
                    didntNewBest++;
                }

                Population newPopulation = SelectParents(population);

                while (newPopulation.Chromosomes.Count <= NumberOfPopulation)
                {
                    Population matingPopulation = Selection(population);
                    Chromosome newChromosome = CrossOver(matingPopulation);
                    newChromosome = Mutate(newChromosome);
                    newPopulation.Chromosomes.Add(newChromosome);
                }

                population = (Population)newPopulation.Clone();
                pBest = Evaluation(population);

                iteration++;
            }

            //TSP.SaveTownsToFile("output.log");
        }

        private Chromosome Mutate(Chromosome chromosome)
        {
            for (int i = 0; i < chromosome.Towns.Count/20; i++)
            {
                int a = rnd.Next(0, chromosome.Towns.Count);
                int b = rnd.Next(0, chromosome.Towns.Count);
                Town town = (Town)chromosome.Towns[a].Clone();
                chromosome.Towns[a] = (Town)chromosome.Towns[b].Clone();
                chromosome.Towns[b] = (Town)town.Clone();
            }

            return chromosome;
        }

        private Chromosome CrossOver(Population population)
        {
            Chromosome child = new Chromosome();

            Chromosome parent1 = (Chromosome)population.Chromosomes[rnd.Next(0, population.Chromosomes.Count)].Clone();
            Chromosome parent2 = (Chromosome)population.Chromosomes[rnd.Next(0, population.Chromosomes.Count)].Clone();

            int cutIndex = rnd.Next(0, population.Chromosomes.Count);

            int i = 0;

            while (i < cutIndex)
            {
                child.Towns.Add((Town)parent1.Towns[i].Clone());
                i++;
            }


            while (i < parent2.Towns.Count)
            {
                child.Towns.Add((Town)parent2.Towns[i].Clone());
                i++;
            }

            child.Fitness = TSP.Objective(child.Towns);

            return child;
        }

        private Population Selection(Population population)
        {
            Population matingPoppulation = new Population();

            for (int i = 0; i < rnd.Next(0, population.Chromosomes.Count); i++)
            {
                matingPoppulation.Chromosomes.Add((Chromosome)population.Chromosomes.OrderBy(x => x.Fitness).ToList()[i].Clone());
            }

            while (matingPoppulation.Chromosomes.Count != population.Chromosomes.Count)
            {
                matingPoppulation.Chromosomes.Add((Chromosome)population.Chromosomes[rnd.Next(0, population.Chromosomes.Count)].Clone());
            }

            return matingPoppulation;
        }

        private Population SelectParents(Population population)
        {
            population.Chromosomes = population.Chromosomes.OrderBy(x => x.Fitness).ToList();

            Population newPopulation = new Population()
            {
                Chromosomes = new List<Chromosome>
                {
                    (Chromosome)population.Chromosomes[0].Clone(),
                    (Chromosome)population.Chromosomes[1].Clone(),
                    (Chromosome)population.Chromosomes[2].Clone(),
                    (Chromosome)population.Chromosomes[3].Clone(),
                    (Chromosome)population.Chromosomes[4].Clone(),
                }
            };

            return newPopulation;
        }

        private Population InitializePopulation(Chromosome chromosome)
        {
            Population population = new Population();
            Chromosome newChromosome = (Chromosome)chromosome.Clone();

            for (int i = 0; i < NumberOfPopulation; i++)
            {
                int n = newChromosome.Towns.Count;
                while (n > 1)
                {
                    n--;
                    int k = rnd.Next(n + 1);
                    Town value = (Town)newChromosome.Towns[k].Clone();
                    newChromosome.Towns[k] = (Town)newChromosome.Towns[n].Clone();
                    newChromosome.Towns[n] = value;
                }
                newChromosome.Fitness = TSP.Objective(newChromosome.Towns);
                population.Chromosomes.Add((Chromosome)newChromosome.Clone());
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

        public override string ToString()
        {
            return $"Iteration: {iteration}, Fitness: {Result[Result.Count - 1].Fitness}";
        }
    }
}