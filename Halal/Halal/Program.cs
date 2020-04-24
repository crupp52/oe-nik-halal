using Halal.Problems.TravellingSalesmanProblem;
using Halal.Solvers.GeneticAlgorithm;
using System;

namespace Halal
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            GeneticAlgorithm GA = new GeneticAlgorithm(new TravellingSalesmanProblem());

            Console.WriteLine("TSP GA Solver Started");
            Console.WriteLine("Press ESC to stop");

            GA.Solve();

            Console.WriteLine("TSP GA Solver Finished");

            Console.ReadKey();
        }
    }
}