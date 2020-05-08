using Halal.Problems;
using Halal.Problems.FunctionApproximation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Solvers.HillClimbing
{
    class HillClimbing
    {
        private static Random rnd = new Random();

        private Stopwatch stopwatch;

        private FunctionApproximation fa;
        private int iteration;
        private float[] actualCoefficients;

        public HillClimbing()
        {
            fa = new FunctionApproximation();
            fa.LoadKnownValuesFromFile("FuncAppr1.txt");
        }

        public void Solve(int epsilon)
        {
            actualCoefficients = new float[] { 1, 1, 1, 1, 1 };

            stopwatch = new Stopwatch();

            stopwatch.Start();
            iteration = 0;

            Log();

            for (int i = 0; i < 5; i++)
            {
                bool stuck = false;

                while (!stuck)
                {
                    iteration++;

                    float[] newCoefficients = (float[])actualCoefficients.Clone();
                    float[] temp = (float[])newCoefficients.Clone();
                    float value = temp[i] - epsilon;

                    for (int j = 0; j < epsilon * 10; j++)
                    {
                        value += 0.1f;
                        temp[i] = value;

                        if (fa.Objective(temp.ToList()) < fa.Objective(newCoefficients.ToList()))
                        {
                            newCoefficients = (float[])temp.Clone();
                        }
                    }

                    if (fa.Objective(newCoefficients.ToList()) < fa.Objective(actualCoefficients.ToList()))
                    {
                        actualCoefficients = (float[])newCoefficients.Clone();
                    }
                    else
                    {
                        stuck = true;
                    }

                    Log();
                }
            }

            fa.StoreResultToFile(actualCoefficients);

            stopwatch.Stop();

            Log();
        }

        private void Log()
        {
            string parameters = "";
            for (int i = 0; i < actualCoefficients.Length; i++)
            {
                parameters += $"{i}: {actualCoefficients[i]} ";
            }

            Console.SetCursorPosition(0, 2);
            Console.WriteLine($"Iteration: {iteration}\tFitness: {fa.Objective(actualCoefficients.ToList())}\tParameters: {parameters}\tElapsed time: {stopwatch.Elapsed}");
        }
    }
}
