using System;

namespace Sudoku_Solver
{
    static class Factory
    {
        public static Sudoku CreateSudoku()
        {
            Console.WriteLine("Reading sudoku from user input");
            int[] init = new int[81];
            for (int i = 0; i <= 72; i += 9)
            {
                Console.Write("Line {0}: ", i / 9 + 1);
                string input = Console.ReadLine();
                char[] line = input.ToCharArray();
                try
                {
                    if (input.Length == 9)
                    {
                        for (int j = 0; j < line.Length; j++)
                        {
                            init[i + j] = int.Parse(line[j].ToString());
                        }
                    }
                    else
                    {
                        throw new IndexOutOfRangeException("Input does not contain 9 characters");
                    }
                }
                catch (Exception e)
                {
                    if (e is IndexOutOfRangeException || e is FormatException)
                    {
                        Console.WriteLine(e.Message);
                        i -= 9;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            Console.WriteLine("");

            return new Sudoku(init);
        }
    }
}
