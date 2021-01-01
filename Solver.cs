using System;
using System.Collections.Generic;

namespace Sudoku_Solver
{
    class Solver
    {
        private Sudoku sudoku;

        public Solver(Sudoku sudoku)
        {
            this.sudoku = sudoku;
        }

        public Sudoku Solve()
        {
            int i = 0;
            do
            {
                List<int> toRemove = CheckSquare(i);
                List<int> horizontal = CheckHorizontalLine(i);
                foreach (int v in horizontal)
                {
                    if (!toRemove.Contains(v))
                    {
                        toRemove.Add(v);
                    }
                }

                List<int> vertical = CheckVerticalLine(i);
                foreach (int v in vertical)
                {
                    if (!toRemove.Contains(v))
                    {
                        toRemove.Add(v);
                    }
                }

                List<int> possibilities = sudoku.GetPossibilities(i);
                foreach (int v in toRemove)
                {
                    if (possibilities.Contains(v))
                    {
                        sudoku.RemovePossibility(i, v);
                    }
                }

                if (possibilities.Count == 1)
                {
                    sudoku.Set(i, possibilities[0]);
                    sudoku.RemovePossibility(i, possibilities[0]);
                }

                i = (i + 1) % Sudoku.SIZE;
            } while (!CheckSolved());

            Console.WriteLine("Solved it in " + i + " iterations");

            return sudoku;
        }

        private List<int> CheckSquare(int index)
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

        private List<int> CheckHorizontalLine(int index)
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

        private List<int> CheckVerticalLine(int index)
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

        private bool CheckSolved()
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
    }
}
