# Welcome to Sudoku Solver

This C# Project solves sudokus using common techniques instead of bruteforce (like backtracking).

## How does it work?
When started the user is requested to type in the sudoku into the console line by line.
Empty fields in the sudoku are represented by a 0.

Alternatively the factory method in Program.cs can be removed and a hardcoded sudoku can be used. (There are some examples)

> init[index] = value;

The index defines the position of the field. The fields are numbered from 0 - 80. Starting in the top left corner, left to right.
If you want to use a coded sudoku initialize init with the size of 81:

> int[] init = new int[81];

You also need to modify the line which uses the factory. It must be changed to the following:

> Sudoku sudoku = new Sudoku(init);

### Example for an easy sudoku
```
init[4] = 8; init[10] = 1; init[11] = 7; init[15] = 3; init[16] = 2;
init[20] = 3; init[21] = 7; init[22] = 2; init[23] = 6; init[24] = 1;
init[27] = 2; init[28] = 5; init[31] = 6; init[34] = 1; init[35] = 8;
init[45] = 6; init[46] = 3; init[49] = 5; init[52] = 4; init[53] = 7;
init[56] = 8; init[57] = 5; init[58] = 4; init[59] = 1; init[60] = 6;
init[64] = 7; init[65] = 2; init[69] = 4; init[70] = 9; init[76] = 7;
```

### Example for a medium sudoku
```
init[1] = 3; init[3] = 4; init[5] = 2; init[7] = 9; init[11] = 9; init[13] = 5;
init[15] = 8; init[18] = 2; init[26] = 1; init[29] = 8; init[30] = 5; init[32] = 9;
init[33] = 6; init[40] = 7; init[47] = 2; init[48] = 3; init[50] = 6; init[51] = 1;
init[54] = 6; init[62] = 9; init[65] = 1; init[67] = 6; init[69] = 2; init[73] = 2;
init[75] = 7; init[77] = 4; init[79] = 6;
```

### Example for a hard sudoku (yet not solvable by this program)
```
init[2] = 2; init[3] = 6; init[5] = 9; init[11] = 7; init[12] = 2; init[17] = 8;
init[22] = 1; init[25] = 4; init[27] = 4; init[34] = 1; init[36] = 1; init[39] = 3;
init[40] = 6; init[41] = 5; init[44] = 4; init[46] = 2; init[53] = 3; init[55] = 5;
init[58] = 3; init[63] = 6; init[68] = 7; init[69] = 9; init[75] = 8; init[77] = 6
init[78] = 5;
```
