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

        public bool IsIdenticalField(Field other)
        {
            if (this == other)
            {
                return true;
            }
            if (other.Value != Value)
            {
                return false;
            }
            if (!HasIdenticalPossibilities(other))
            {
                return false;
            }

            return true;
        }

        public bool HasIdenticalPossibilities(Field other)
        {
            List<int> otherPossibilities = other.GetPossibilities();

            if(possibilities.Count != otherPossibilities.Count)
            {
                return false;
            }

            foreach (int possibility in possibilities)
            {
                if (!otherPossibilities.Contains(possibility))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
