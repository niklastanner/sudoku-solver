using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku_Solver
{
    class Sudoku
    {
        private const int SIZE = 81;

        private Field[] game = new Field[SIZE];

        public Sudoku(int[] init)
        {
            for(int i = 0; i < SIZE; i++)
            {
                game[i] = new Field();

                if (init[i] == 0)
                {
                    for(int j = 1; j < 10; j++)
                    {
                        game[i].AddPossibility(j);
                    }
                } else
                {
                    game[i].Value = init[i];
                }
            }
        }

        public int Get (int index)
        {
            return game[index].Value;
        }

        public void Set (int index, int value)
        {
            game[index].Value = value;
        }

        public void RemovePossibility (int index, int value)
        {
            game[index].RemovePossibility(value);
        }
    }
}
