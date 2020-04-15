using System;
using System.Collections.Generic;
using System.IO;

namespace Halal.Problems.FunctionApproximation
{
    public class FunctionApproximation
    {
        public List<ValuePair> KnownValues { get; set; }

        public void LoadKnownValuesFromFile(string filename)
        {
            string[] fileInput = File.ReadAllLines(filename);

            foreach (string line in fileInput)
            {
                string[] lineElements = line.Split(';');
                KnownValues.Add(new ValuePair
                {
                    Input = float.Parse(lineElements[0]),
                    Output = float.Parse(lineElements[1])
                });
            }
        }

        public float Objective(List<float> coefficients)
        {
            float sumDiff = 0;

            foreach (ValuePair valuePair in KnownValues)
            {
                float x = valuePair.Input;
                float y = coefficients[0] * (float) Math.Pow(x - coefficients[1], 3) + coefficients[2] * (float) Math.Pow(x - coefficients[3], 2) + coefficients[4];

                float diff = (float) Math.Pow(y - valuePair.Output, 2);
                sumDiff += diff;
            }

            return sumDiff;
        }
    }
}