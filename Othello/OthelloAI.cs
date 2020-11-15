using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class OthelloAI
    {
        // class constants
        private const int MAX_DEPTH = 4;    // max search depth for Minimax and AlphaBeta

        // store reference to the current OthelloGame object
        public OthelloGame AttachedGame { get; private set; }

        public bool IsBlackPlayer { get; private set; }

        // determines whether alpha-beta pruning is active
        public bool AlphaBetaActive { get; private set; }

        // tracks the number of moves considered when deciding on a move
        private int statesAnalyzed;

        // default constructor
        public OthelloAI(OthelloGame attachedGame, bool isBlackPlayer)
        {
            AttachedGame = attachedGame;
            IsBlackPlayer = isBlackPlayer;
            AlphaBetaActive = true;
        }

        // toggles alpha-beta pruning on or off
        public void ToggleAlphaBeta()
        {
            AlphaBetaActive = !AlphaBetaActive;
        }

        // runs tree search algorithm and makes move based on results
        public int[] ChooseMove()
        {
            // reset moves considered
            statesAnalyzed = 0;

            // get current board state from game
            char[,] boardCopy;

            // get color
            char color = IsBlackPlayer ? '@' : 'O';

            // stores the move determined to be best by given algorithm
            int[] bestEvaluatedMove = null;

            // TODO: AlphaBeta needs the updated alpha and beta information
            if (AlphaBetaActive)
            {
                int alpha = IsBlackPlayer ? int.MinValue : int.MaxValue;
                int beta = IsBlackPlayer ? int.MaxValue : int.MinValue;
                int maxEval = int.MinValue, eval;
                List<int[]> validMoves = GetValidMoves(color, AttachedGame.BoardState);
                validMoves.ForEach(position =>
                {
                    boardCopy = (char[,])AttachedGame.BoardState.Clone();
                    ApplyMove(position, boardCopy, color);
                    eval = AlphaBetaSearch(boardCopy, MAX_DEPTH, alpha, beta, IsBlackPlayer);

                    if (IsBlackPlayer)
                        alpha = Math.Max(alpha, eval);
                    else
                        beta = Math.Max(beta, eval);

                    if (eval > maxEval)
                    {
                        maxEval = eval;
                        bestEvaluatedMove = position;
                    }
                    statesAnalyzed++;
                });
            }
            else
            {
                int maxEval = int.MinValue, eval;
                List<int[]> validMoves = GetValidMoves(color, AttachedGame.BoardState);
                validMoves.ForEach(position =>
                {
                    boardCopy = (char[,])AttachedGame.BoardState.Clone();
                    ApplyMove(position, boardCopy, color);
                    eval = MinimaxSearch(boardCopy, MAX_DEPTH, IsBlackPlayer);
                    if (eval > maxEval)
                    {
                        maxEval = eval;
                        bestEvaluatedMove = position;
                    }
                    statesAnalyzed++;
                });
            }

            // print number of analyzed states
            Console.WriteLine($"AI analyzed {statesAnalyzed} board states before reaching conclusion.\nPress enter to continue...");
            Console.ReadLine();

            // return the determined best move
            return bestEvaluatedMove;
        }

        // performs minimax search
        private int MinimaxSearch(char[,] boardState, int depth, bool maximizingPlayer)
        {
            // evaluate board if in gameover position or at max depth
            if (depth == 0 || GameOver(boardState))
                return EvaluatePosition(boardState);

            // copy of board that can be safely modified
            char[,] stateCopy;

            // get player color
            char color = maximizingPlayer ? '@' : 'O';

            if (maximizingPlayer)
            {
                int maxEval = int.MinValue;
                List<int[]> validMoves = GetValidMoves(color, boardState);

                // if there's no moves available, pass to the next ply
                if (validMoves == null)
                {
                    int eval = MinimaxSearch(boardState, depth - 1, false);
                    maxEval = Math.Max(maxEval, eval);
                }
                else // else if moves are available, loop through them
                {
                    validMoves.ForEach(position =>
                    {
                        stateCopy = (char[,])boardState.Clone();
                        ApplyMove(position, stateCopy, color);
                        int eval = MinimaxSearch(stateCopy, depth - 1, false);
                        maxEval = Math.Max(maxEval, eval);
                        statesAnalyzed++;
                    });
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                List<int[]> validMoves = GetValidMoves(color, boardState);

                // if there's no moves available, pass to the next ply
                if (validMoves == null)
                {
                    int eval = MinimaxSearch(boardState, depth - 1, true);
                    minEval = Math.Min(minEval, eval);
                }
                else // else if moves are available, loop through them
                {
                    validMoves.ForEach(position =>
                    {
                        stateCopy = (char[,])boardState.Clone();
                        ApplyMove(position, stateCopy, color);
                        int eval = MinimaxSearch(stateCopy, depth - 1, true);
                        minEval = Math.Min(minEval, eval);
                        statesAnalyzed++;
                    });
                }
                return minEval;
            }
        }

        private int AlphaBetaSearch(char[,] boardState, int depth, int alpha, int beta, bool maximizingPlayer)
        {
            // evaluate board if in gameover position or at max depth
            if (depth == 0 || GameOver(boardState))
                return EvaluatePosition(boardState);

            // make copy of board that can be safely modified
            char[,] stateCopy;

            // get player color
            char color = maximizingPlayer ? '@' : 'O';

            if (maximizingPlayer)
            {
                int maxEval = int.MinValue;
                List<int[]> validMoves = GetValidMoves(color, boardState);

                // if there's no moves available, pass to the next ply
                if (validMoves == null)
                {
                    int eval = AlphaBetaSearch(boardState, depth - 1, alpha, beta, false);
                    maxEval = Math.Max(maxEval, eval);
                }
                else // else if moves are available, loop through them
                {
                    // loop through valid moves
                    foreach (int[] position in validMoves)
                    {
                        stateCopy = (char[,])boardState.Clone();
                        ApplyMove(position, stateCopy, color);
                        statesAnalyzed++;
                        int eval = AlphaBetaSearch(stateCopy, depth - 1, alpha, beta, false);
                        maxEval = Math.Max(maxEval, eval);
                        alpha = Math.Max(alpha, eval);
                        if (beta <= alpha)
                            break;
                    }
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                List<int[]> validMoves = GetValidMoves(color, boardState);
                // if there's no moves available, pass to the next ply
                if (validMoves == null)
                {
                    int eval = AlphaBetaSearch(boardState, depth - 1, alpha, beta, true);
                    minEval = Math.Min(minEval, eval);
                }
                else // else if moves are available, loop through them
                {
                    foreach (int[] position in validMoves)
                    {
                        stateCopy = (char[,])boardState.Clone();
                        ApplyMove(position, stateCopy, color);
                        statesAnalyzed++;
                        int eval = MinimaxSearch(stateCopy, depth - 1, true);
                        minEval = Math.Min(minEval, eval);
                        beta = Math.Min(beta, eval);
                        if (beta <= alpha)
                            break;
                    }
                }
                return minEval;
            }
        }

        // apply the supplied move to the supplied copy of the board
        private void ApplyMove(int[] move, char[,] boardCopy, char turnColor)
        {
            List<int[]> positionsToUpdate = AttachedGame.CheckMove(move, turnColor, boardCopy);
            positionsToUpdate.ForEach(position =>
            {
                boardCopy[position[0], position[1]] = turnColor;
            });
        }

        // returns list of valid positions to consider, or null if none are found
        private List<int[]> GetValidMoves(char color, char[,] boardState)
        {
            // list of valid moves to return
            List<int[]> validMoves = new List<int[]>();

            // search each square of board, if a legal move is found return true
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (AttachedGame.CheckMove(new int[] { i, j }, color, boardState) != null)
                        validMoves.Add(new int[] { i, j });

            // if any valid moves are found, return list, else return null
            if (validMoves.Count > 0)
                return validMoves;
            else
                return null;
        }

        // evaluation function from http://www.cs.cornell.edu/~yuli/othello/othello.html
        private int EvaluatePosition(char[,] boardState)
        {
            // each players score
            float blackScore = 0;
            float whiteScore = 0;

            // number of valid moves for each color
            List<int[]> validMoves;
            int blackNumMoves = 0, whiteNumMoves = 0;
            if ((validMoves = GetValidMoves('@', boardState)) != null)
                blackNumMoves = validMoves.Count;
            if ((validMoves = GetValidMoves('O', boardState)) != null)
                whiteNumMoves = validMoves.Count;

            // if any moves remain for either player, eval as normal
            if (whiteNumMoves > 0 || blackNumMoves > 0)
            {
                // check for posession of corners
                #region Corners
                if (boardState[0, 0] == '@')
                    blackScore += 10;
                else if (boardState[0, 0] == 'O')
                    whiteScore += 10;
                if (boardState[0, 7] == '@')
                    blackScore += 10;
                else if (boardState[0, 7] == 'O')
                    whiteScore += 10;
                if (boardState[7, 0] == '@')
                    blackScore += 10;
                else if (boardState[7, 0] == 'O')
                    whiteScore += 10;
                if (boardState[7, 7] == '@')
                    blackScore += 10;
                else if (boardState[7, 7] == 'O')
                    whiteScore += 10;
                #endregion

                // check number of available moves
                blackScore += blackNumMoves;
                whiteScore += whiteNumMoves;

                // check for number of pieces possesed
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (boardState[i, j] == '@')
                            blackScore += 0.01f;
                        else if (boardState[i, j] == 'O')
                            whiteScore += 0.01f;
                    }
                }
            }
            else // if no moves remain the game is over, and the evaluation should be how many pieces each player has
            {
                // check for number of pieces possesed
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (boardState[i, j] == '@')
                            blackScore += 1;
                        else if (boardState[i, j] == 'O')
                            whiteScore += 1;
                    }
                }
            }

            // return difference in scores rounded to nearest int
            return (int)Math.Round(blackScore - whiteScore);
        }

        // returns true if there are no legal moves for either black or white
        private bool GameOver(char[,] boardState)
        {
            return (!AttachedGame.HasLegalMoves('@', boardState) && !AttachedGame.HasLegalMoves('O', boardState));
        }
    }
}
