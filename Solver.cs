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
            int count = 0;
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

                if (possibilities.Count > 1)
                {
                    int value = CheckNeighbors(i);
                    if (value != 0)
                    {
                        sudoku.Set(i, value);
                        sudoku.RemoveAllPossibilities(i);
                    }
                }

                i = (i + 1) % Sudoku.SIZE;
                count++;
            } while (!CheckSolved() && count < 10_000_000);

            if (ValidateSudoku())
            {
                Console.WriteLine("Solved it in " + count + " iterations\n");
            }
            else
            {
                Console.WriteLine("I am way too dumb to solve this sudoku\n");
            }

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

        private int CheckNeighbors(int index) // Set recursion depth limit
        {
            int horizontal = index / 9 * 9;
            int vertical = index % 9;
            int square = (index / 27 * 27) + (index % 9) - (index % 3);
            List<Field> horizontalValues = new List<Field>();
            List<Field> verticalValues = new List<Field>();
            List<Field> squareValues = new List<Field>();
            List<int> ownPossibilities = sudoku.GetPossibilities(index);

            // Get all possibilities from the empty fields in a row or square
            for (int i = 0; i < 9; i++)
            {
                if (sudoku.Get(horizontal + i) == 0 && (horizontal + i) != index)
                {
                    horizontalValues.Add(sudoku.GetField(horizontal + i));
                }
            }
            for (int i = 0; i < (73 + vertical); i += 9)
            {
                if (sudoku.Get(vertical + i) == 0 && (vertical + i) != index)
                {
                    verticalValues.Add(sudoku.GetField(vertical + i));
                }
            }
            for (int j = 0; j <= 18; j += 9)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (sudoku.Get(square + i + j) == 0 && (square + i + j) != index)
                    {
                        squareValues.Add(sudoku.GetField(square + i + j));
                    }
                }
            }

            // Use the collected possibilities to set a value
            foreach (int value in ownPossibilities)
            {

                bool foundHorizontal = false;
                bool fountVertical = false;
                bool foundSquare = false;

                foreach (Field field in horizontalValues)
                {
                    if (field.GetPossibilities().Contains(value))
                    {
                        foundHorizontal = true;
                        break;
                    }
                }

                foreach (Field field in verticalValues)
                {
                    if (field.GetPossibilities().Contains(value))
                    {
                        fountVertical = true;
                        break;
                    }
                }

                foreach (Field field in squareValues)
                {
                    if (field.GetPossibilities().Contains(value))
                    {
                        foundSquare = true;
                        break;
                    }
                }

                if (!foundHorizontal || !fountVertical || !foundSquare)
                {
                    return value;
                }

            }

            return 0;
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

        private bool ValidateSudoku()
        {
            for (int i = 0; i < Sudoku.SIZE; i += 9)
            {
                List<int> row = new List<int>();

                for (int j = 0; j < 9; j++)
                {
                    if (row.Contains(sudoku.Get(i + j)))
                    {
                        return false;
                    }
                    else
                    {
                        row.Add(sudoku.Get(i + j));
                    }
                }
            }

            return true;
        }
    }
}
