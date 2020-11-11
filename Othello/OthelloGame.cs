using System;
using System.Collections.Generic;
using System.Data;
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

        // resets board to starting state
        public void ResetBoard()
        {
            IsBlacksTurn = true;

            // initialize all places to 'O', representing no piece
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    BoardState[i, j] = 'O';

            // set white's starting pieces
            BoardState[3, 3] = 'W';
            BoardState[4, 4] = 'W';

            // set black's starting pieces
            BoardState[4, 3] = 'B';
            BoardState[3, 4] = 'B';
        }

        // returns true if valid move was played
        public bool PlayMove(string move)
        {
            // convert move to character array for parsing
            char[] moveArray = move.ToCharArray();

            // get integer representation of move
            int column = char.ToUpper(moveArray[0]) - 65;
            int row = moveArray[1] - 49;

            // checks for invalid move input
            if (moveArray.Length != 2 || !char.IsLetter(moveArray[0]) || !char.IsDigit(moveArray[1])) // invalid length or format
            {
                Console.WriteLine("Format of supplied move was invalid. Move should be given as a character column followed by a numeric row, i.e. \"A1\", \"B4\", \"C6\". Press Enter to continue...");
                Console.ReadLine();
                return false;
            }
            else if (column < 1 || column > 7) // invalid column
            {
                Console.WriteLine("Column of supplied move was out of range, valid columns are letters A-H. Press Enter to continue...");
                Console.ReadLine();
                return false;
            }
            else if (row < 1 || row > 7) // invalid row
            {
                Console.WriteLine("Row of supplied move was out of range, valid rows are numbers 1-8. Press Enter to continue...");
                Console.ReadLine();
                return false;
            }
            else if (BoardState[row, column] != 'O') // attempted to play in non-empty space
            {
                Console.WriteLine("Cannot play in already occupied space.");
                Console.ReadLine();
                return false;
            }

            List<int[]> positionsToUpdate;
            if ((positionsToUpdate = CheckMove(new int[] { column, row })) != null)
            {
                // update board positions
                positionsToUpdate.ForEach(position =>
                {
                    BoardState[position[0], position[1]] = IsBlacksTurn ? 'B' : 'W';
                });

                // toggle current player turn
                IsBlacksTurn = !IsBlacksTurn;
                return true;
            }
            else
            {
                Console.WriteLine("Supplied move was not a legal move. Press Enter to continue...");
                Console.ReadLine();
                return false;
            }
        }

        // returns a list positions to flip from a given move, or null if the move is illegal
        private List<int[]> CheckMove(int[] move)
        {
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

            // color of current turn
            char turnColor = IsBlacksTurn ? 'B' : 'W';

            // check each direction from move for a flank
            int[] flankingPiece;
            directions.ForEach(direction =>
            {
                // check direction, if flank found, then add pieces to flip to positionsToUpdate
                if ((flankingPiece = CheckForFlank(move, direction, turnColor)) != null)
                {
                    int column = move[0], row = move[1];
                    for (; !(row == flankingPiece[0] && column == flankingPiece[1]) ; column += direction[0], row += direction[1])
                        positionsToUpdate.Add(new int[] { row, column });
                }
            });

            #region Expanded If Chain
            //if ((flankingPiece = CheckForFlank(move, new int[] { 1, 0 }, turnColor)) != null) // check down
            //{
            //    int column = move[0], row = move[1];
            //    for (; column != flankingPiece[0] && row != flankingPiece[1]; column += 1, row += 0)
            //        positionsToUpdate.Add(new int[] { column, row });
            //}

            //if ((flankingPiece = CheckForFlank(move, new int[] { -1, 0 }, turnColor)) != null) // check up
            //{
            //    int column = move[0], row = move[1];
            //    for (; column != flankingPiece[0] && row != flankingPiece[1]; column += -1, row += 0)
            //        positionsToUpdate.Add(new int[] { column, row });
            //}

            //if ((flankingPiece = CheckForFlank(move, new int[] { 0, 1 }, turnColor)) != null) // check right
            //{
            //    int column = move[0], row = move[1];
            //    for (; column != flankingPiece[0] && row != flankingPiece[1]; column += 0, row += 1)
            //        positionsToUpdate.Add(new int[] { column, row });
            //}

            //if ((flankingPiece = CheckForFlank(move, new int[] { 0, -1 }, turnColor)) != null) // check left
            //{
            //    int column = move[0], row = move[1];
            //    for (; column != flankingPiece[0] && row != flankingPiece[1]; column += 0, row += -1)
            //        positionsToUpdate.Add(new int[] { column, row });
            //}

            //if ((flankingPiece = CheckForFlank(move, new int[] { 1, 1 }, turnColor)) != null) // check down-right
            //{
            //    int column = move[0], row = move[1];
            //    for (; column != flankingPiece[0] && row != flankingPiece[1]; column += 1, row += 1)
            //        positionsToUpdate.Add(new int[] { column, row });
            //}

            //if ((flankingPiece = CheckForFlank(move, new int[] { 1, -1 }, turnColor)) != null) // check down-left
            //{
            //    int column = move[0], row = move[1];
            //    for (; column != flankingPiece[0] && row != flankingPiece[1]; column += 1, row += -1)
            //        positionsToUpdate.Add(new int[] { column, row });
            //}                                                           

            //if ((flankingPiece = CheckForFlank(move, new int[] { -1, 1 }, turnColor)) != null) // check up-right
            //{
            //    int column = move[0], row = move[1];
            //    for (; column != flankingPiece[0] && row != flankingPiece[1]; column += -1, row += 1)
            //        positionsToUpdate.Add(new int[] { column, row });
            //}

            //if ((flankingPiece = CheckForFlank(move, new int[] { -1, -1 }, turnColor)) != null) // check up-left
            //{
            //    int column = move[0], row = move[1];
            //    for (; column != flankingPiece[0] && row != flankingPiece[1]; column += -1, row += -1)
            //        positionsToUpdate.Add(new int[] { column, row });
            //}
            #endregion

            if (positionsToUpdate.Count != 0)
                return positionsToUpdate;
            else
                return null;

            #region More Old Code
            //// list of size 2 int arrays containing indexes of positions to flip color for
            //List<int[]> positionsToUpdate = new List<int[]>();

            //// get integer representation of move
            //int column = char.ToUpper(move[0]) - 65;
            //int row = move[1] - 49;

            //// if move is on already occupied space
            //if (BoardState[column, row] != 'O')
            //{
            //    Console.WriteLine("Space is already occupied");
            //    return null;
            //}

            //if (IsBlacksTurn)
            //{
            //    List<int[]> piecesToConsider = new List<int[]>();
            //    blackPieces.ForEach(piece =>
            //    {
            //        // check if move is in column with another piece of the same color
            //        if (piece[0] == column)
            //        {
            //            if (piece[0] > column)
            //        }

            //        // check if move is in row with another piece of the same color
            //        if (piece[1] == row)
            //        {

            //        }

            //        // check if move is on a diagonal from another piece of the same color
            //        if (Math.Abs(column - piece[0]) == Math.Abs(row - piece[1]))
            //        {

            //        }
            //    });
            //}
            //else
            //{
            //    List<int[]> piecesToConsider = new List<int[]>();
            //    whitePieces.ForEach(piece =>
            //    {
            //        // if move is in line with another piece of the same color
            //        if (piece[0] == column || piece[1] == row)
            //        {

            //        }

            //        // check if move is on a diagonal from another piece of the same color
            //        if (Math.Abs(column - piece[0]) == Math.Abs(row - piece[1]))
            //        {

            //        }
            //    });
            //}
            #endregion
        }

        private int[] CheckForFlank(int[] move, int[] direction, char turnColor)
        {
            // get column and row from move
            int column = move[0], row = move[1];

            // get opponent color
            char oppColor = turnColor == 'B' ? 'W' : 'B';

            do
            {
                // increment row and column by search direction
                column += direction[0];
                row += direction[1];

                // if we go out of array bounds, return null
                if (column < 0 || column > 7 || row < 0 || row > 7)
                    return null;

            } while (BoardState[row, column] == oppColor); // if we stop seeing the opponents piece, check why

            if (BoardState[row, column] == turnColor) // if space is the current turns color, return the piece position
                return new int[] { row, column };
            else
                return null;
        }

        public bool HasLegalMoves(char player)
        {
            // TODO: Implement HasLegalMoves()
            return true;
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
