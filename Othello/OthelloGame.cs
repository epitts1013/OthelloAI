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
        public bool IsBlacksTurn { get; private set; }

        // default constructor
        public OthelloGame()
        {
            // initialize variables
            BoardState = new char[8,8];
            IsBlacksTurn = true;
            
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

            List<int[]> positionsToUpdate;
            if ((positionsToUpdate = CheckMove(moveArray)) != null)
            {
                // TODO: Change board state based on move

                // toggle current player turn
                IsBlacksTurn = !IsBlacksTurn;
                return true;
            }
            else
            {
                Console.WriteLine("Supplied move was not a legal move.");
                return false;
            }
        }

        // returns a list positions to flip from a given move, or null if the move is invalid
        public List<int[]> CheckMove(char[] move)
        {
            // list of size 2 int arrays containing indexes of positions to flip color for
            List<int[]> positionsToUpdate = new List<int[]>();

            // get integer representation of move
            int column = char.ToUpper(move[0]) - 65;
            int row = move[1] - 49;

            // color of current turn
            char turnColor = IsBlacksTurn ? 'B' : 'W';

            // counts how many tiles will be flipped
            int count = 0;

            // check upwards
            for (int i = row; i >= 0; i--)
            {
                if (BoardState[column, i] == 'O') // if the search encounters a blank spot
                    break;
                else if (BoardState[column, i] != turnColor) // if the tile matches the opponent players tile color
                    count++;
                else if (BoardState[column, i] == turnColor) // if the tile matches the current players tile color
                {
                    // add positions if there are positions to add
                    if (count != 0)
                    {
                        // go back and add positions between the two tiles
                        for (int j = i + 1; j < row; j++)
                            positionsToUpdate.Add(new int[] { column, j });
                        break;
                    }
                    else break;
                }
            }
            count = 0;

            // check downwards
            for (int i = row; i < 8; i++)
            {
                if (BoardState[column, i] == 'O') // if the search encounters a blank spot
                    break;
                else if (BoardState[column, i] != turnColor) // if the tile matches the opponent players tile color
                    count++;
                else if (BoardState[column, i] == turnColor) // if the tile matches the current players tile color
                {
                    // add positions if there are positions to add
                    if (count != 0)
                    {
                        // go back and add positions between the two tiles
                        for (int j = i - 1; j >= row; j--)
                            positionsToUpdate.Add(new int[] { column, j });
                        break;
                    }
                    else break;
                }
            }
            count = 0;

            // if list of positions to update isn't empty, return it, otherwise return null
            if (positionsToUpdate.Count > 0)
                return positionsToUpdate;
            else
                return null;
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
