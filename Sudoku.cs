using System.Collections.Generic;

namespace Sudoku_Solver
{
    class Sudoku
    {
        public const int SIZE = 81;

        private Field[] game = new Field[SIZE];

        public Sudoku(int[] init)
        {
            for (int i = 0; i < SIZE; i++)
            {
                game[i] = new Field();

                if (init[i] == 0)
                {
                    for (int j = 1; j < 10; j++)
                    {
                        game[i].AddPossibility(j);
                    }
                }
                else
                {
                    game[i].Value = init[i];
                }
            }
        }

        public int Get(int index)
        {
            return game[index].Value;
        }

        public Field GetField(int index)
        {
            return game[index];
        }

        public void Set(int index, int value)
        {
            game[index].Value = value;
        }

        public List<int> GetPossibilities(int index)
        {
            return game[index].GetPossibilities();
        }

        public void RemovePossibility(int index, int value)
        {
            game[index].RemovePossibility(value);
        }

        public void RemoveAllPossibilities(int index)
        {
            game[index].RemoveAllPossibilities();
        }
    }
}
