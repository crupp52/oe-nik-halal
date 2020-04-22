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

            GA.Solve();

            Console.ReadKey();
        }
    }
}