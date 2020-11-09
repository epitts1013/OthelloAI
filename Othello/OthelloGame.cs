using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class OthelloGame
    {
        // store board state
        private char[,] boardState;

        public OthelloGame()
        {
            boardState = new char[8,8];
            
            // initialize boardState
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    boardState[i, j] = 'O';
        }
    }
}
