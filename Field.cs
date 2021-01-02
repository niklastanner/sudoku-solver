using System.Collections.Generic;

namespace Sudoku_Solver
{
    class Field
    {
        private List<int> possibilities = new List<int>();

        public int Value { get; set; }

        public List<int> GetPossibilities()
        {
            return possibilities;
        }

        public void AddPossibility(int value)
        {
            possibilities.Add(value);
        }

        public void RemovePossibility(int value)
        {
            possibilities.Remove(value);
        }

        public void RemoveAllPossibilities()
        {
            possibilities = new List<int>();
        }
    }
}
