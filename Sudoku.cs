using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku_Solver
{
    class Sudoku
    {
        private const int SIZE = 81;

        private int?[,] game = new int?[SIZE,11];

        public Sudoku(int?[,] init)
        {
            game = init;

            for(int i = 0; i < SIZE; i++)
            {
                if(game[i,0] == null)
                {
                    for(int j = 1; j < 11; j++)
                    {
                        game[i, j] = j-1;
                    }
                }
            }
        }

        public int? Get (int index)
        {
            return game[index, 0];
        }

        public void Set (int index, int? value)
        {
            game[index, 0] = value;
        }

        public void RemovePossibility (int index, int value)
        {
            game[index, value] = null;
        }
    }
}
