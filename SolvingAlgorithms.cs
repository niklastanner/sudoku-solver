using System.Collections.Generic;

namespace Sudoku_Solver
{
    static class SolvingAlgorithms
    {
        private static readonly LockerList<int> locker = new LockerList<int>();

        #region Fill Unique Fields
        /// <summary>
        /// Fields with only one possibility will be set
        /// </summary>
        public static void FillUniqueFields(object param)
        {
            Sudoku sudoku = (Sudoku)param;

            int index = 0;
            do
            {
                if (locker.Acquire(index))
                {
                    Field field = sudoku.GetField(index);
                    if (field.Value == 0 && field.GetPossibilities().Count == 1)
                    {
                        field.Value = field.GetPossibilities()[0];
                        field.RemoveAllPossibilities();
                    }
                    locker.Release(index);
                }

                index = (index + 1) % Sudoku.SIZE;
            } while (!Solver.IsSolved());
        }
        #endregion

        #region Simple Elimination
        /// <summary>
        /// Reduce possibilities as low as possible
        /// with known numbers in row/column/square
        /// </summary>

        public static void SimpleElimination(object param)
        {
            Sudoku sudoku = (Sudoku)param;

            int index = 0;
            do
            {
                if (locker.Acquire(index))
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

                    locker.Release(index);
                }

                index = (index + 1) % Sudoku.SIZE;
            } while (!Solver.IsSolved());
        }
        #endregion

        #region Hidden Elimination
        /// <summary>
        /// If the number is the only possibility in a row/column/square
        /// even if there are more possibilities for this field,
        /// then this number will be set.
        /// </summary>

        public static void HiddenElimination(object param)
        {
            Sudoku sudoku = (Sudoku)param;

            int index = 0;
            do
            {
                if (locker.Acquire(index))
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

                    locker.Release(index);
                }

                index = (index + 1) % Sudoku.SIZE;
            } while (!Solver.IsSolved());
        }
        #endregion

        #region Naked Group
        /// <summary>
        /// Two cells in a row/column/square having onlythe same 
        /// group (pair/triple/quad) of numbers are called a naked pair/triple/quad.
        /// All other appearances of the numbers in
        /// the same row/column/square can be eliminated.
        /// </summary>

        public static void NakedGroup(object param)
        {
            Sudoku sudoku = (Sudoku)param;

            do
            {
                // Look for each row
                for (int i = 0; i <= 72; i += 9)
                {
                    List<Field> candidates = Solver.GetEmptyFieldsInRow(i, sudoku);
                    NakedGroupSearch(candidates, sudoku);
                }

                // Look for each column
                for (int i = 0; i < 9; i++)
                {
                    List<Field> candidates = Solver.GetEmptyFieldsInColumn(i, sudoku);
                    NakedGroupSearch(candidates, sudoku);
                }

                // Look for each square
                List<int> traversed = new List<int>();
                for (int i = 0; i < 81; i += 3)
                {
                    int next = (i / 27 * 27) + (i % 9) - (i % 3);
                    if (!traversed.Contains(next))
                    {
                        traversed.Add(next);
                        List<Field> candidates = Solver.GetEmptyFieldsInSquare(i, sudoku);
                        NakedGroupSearch(candidates, sudoku);
                    }
                }
            } while (!Solver.IsSolved());
        }

        private static void NakedGroupSearch(List<Field> candidates, Sudoku sudoku)
        {
            for (int k = 0; k < candidates.Count; k++)
            {
                Field field = candidates[k];
                List<Field> pair = new List<Field>();
                List<Field> triple = new List<Field>();
                List<Field> quad = new List<Field>();
                List<int> ownPossibilities = field.GetPossibilities();

                locker.WaitingAcquire(sudoku.IndexOf(field));
                for (int j = k + 1; j < candidates.Count; j++)
                {
                    if (field.HasIdenticalPossibilities(candidates[j]))
                    {
                        // Look for pairs
                        if (ownPossibilities.Count == 2)
                        {
                            pair.Add(candidates[j]);
                        }
                        // Look for triples
                        else if (ownPossibilities.Count == 3)
                        {
                            triple.Add(candidates[j]);
                        }
                        // Look for quads
                        else if (ownPossibilities.Count == 4)
                        {
                            quad.Add(candidates[j]);
                        }
                    }
                }

                // The pair is full with one, because the first one is "field" which is not in the list
                if (pair.Count == 1)
                {
                    pair.Add(field);
                    candidates.Remove(pair[0]);
                    candidates.Remove(pair[1]);
                    NakedGroupRemove(candidates, field.GetPossibilities(), sudoku);
                }

                // The triple is full with two, because the first one is "field" which is not in the list
                if (triple.Count == 2)
                {
                    triple.Add(field);
                    candidates.Remove(triple[0]);
                    candidates.Remove(triple[1]);
                    candidates.Remove(triple[2]);
                    NakedGroupRemove(candidates, field.GetPossibilities(), sudoku);
                }

                // The quad is full with three, because the first one is "field" which is not in the list
                if (quad.Count == 3)
                {
                    quad.Add(field);
                    candidates.Remove(quad[0]);
                    candidates.Remove(quad[1]);
                    candidates.Remove(quad[2]);
                    candidates.Remove(quad[3]);
                    NakedGroupRemove(candidates, field.GetPossibilities(), sudoku);
                }
                locker.Release(sudoku.IndexOf(field));
            }
        }

        private static void NakedGroupRemove(List<Field> candidates, List<int> possibilities, Sudoku sudoku)
        {
            foreach (Field otherField in candidates)
            {
                int index = sudoku.IndexOf(otherField);

                locker.WaitingAcquire(index);
                for (int i = 0; i < possibilities.Count; i++)
                {
                    sudoku.GetField(index).RemovePossibility(possibilities[i]);
                }
                locker.Release(index);
            }
        }
        #endregion
    }
}
