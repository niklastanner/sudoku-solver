using System;

namespace Sudoku_Solver
{
    [Flags]
    enum SolvingAlgorithms
    {
        All = 15,
        FillUniqueFileds = 1,
        SimpleElimination = 2,
        HiddenElimination = 4,
        NakedGroup = 8
    }
}
