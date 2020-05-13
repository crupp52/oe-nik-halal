using Halal.Problems.TravellingSalesmanProblem;
using Halal.Solvers.GeneticAlgorithm;
using Halal.Solvers.HillClimbing;
using Halal.Solvers.NSGA;
using Halal.Solvers.NSGAII;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Halal
{
    public class Menu
    {
        public Menu(IEnumerable<string> items)
        {
            Items = items.ToArray();
        }

        public IReadOnlyList<string> Items { get; }

        public int SelectedIndex { get; private set; } = 0; // nothing selected

        public string SelectedOption => SelectedIndex != -1 ? Items[SelectedIndex] : null;


        public void MoveUp() => SelectedIndex = Math.Max(SelectedIndex - 1, 0);

        public void MoveDown() => SelectedIndex = Math.Min(SelectedIndex + 1, Items.Count - 1);
    }


    // logic for drawing menu list
    public class ConsoleMenuPainter
    {
        readonly Menu menu;

        public ConsoleMenuPainter(Menu menu)
        {
            this.menu = menu;
        }

        public void Paint(int x, int y)
        {
            for (int i = 0; i < menu.Items.Count; i++)
            {
                Console.SetCursorPosition(0, i);

                var bgColor = menu.SelectedIndex == i ? ConsoleColor.Gray : ConsoleColor.Black;
                var fgColor = menu.SelectedIndex == i ? ConsoleColor.Black : ConsoleColor.Gray;

                Console.BackgroundColor = bgColor;
                Console.ForegroundColor = fgColor;
                Console.WriteLine(menu.Items[i]);
            }
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var menu = new Menu(new string[] { "TSP with GA", "FA with HC", "WA with NSGA", "WA with NSGAII" });
            var menuPainter = new ConsoleMenuPainter(menu);

            bool done = false;

            do
            {
                menuPainter.Paint(8, 5);

                var keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow: menu.MoveUp(); break;
                    case ConsoleKey.DownArrow: menu.MoveDown(); break;
                    case ConsoleKey.Enter: done = true; break;
                }
            }
            while (!done);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.Clear();

            if (menu.SelectedOption != null)
            {
                switch (menu.SelectedIndex)
                {
                    case 0:
                        SolveGA();
                        break;
                    case 1:
                        SolveHC();
                        break;
                    case 2:
                        SolveNSGA();
                        break;
                    case 3:
                        SolveNSGAII();
                        break;
                    default:
                        break;
                }
            }

            Console.ReadKey();
        }

        private static void SolveGA()
        {
            GeneticAlgorithm GA = new GeneticAlgorithm(new TravellingSalesmanProblem(), "Towns.txt");

            Console.WriteLine("TSP GA Solver Started");
            Console.WriteLine("Press ESC to stop");

            GA.Solve();

            Console.WriteLine("TSP GA Solver Finished");
        }

        private static void SolveHC()
        {
            HillClimbing hc = new HillClimbing();

            Console.WriteLine("FA HC Solver Started");
            Console.WriteLine("Press ESC to stop");

            hc.Solve(3);

            Console.WriteLine("FA HC Solver Finished");
        }

        private static void SolveNSGA()
        {
            NSGA nsga = new NSGA();

            Console.WriteLine("WA NSGA Solver Started");
            Console.WriteLine("Press ESC to stop");

            nsga.Solve();

            Console.WriteLine("WA NSGA Solver Finished");
        }

        private static void SolveNSGAII()
        {
            NSGAII nsgaii = new NSGAII();

            Console.WriteLine("WA NSGAII Solver Started");
            Console.WriteLine("Press ESC to stop");

            nsgaii.Solve();

            Console.WriteLine("WA NSGAII Solver Finished");
        }
    }
}