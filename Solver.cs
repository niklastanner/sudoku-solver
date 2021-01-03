using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Sudoku_Solver
{
    class Solver
    {
        private static Sudoku sudoku;
        private readonly List<Thread> threads = new List<Thread>();
        private readonly int lifespan = 1000 * 5;

        public Solver(Sudoku sudoku)
        {
            Solver.sudoku = sudoku;
        }

        public void Solve()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            threads.Add(new Thread(SolvingAlgorithms.SimpleElimination));
            threads.Add(new Thread(SolvingAlgorithms.HiddenElimination));

            foreach (Thread thread in threads)
            {
                thread.IsBackground = true;
                thread.Start(sudoku);
            }

            WaitForAllThreads(lifespan);

            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            Console.WriteLine("\nRunTime {0}.{1} Seconds", ts.Seconds, ts.Milliseconds / 10);

            if (ValidateSudoku())
            {
                Console.WriteLine("Solved correctly\n");
            }
            else
            {
                Console.WriteLine("I am way too dumb to solve this sudoku\n");
            }
        }

        private void WaitForAllThreads(int lifespan)
        {
            foreach (Thread thread in threads)
            {
                Stopwatch timeout = Stopwatch.StartNew();
                if (!thread.Join(lifespan))
                {
                    break;
                }
                timeout.Stop();
                lifespan -= (int)timeout.ElapsedMilliseconds;
            }
        }

        #region Validation Methods
        public static List<int> GetNumbersInSquare(int index, Sudoku sudoku)
        {
            List<int> tuple = new List<int>();
            index = (index / 27 * 27) + (index % 9) - (index % 3);

            for (int j = 0; j <= 18; j += 9)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (sudoku.Get(index + j + i) != 0)
                    {
                        tuple.Add(sudoku.Get(index + j + i));
                    }
                }
            }

            return tuple;
        }

        public static List<int> GetNumbersInHorizontalLine(int index, Sudoku sudoku)
        {
            List<int> tuple = new List<int>();
            index = index / 9 * 9;

            for (int i = 0; i < 9; i++)
            {
                if (sudoku.Get(index + i) != 0)
                {
                    tuple.Add(sudoku.Get(index + i));
                }
            }

            return tuple;
        }

        public static List<int> GetNumbersInVerticalLine(int index, Sudoku sudoku)
        {
            List<int> tuple = new List<int>();
            index = index % 9;

            for (int i = 0; i < (73 + index); i += 9)
            {
                if (sudoku.Get(index + i) != 0)
                {
                    tuple.Add(sudoku.Get(index + i));
                }
            }

            return tuple;
        }

        public static bool IsSolved()
        {
            for (int i = 0; i < Sudoku.SIZE; i++)
            {
                if (sudoku.Get(i) == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ValidateSudoku()
        {
            for (int i = 0; i < Sudoku.SIZE; i += 9)
            {
                List<int> Checkblock;

                Checkblock = GetNumbersInSquare(i, sudoku);
                if (Checkblock.Count != 9)
                {
                    return false;
                }
                for (int j = 1; j < 10; j++)
                {
                    if (!Checkblock.Contains(j))
                    {
                        return false;
                    }
                }

                Checkblock = GetNumbersInHorizontalLine(i, sudoku);
                if (Checkblock.Count != 9)
                {
                    return false;
                }
                for (int j = 1; j < 10; j++)
                {
                    if (!Checkblock.Contains(j))
                    {
                        return false;
                    }
                }

                Checkblock = GetNumbersInVerticalLine(i, sudoku);
                if (Checkblock.Count != 9)
                {
                    return false;
                }
                for (int j = 1; j < 10; j++)
                {
                    if (!Checkblock.Contains(j))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion
    }
}
