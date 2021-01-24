using System.Collections.Generic;

namespace Sudoku_Solver
{
    class SimpleElimination : SolvingAlgorithm
    {
        /// <summary>
        /// Reduce possibilities as low as possible
        /// with known numbers in row/column/square
        /// </summary>

        public void Solve(object param)
        {
            Sudoku sudoku = (Sudoku)param;

            int index = 0;
            do
            {
                if (Solver.locker.Acquire(index))
                {
                    if (sudoku.GetField(index).Value == 0)
                    {
                        List<int> toRemove = Solver.GetNumbersInSquare(index, sudoku);
                        List<int> horizontal = Solver.GetNumbersInRow(index, sudoku);
                        foreach (int v in horizontal)
                        {
                            if (!toRemove.Contains(v))
                            {
                                toRemove.Add(v);
                            }
                        }

                        List<int> vertical = Solver.GetNumbersInColumn(index, sudoku);
                        foreach (int v in vertical)
                        {
                            if (!toRemove.Contains(v))
                            {
                                toRemove.Add(v);
                            }
                        }

                        List<int> possibilities = sudoku.GetPossibilities(index);
                        foreach (int v in toRemove)
                        {
                            if (possibilities.Contains(v))
                            {
                                sudoku.RemovePossibility(index, v);
                            }
                        }

                        if (possibilities.Count == 1)
                        {
                            sudoku.Set(index, possibilities[0]);
                            sudoku.RemovePossibility(index, possibilities[0]);
                        }
                    }

                    Solver.locker.Release(index);
                }

                index = (index + 1) % Sudoku.SIZE;
            } while (!Solver.IsSolved());
        }
    }
}
