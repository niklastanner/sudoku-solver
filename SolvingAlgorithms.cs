using System.Collections.Generic;

namespace Sudoku_Solver
{
    static class SolvingAlgorithms
    {
        private static LockerList<int> locker = new LockerList<int>();

        #region Simple Elimination
        /// <summary>
        /// A cell with only one possibility should be set
        /// </summary>

        public static void SimpleElimination(object param)
        {
            Sudoku sudoku = (Sudoku)param;

            int index = 0;
            do
            {
                if (locker.Add(index))
                {
                    if (sudoku.GetField(index).Value == 0)
                    {
                        List<int> toRemove = Solver.GetNumbersInSquare(index, sudoku);
                        List<int> horizontal = Solver.GetNumbersInHorizontalLine(index, sudoku);
                        foreach (int v in horizontal)
                        {
                            if (!toRemove.Contains(v))
                            {
                                toRemove.Add(v);
                            }
                        }

                        List<int> vertical = Solver.GetNumbersInVerticalLine(index, sudoku);
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

                    index = (index + 1) % Sudoku.SIZE;
                    locker.Remove(index);
                }
            } while (!Solver.IsSolved());
        }
        #endregion

        #region Hidden Elimination
        /// <summary>
        /// If the number is the only possibility in a row/square
        /// even if there are more possibilities for this field,
        /// then this number will be set.
        /// </summary>

        public static void HiddenElimination(object param)
        {
            Sudoku sudoku = (Sudoku)param;

            int index = 0;
            do
            {
                if (locker.Add(index))
                {
                    if (sudoku.GetField(index).Value == 0)
                    {
                        int horizontal = index / 9 * 9;
                        int vertical = index % 9;
                        int square = (index / 27 * 27) + (index % 9) - (index % 3);
                        List<int> ownPossibilities = sudoku.GetPossibilities(index);

                        // Get all fields in a row or square
                        foreach (int value in ownPossibilities)
                        {
                            Field field;
                            bool foundHorizontal = false;
                            bool foundVertical = false;
                            bool foundSquare = false;

                            for (int i = 0; i < 9; i++)
                            {
                                field = sudoku.GetField(horizontal + i);
                                if ((horizontal + i) != index && (field.Value == value || field.GetPossibilities().Contains(value)))
                                {
                                    foundHorizontal = true;
                                    break;
                                }
                            }
                            for (int i = 0; i < (73 + vertical); i += 9)
                            {
                                field = sudoku.GetField(vertical + i);
                                if ((vertical + i) != index && (field.Value == value || field.GetPossibilities().Contains(value)))
                                {
                                    foundVertical = true;
                                    break;
                                }
                            }
                            for (int j = 0; j <= 18; j += 9)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    field = sudoku.GetField(square + i + j);
                                    if ((square + i + j) != index && (field.Value == value || field.GetPossibilities().Contains(value)))
                                    {
                                        foundSquare = true;
                                        break;
                                    }
                                }
                            }

                            if (!foundHorizontal || !foundVertical || !foundSquare)
                            {
                                sudoku.Set(index, value);
                                sudoku.RemoveAllPossibilities(index);
                            }
                        }
                    }

                    index = (index + 1) % Sudoku.SIZE;
                    locker.Remove(index);
                }
            } while (!Solver.IsSolved());
        }
        #endregion
    }
}
