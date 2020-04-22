using Halal.Problems.TravellingSalesmanProblem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Solvers.GeneticAlgorithm.Models
{
    class Population : ICloneable
    {
        public float Fitness { 
            get
            {
                return Chromosomes.OrderBy(x => x.Fitness).First().Fitness;
            }
        }
        public List<Chromosome> Chromosomes { get; set; }

        public Population()
        {
            Chromosomes = new List<Chromosome>();
        }

        public object Clone()
        {
            return new Population()
            {
                Chromosomes = Chromosomes.Select(x => (Chromosome)x.Clone()).ToList()
            };
        }
    }
}
