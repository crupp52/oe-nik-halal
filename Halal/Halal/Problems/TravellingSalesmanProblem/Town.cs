using System;

namespace Halal.Problems.TravellingSalesmanProblem
{
    public class Town : ICloneable
    {
        public float X { get; set; }
        public float Y { get; set; }

        public object Clone()
        {
            return new Town()
            {
                X = this.X,
                Y = this.Y
            };
        }
    }
}