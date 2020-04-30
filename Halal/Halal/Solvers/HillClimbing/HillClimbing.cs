using Halal.Problems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Solvers.HillClimbing
{
    class HillClimbing
    {
        private static Random rnd = new Random();

        public void Solve(List<IKeyValuePair> searchingField, int epsilon)
        {
            IKeyValuePair point = searchingField[rnd.Next(0, searchingField.Count)];

        }
    }
}
