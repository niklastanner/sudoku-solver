namespace Sudoku_Solver
{
    class FillUniqueFields : SolvingAlgorithm
    {
        /// <summary>
        /// Fields with only one possibility will be set
        /// </summary>
        public void Solve(object param)
        {
            Sudoku sudoku = (Sudoku)param;

            int index = 0;
            do
            {
                if (Solver.locker.Acquire(index))
                {
                    Field field = sudoku.GetField(index);
                    if (field.Value == 0 && field.GetPossibilities().Count == 1)
                    {
                        field.Value = field.GetPossibilities()[0];
                        field.RemoveAllPossibilities();
                    }
                    Solver.locker.Release(index);
                }

                index = (index + 1) % Sudoku.SIZE;
            } while (!Solver.IsSolved());
        }
    }
}
