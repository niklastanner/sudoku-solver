using System;

namespace Sudoku_Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] init = new int[81];

            // Insert known numbers here init[Place of number, from 0 - 80] = number in sudoku
            /* Example for an easy sudoku */
            init[4] = 8; init[10] = 1; init[11] = 7; init[15] = 3; init[16] = 2; init[20] = 3; init[21] = 7; init[22] = 2; init[23] = 6; init[24] = 1;
            init[27] = 2; init[28] = 5; init[31] = 6; init[34] = 1; init[35] = 8; init[45] = 6; init[46] = 3; init[49] = 5; init[52] = 4; init[53] = 7;
            init[56] = 8; init[57] = 5; init[58] = 4; init[59] = 1; init[60] = 6; init[64] = 7; init[65] = 2; init[69] = 4; init[70] = 9; init[76] = 7;


            // Example for a hard sudoku
            /*          init[2] = 2; init[3] = 6; init[5] = 9; init[11] = 7; init[12] = 2; init[17] = 8; init[22] = 1; init[25] = 4;
                        init[27] = 4; init[34] = 1; init[36] = 1; init[39] = 3; init[40] = 6; init[41] = 5; init[44] = 4; init[46] = 2; init[53] = 3;
                        init[55] = 5; init[58] = 3; init[63] = 6; init[68] = 7; init[69] = 9; init[75] = 8; init[77] = 6; init[78] = 5;*/


            Sudoku sudoku = new Sudoku(init);
            Console.WriteLine("Trying to solve the following Sudoku:");
            Output(sudoku);

            Solver solver = new Solver(sudoku);
            solver.Solve();
            Console.WriteLine("Sudoku solver finished:");

            Output(sudoku);
        }

        static void Output(Sudoku sudoku)
        {
            string line = "";
            string separator = "";

            for (int i = 0; i < 38; i++)
            {
                separator += "-";
            }

            Console.WriteLine(separator);

            for (int i = 0; i < Sudoku.SIZE; i++)
            {
                if (i % 3 == 0)
                {
                    line += " | ";
                }

                line += " ";
                if (sudoku.Get(i) == 0)
                {
                    line += "_";
                }
                else
                {
                    line += sudoku.Get(i);
                }
                line += " ";

                if (i % 9 == 8)
                {
                    line += " | ";
                    Console.WriteLine(line);
                    line = "";
                }
                if (i % 27 == 26)
                {
                    Console.WriteLine(separator);
                }
            }
        }
    }
}
