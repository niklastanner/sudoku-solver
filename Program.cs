﻿namespace Sudoku_Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] init = new int[81];

            // Insert known numbers here init[Place of number, from 0 - 80] = number in sudoku
            // Example for an easy sudoku
            /*            init[4] = 8; init[10] = 1; init[11] = 7; init[15] = 3; init[16] = 2; init[20] = 3; init[21] = 7; init[22] = 2; init[23] = 6; init[24] = 1;
                        init[27] = 2; init[28] = 5; init[31] = 6; init[34] = 1; init[35] = 8; init[45] = 6; init[46] = 3; init[49] = 5; init[52] = 4; init[53] = 7;
                        init[56] = 8; init[57] = 5; init[58] = 4; init[59] = 1; init[60] = 6; init[64] = 7; init[65] = 2; init[69] = 4; init[70] = 9; init[76] = 7;*/

            // Example for a medium hard sudoku
            /*init[1] = 3; init[3] = 4; init[5] = 2; init[7] = 9; init[11] = 9; init[13] = 5; init[15] = 8; init[18] = 2; init[26] = 1;
            init[29] = 8; init[30] = 5; init[32] = 9; init[33] = 6; init[40] = 7; init[47] = 2; init[48] = 3; init[50] = 6; init[51] = 1;
            init[54] = 6; init[62] = 9; init[65] = 1; init[67] = 6; init[69] = 2; init[73] = 2; init[75] = 7; init[77] = 4; init[79] = 6;*/

            // Example for a medium hard sudoku
            /*            init[0] = 7; init[1] = 4; init[7] = 2; init[8] = 9; init[12] = 2; init[14] = 5; init[19] = 5; init[21] = 6; init[23] = 7;
                        init[25] = 8; init[27] = 5; init[35] = 1; init[37] = 6; init[43] = 9; init[45] = 9; init[53] = 5; init[55] = 7; init[57] = 9;
                        init[59] = 8; init[61] = 5; init[66] = 3; init[68] = 1; init[72] = 8; init[73] = 9; init[79] = 1; init[80] = 3;*/

            // Example for a hard sudoku
            /*            init[2] = 2; init[3] = 6; init[5] = 9; init[11] = 7; init[12] = 2; init[17] = 8; init[22] = 1; init[25] = 4;
                        init[27] = 4; init[34] = 1; init[36] = 1; init[39] = 3; init[40] = 6; init[41] = 5; init[44] = 4; init[46] = 2; init[53] = 3;
                        init[55] = 5; init[58] = 3; init[63] = 6; init[68] = 7; init[69] = 9; init[75] = 8; init[77] = 6; init[78] = 5;*/

            init[3] = 7; init[4] = 1; init[7] = 8; init[8] = 3; init[9] = 2; init[15] = 4; init[21] = 4; init[24] = 9; init[25] = 7;
            init[28] = 8; init[32] = 5; init[35] = 2; init[40] = 2; init[45] = 5; init[48] = 8; init[52] = 6;
            init[55] = 3; init[56] = 2; init[59] = 7; init[65] = 8; init[71] = 9; init[72] = 6; init[73] = 7; init[76] = 3; init[77] = 4;

            Sudoku sudoku = Factory.Create();

            Solver solver = new Solver(sudoku);
            solver.Solve();
        }
    }
}
