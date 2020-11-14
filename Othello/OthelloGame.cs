using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
            
            // initialize all places to '.', representing no piece
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    BoardState[i, j] = '.';

            // set white's starting pieces
            BoardState[3, 3] = 'O';
            BoardState[4, 4] = 'O';

            // set black's starting pieces
            BoardState[4, 3] = '@';
            BoardState[3, 4] = '@';
        }

        // resets board to starting state
        public void ResetBoard()
        {
            IsBlacksTurn = true;

            // initialize all places to '.', representing no piece
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    BoardState[i, j] = '.';

            // set white's starting pieces
            BoardState[3, 3] = 'O';
            BoardState[4, 4] = 'O';

            // set black's starting pieces
            BoardState[4, 3] = '@';
            BoardState[3, 4] = '@';
        }

        // returns true if valid move was played
        public bool PlayMove(string move)
        {
            // validate move format
            if (!Regex.IsMatch(move, @"\A[A-Ha-h][1-8]\Z"))
            {
                Console.WriteLine("Move format invalid. Move should be column A-H followed by row 1-8, i.e. \"A1\", \"B4\", \"C6\".\nPress Enter to continue...");
                Console.ReadLine();
                return false;
            }

            // get integer representation of move
            int column = char.ToUpper(move.ToCharArray()[0]) - 65;
            int row = move.ToCharArray()[1] - 49;

            // make sure move is in an empty space
            if (BoardState[row, column] != '.')
            {
                Console.WriteLine("Cannot play in already occupied space. Press enter to continue...");
                Console.ReadLine();
                return false;
            }

            #region Old Validation Code
            //// checks for invalid move format
            //if (moveArray.Length != 2 || !char.IsLetter(moveArray[0]) || !char.IsDigit(moveArray[1])) // invalid length or format
            //{
            //    Console.WriteLine("Format of supplied move was invalid. Move should be given as a character column followed by a numeric row, i.e. \"A1\", \"B4\", \"C6\". Press Enter to continue...");
            //    Console.ReadLine();
            //    return false;
            //}
            //// check for invalid move parameters
            //if (column < 0 || column > 7) // invalid column
            //{
            //    Console.WriteLine("Column of supplied move was out of range, valid columns are letters A-H. Press Enter to continue...");
            //    Console.ReadLine();
            //    return false;
            //}
            //else if (row < 0 || row > 7) // invalid row
            //{
            //    Console.WriteLine("Row of supplied move was out of range, valid rows are numbers 1-8. Press Enter to continue...");
            //    Console.ReadLine();
            //    return false;
            //}
            #endregion

            // check if the supplied move is legal, if it is, play it
            List<int[]> positionsToUpdate;
            char turnColor = IsBlacksTurn ? '@' : 'O';
            if ((positionsToUpdate = CheckMove(new int[] { column, row }, turnColor, BoardState)) != null)
            {
                // update board positions
                positionsToUpdate.ForEach(position =>
                {
                    BoardState[position[0], position[1]] = turnColor;
                });

                // toggle current player turn
                IsBlacksTurn = !IsBlacksTurn;
                return true;
            }
            else // if the move is not legal, inform the player
            {
                Console.WriteLine("Supplied move was not a legal move. Press Enter to continue...");
                Console.ReadLine();
                return false;
            }
        }

        public bool PlayMove(int[] move)
        {
            // check if the supplied move is legal, if it is, play it
            List<int[]> positionsToUpdate;
            char turnColor = IsBlacksTurn ? '@' : 'O';
            if ((positionsToUpdate = CheckMove(new int[] { move[0], move[1] }, turnColor, BoardState)) != null)
            {
                // update board positions
                positionsToUpdate.ForEach(position =>
                {
                    BoardState[position[0], position[1]] = turnColor;
                });

                // toggle current player turn
                IsBlacksTurn = !IsBlacksTurn;
                return true;
            }
            else // if the move is not legal, inform the player
            {
                Console.WriteLine("Supplied move was not a legal move. Press Enter to continue...");
                Console.ReadLine();
                return false;
            }
        }

        // returns a list positions to flip from a given move, or null if the move is illegal
        public List<int[]> CheckMove(int[] move, char turnColor, char[,] boardState)
        {
            // check if space is already occupied
            if (boardState[move[1], move[0]] != '.')
                return null;

            // list of size 2 int arrays containing indexes of positions to flip color for
            List<int[]> positionsToUpdate = new List<int[]>();

            // list of directions to search in
            List<int[]> directions = new List<int[]> { 
                new int[] { 1, 0 },
                new int[] { -1, 0 },
                new int[] { 0, 1 },
                new int[] { 0, -1},
                new int[] { 1, 1 },
                new int[] { 1, -1 },
                new int[] { -1, 1},
                new int[] { -1, -1 }
            };

            // check each direction from move for a flank
            int[] flankingPiece;
            directions.ForEach(direction =>
            {
                // check direction, if flank found, then add pieces to flip to positionsToUpdate
                if ((flankingPiece = CheckForFlank(move, direction, turnColor, boardState)) != null)
                {
                    int column = move[0], row = move[1];
                    for (; !(row == flankingPiece[0] && column == flankingPiece[1]) ; column += direction[0], row += direction[1])
                        positionsToUpdate.Add(new int[] { row, column });
                }
            });

            // if any positions should be updated, return them, else return null
            if (positionsToUpdate.Count != 0)
                return positionsToUpdate;
            else
                return null;
        }

        private int[] CheckForFlank(int[] move, int[] direction, char turnColor, char[,] boardState)
        {
            // get column and row from move
            int column = move[0], row = move[1];

            // get opponent color
            char oppColor = turnColor == '@' ? 'O' : '@';

            // tracks the number of flanked pieces
            // start at -1 to accomodate that it must be incremented before the first do-while condition check
            int count = -1;

            do
            {
                // increment row and column by search direction
                column += direction[0];
                row += direction[1];

                // if we go out of array bounds, return null
                if (column < 0 || column > 7 || row < 0 || row > 7)
                    return null;

                count++;
            } while (boardState[row, column] == oppColor); // if we stop seeing the opponents piece, check why

            // if space is the current turns color and we have flanked pieces, return the piece position
            // else return null
            if (boardState[row, column] == turnColor && count > 0)
                return new int[] { row, column };
            else
                return null;
        }

        // evaluate if a player has moves on this objects board
        public bool HasLegalMoves(char player)
        {
            return HasLegalMoves(player, BoardState);
        }

        // evaluate if a player has legal moves on an arbitrary board
        public bool HasLegalMoves(char player, char[,] boardState)
        {
            // search each square of board, if a legal move is found return true
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (CheckMove(new int[] { i, j }, player, boardState) != null)
                        return true;

            // if whole board is scanned without finding a legal move, return false
            return false;
        }

        public void PrintBoard()
        {
            Console.Clear();

            Console.WriteLine("  A B C D E F G H");
            for (int i = 0; i < 8; i++)
            {
                Console.Write((i + 1) + " ");
                for (int j = 0; j < 7; j++)
                    Console.Write(BoardState[i, j] + " ");
                Console.WriteLine(BoardState[i, 7]);
            }
        }
    }
}
