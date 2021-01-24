using System.Collections.Generic;

namespace Sudoku_Solver
{
    class HiddenElimination : SolvingAlgorithm
    {
        /// <summary>
        /// If the number is the only possibility in a row/column/square
        /// even if there are more possibilities for this field,
        /// then this number will be set.
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
                        int horizontal = index / 9 * 9;
                        int vertical = index % 9;
                        int square = (index / 27 * 27) + (index % 9) - (index % 3);
                        List<int> ownPossibilities = sudoku.GetPossibilities(index);

                        // Get all fields in a row/column/square
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

                    Solver.locker.Release(index);
                }

                index = (index + 1) % Sudoku.SIZE;
            } while (!Solver.IsSolved());
        }
    }
}
