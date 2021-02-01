using System.Collections.Generic;

namespace Sudoku_Solver
{
    class Omission : SolvingAlgorithm
    {
        /// <summary>
        /// Twice the same number in the same square, but the only appearance in the same row are called Omission.
        /// Every other appearance in the square can be eliminated.
        /// </summary>
        public void Solve(object param)
        {
            Sudoku sudoku = (Sudoku)param;
            do
            {
                // Look for each row
                for (int i = 0; i <= 72; i += 9)
                {
                    List<Field> candidates = Solver.GetEmptyFieldsInRow(i, sudoku);
                    if (candidates.Count > 1)
                    {
                        OmissionSearch(candidates, "row", sudoku);
                    }
                }

                // Look for each column
                for (int i = 0; i < 9; i++)
                {
                    List<Field> candidates = Solver.GetEmptyFieldsInColumn(i, sudoku);
                    if (candidates.Count > 1)
                    {
                        OmissionSearch(candidates, "column", sudoku);
                    }
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
                        if (candidates.Count > 1)
                        {
                            OmissionSearch(candidates, "square", sudoku);
                        }
                    }
                }
            } while (!Solver.IsSolved());
        }

        private void OmissionSearch(List<Field> candidates, string scope, Sudoku sudoku)
        {
            int[] numberList = new int[0];
            if (scope == "row")
            {
                numberList = Solver.GetMissingNumbersInRow(sudoku.IndexOf(candidates[0]), sudoku);
            }
            else if (scope == "column")
            {
                numberList = Solver.GetMissingNumbersInColumn(sudoku.IndexOf(candidates[0]), sudoku);
            }
            else if (scope == "square")
            {
                numberList = Solver.GetMissingNumbersInColumn(sudoku.IndexOf(candidates[0]), sudoku);
            }

            foreach (int i in numberList)
            {
                List<Field> foundAppearances = new List<Field>();
                bool skipNumber = false;
                for (int t = 0; t < candidates.Count; t++)
                {
                    Field field = candidates[t];
                    if (field.GetPossibilities().Contains(i))
                    {
                        for (int k = t + 1; k < candidates.Count; k++)
                        {
                            if (candidates[k].GetPossibilities().Contains(i))
                            {
                                if (scope == "row" || scope == "column")
                                {
                                    if (!Solver.AreSameSquare(field, candidates[k], sudoku))
                                    {
                                        skipNumber = true;
                                        break;
                                    }
                                }
                                else if (scope == "square")
                                {
                                    if (!Solver.AreSameRow(field, candidates[k], sudoku) && !Solver.AreSameColumn(field, candidates[k], sudoku))
                                    {
                                        skipNumber = true;
                                        break;
                                    }
                                }
                                if (!foundAppearances.Contains(field))
                                {
                                    foundAppearances.Add(field);
                                }
                                if (!foundAppearances.Contains(candidates[k]))
                                {
                                    foundAppearances.Add(candidates[k]);
                                }
                            }
                        }
                        if (skipNumber)
                        {
                            break;
                        }
                    }
                }
                if (!skipNumber && foundAppearances.Count > 1)
                {
                    OmissionDelete(i, scope, foundAppearances, sudoku);
                }
            }
        }

        private void OmissionDelete(int number, string scope, List<Field> ignore, Sudoku sudoku)
        {
            int index = sudoku.IndexOf(ignore[0]);
            List<Field> candidates = new List<Field>();

            if (scope == "row" || scope == "column")
            {
                candidates = Solver.GetEmptyFieldsInSquare(index, sudoku);
            }
            else if (scope == "square")
            {
                if (Solver.AreSameRow(ignore[0], ignore[1], sudoku))
                {
                    candidates = Solver.GetEmptyFieldsInRow(index, sudoku);
                }
                else
                {
                    candidates = Solver.GetEmptyFieldsInColumn(index, sudoku);
                }
            }

            foreach (Field field in ignore)
            {
                candidates.Remove(field);
            }
            foreach (Field candidate in candidates)
            {
                int toRemoveIndex = sudoku.IndexOf(candidate);
                Solver.locker.WaitingAcquire(toRemoveIndex);
                candidate.RemovePossibility(number);
                Solver.locker.Release(toRemoveIndex);
            }
        }
    }
}
