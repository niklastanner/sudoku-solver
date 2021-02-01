using System.Collections.Generic;

namespace Sudoku_Solver
{
    class ChooseAlgorithms
    {
        public static List<SolvingAlgorithm> GetAlgorithms(SolvingAlgorithms requested)
        {
            List<SolvingAlgorithm> algorithms = new List<SolvingAlgorithm>();

            if ((requested & SolvingAlgorithms.FillUniqueFileds) != 0)
            {
                algorithms.Add(new FillUniqueFields());
            }
            if ((requested & SolvingAlgorithms.SimpleElimination) != 0)
            {
                algorithms.Add(new SimpleElimination());
            }
            if ((requested & SolvingAlgorithms.HiddenElimination) != 0)
            {
                algorithms.Add(new HiddenElimination());
            }
            if ((requested & SolvingAlgorithms.NakedGroup) != 0)
            {
                algorithms.Add(new NakedGroup());
            }
            if ((requested & SolvingAlgorithms.Omission) != 0)
            {
                algorithms.Add(new Omission());
            }

            return algorithms;
        }
    }
}
