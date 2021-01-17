using System.Collections.Generic;

namespace Sudoku_Solver.SolvingAlgorithms
{
    class NakedGroup
    {
        /// <summary>
        /// Two cells in a row/column/square having onlythe same 
        /// group (pair/triple/quad) of numbers are called a naked pair/triple/quad.
        /// All other appearances of the numbers in
        /// the same row/column/square can be eliminated.
        /// </summary>

        public static void Solve(object param)
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

                Solver.locker.WaitingAcquire(sudoku.IndexOf(field));
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
                Solver.locker.Release(sudoku.IndexOf(field));
            }
        }

        private static void NakedGroupRemove(List<Field> candidates, List<int> possibilities, Sudoku sudoku)
        {
            foreach (Field otherField in candidates)
            {
                int index = sudoku.IndexOf(otherField);

                Solver.locker.WaitingAcquire(index);
                for (int i = 0; i < possibilities.Count; i++)
                {
                    sudoku.GetField(index).RemovePossibility(possibilities[i]);
                }
                Solver.locker.Release(index);
            }
        }
    }
}
