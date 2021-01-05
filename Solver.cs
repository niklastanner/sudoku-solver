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
            Console.WriteLine("Trying to solve the following Sudoku:");
            sudoku.PrintSudoku();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            threads.Add(new Thread(SolvingAlgorithms.FillUniqueFields));
            threads.Add(new Thread(SolvingAlgorithms.SimpleElimination));
            threads.Add(new Thread(SolvingAlgorithms.HiddenElimination));
            threads.Add(new Thread(SolvingAlgorithms.NakedPair));

            foreach (Thread thread in threads)
            {
                thread.IsBackground = true;
                thread.Start(sudoku);
            }

            WaitForAllThreads(lifespan);/*
            while (true) ;*/

            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            Console.WriteLine("\nRunTime {0}.{1} Seconds", ts.Seconds, ts.Milliseconds);

            if (ValidateSudoku())
            {
                Console.WriteLine("Solved correctly\n");
            }
            else
            {
                Console.WriteLine("I am way too dumb to solve this sudoku\n");
            }

            sudoku.PrintSudoku();
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

        #region Support Methods
        public static List<Field> GetFieldsInSquare(int index, Sudoku sudoku)
        {
            List<Field> tuple = new List<Field>();
            index = (index / 27 * 27) + (index % 9) - (index % 3);

            for (int j = 0; j <= 18; j += 9)
            {
                for (int i = 0; i < 3; i++)
                {
                    tuple.Add(sudoku.GetField(index + j + i));
                }
            }

            return tuple;
        }

        public static List<Field> GetFieldsInRow(int index, Sudoku sudoku)
        {
            List<Field> tuple = new List<Field>();
            index = index / 9 * 9;

            for (int i = 0; i < 9; i++)
            {
                tuple.Add(sudoku.GetField(index + i));
            }

            return tuple;
        }
        public static List<Field> GetFieldsInColumn(int index, Sudoku sudoku)
        {
            List<Field> tuple = new List<Field>();
            index = index % 9;

            for (int i = 0; i < (73 + index); i += 9)
            {
                tuple.Add(sudoku.GetField(index + i));
            }

            return tuple;
        }

        public static List<int> GetNumbersInSquare(int index, Sudoku sudoku)
        {
            List<int> tuple = new List<int>();
            List<Field> collection = GetFieldsInSquare(index, sudoku);

            foreach (Field field in collection)
            {
                if (field.Value != 0)
                {
                    tuple.Add(field.Value);
                }
            }

            return tuple;
        }

        public static List<int> GetNumbersInRow(int index, Sudoku sudoku)
        {
            List<int> tuple = new List<int>();
            List<Field> collection = GetFieldsInRow(index, sudoku);

            foreach (Field field in collection)
            {
                if (field.Value != 0)
                {
                    tuple.Add(field.Value);
                }
            }

            return tuple;
        }

        public static List<int> GetNumbersInColumn(int index, Sudoku sudoku)
        {
            List<int> tuple = new List<int>();
            List<Field> collection = GetFieldsInColumn(index, sudoku);

            foreach (Field field in collection)
            {
                if (field.Value != 0)
                {
                    tuple.Add(field.Value);
                }
            }

            return tuple;
        }
        #endregion

        #region Validation Methods
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

                Checkblock = GetNumbersInRow(i, sudoku);
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

                Checkblock = GetNumbersInColumn(i, sudoku);
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
