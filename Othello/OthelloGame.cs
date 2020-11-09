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
        public char[,] BoardState { get; private set; }

        // bool tracks who's turn it is
        public bool IsWhitesTurn { get; private set; }

        public OthelloGame()
        {
            BoardState = new char[8,8];
            
            // initialize all places to 'O', representing no piece
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    BoardState[i, j] = 'O';

            // set white's starting pieces
            BoardState[4, 4] = 'W';
            BoardState[5, 5] = 'W';

            // set black's starting pieces
            BoardState[5, 4] = 'B';
            BoardState[4, 5] = 'B';
        }

        // returns true if valid move was played
        public bool PlayMove(string move)
        {
            // convert move to character array for parsing
            char[] moveArray = move.ToCharArray();

            // checks for invalid move input
            if (moveArray.Length != 2 || !char.IsLetter(moveArray[0]) || !char.IsDigit(moveArray[1])) // invalid length or format
            {
                Console.WriteLine("Format of supplied move was invalid. Move should be given as a character column followed by a numeric row, i.e. \"A1\", \"B4\", \"C6\".");
                return false;
            }
            else if (char.ToUpper(moveArray[0]) < 65 || char.ToUpper(moveArray[0]) > 72) // invalid column
            {
                Console.WriteLine("Column of supplied move was out of range, valid columns are letters A-H.");
                return false;
            }
            else if (moveArray[1] < 49 || moveArray[1] > 56) // invalid row
            {
                Console.WriteLine("Row of supplied move was out of range, valid rows are numbers 1-8");
                return false;
            }

            if (IsLegalMove(move))
            {
                // TODO: Change board state based on move

                // toggle current player turn
                IsWhitesTurn = !IsWhitesTurn;
                return true;
            }
            else
            {
                Console.WriteLine("Supplied move was not a legal move.");
                return false;
            }
        }

        public bool IsLegalMove(string move)
        {
            // TODO: Implement IsValidMove()
            throw new NotImplementedException();
        }

        public string[] GetLegalMoves()
        {
            // TODO: Implement GetValidMoves()
            throw new NotImplementedException();
        }

        public void PrintBoard()
        {
            Console.Clear();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 7; j++)
                    Console.Write(BoardState[i, j] + " ");
                Console.WriteLine(BoardState[i, 8] + "\n");
            }
        }
    }
}
