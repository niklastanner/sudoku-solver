using System;

namespace Sudoku_Solver
{
    [Flags]
    enum SolvingAlgorithms
    {
        All = 7,
        FillUniqueFileds = 1,
        SimpleElimination = 2,
        HiddenElimination = 4,
        NakedGroup = 8,
        Omission = 16
    }
}
