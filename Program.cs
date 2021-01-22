namespace Sudoku_Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            Sudoku sudoku = Factory.CreateSudoku();

            Solver solver = new Solver(sudoku);
            solver.Solve();
        }
    }
}
