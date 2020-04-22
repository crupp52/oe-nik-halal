using Halal.Problems.TravellingSalesmanProblem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Solvers.GeneticAlgorithm.Models
{
    public class Chromosome : ICloneable
    {
        public float Fitness { get; set; }
        public List<Town> Towns { get; set; }

        public Chromosome()
        {
            Fitness = float.MaxValue;
            Towns = new List<Town>();
        }

        public object Clone()
        {
            return new Chromosome()
            {
                Fitness = this.Fitness,
                Towns = Towns.Select(x => (Town)x.Clone()).ToList()
            };
        }
    }
}
