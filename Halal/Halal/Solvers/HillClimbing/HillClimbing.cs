using System;
using System.Collections.Generic;
using Halal.Problems.FunctionApproximation;

namespace Halal.Solvers.HillClimbing
{
    public class HillClimbing
    {
        private static Random rnd = new Random();

        public static void Solve(List<float> searchingField, int ds, int e)
        {
            ValuePair p = new ValuePair
            {
                Input = searchingField[rnd.Next(0,searchingField.Count)],
                
            };
        }
    }
}