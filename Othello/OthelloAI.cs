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
        private int movesConsidered;

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
        public void MakeMove()
        {
            // reset moves considered
            movesConsidered = 0;

            // get current board state from game
            char[,] boardState = (char[,]) AttachedGame.BoardState.Clone();

            // TODO: These methods should be called on the current available moves
            if (AlphaBetaActive)
            {
                AlphaBetaSearch(boardState, MAX_DEPTH, int.MinValue, int.MaxValue, IsBlackPlayer);
            }
            else
                MinimaxSearch(boardState, MAX_DEPTH, IsBlackPlayer);

            // TODO: Finish method
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
            char color = IsBlackPlayer ? '@' : 'O';

            if (maximizingPlayer)
            {
                int maxEval = int.MinValue;
                List<int[]> validMoves = GetValidMoves(color, boardState);
                validMoves.ForEach(position =>
                {
                    stateCopy = (char[,])boardState.Clone();
                    ApplyMove(position, stateCopy, color);
                    int eval = MinimaxSearch(stateCopy, depth - 1, false);
                    maxEval = Math.Max(maxEval, eval);
                });
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                List<int[]> validMoves = GetValidMoves(color, boardState);
                validMoves.ForEach(position =>
                {
                    stateCopy = (char[,])boardState.Clone();
                    ApplyMove(position, stateCopy, color);
                    int eval = MinimaxSearch(stateCopy, depth - 1, true);
                    minEval = Math.Min(minEval, eval);
                });
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
            char color = IsBlackPlayer ? '@' : 'O';

            if (maximizingPlayer)
            {
                int maxEval = int.MinValue;
                List<int[]> validMoves = GetValidMoves(color, boardState);
                foreach(int[] position in validMoves)
                {
                    stateCopy = (char[,])boardState.Clone();
                    ApplyMove(position, stateCopy, color);
                    int eval = MinimaxSearch(stateCopy, depth - 1, false);
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                        break;
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                List<int[]> validMoves = GetValidMoves(color, boardState);
                foreach (int[] position in validMoves)
                {
                    stateCopy = (char[,])boardState.Clone();
                    ApplyMove(position, stateCopy, color);
                    int eval = MinimaxSearch(stateCopy, depth - 1, true);
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                        break;
                }
                return minEval;
            }
        }

        // apply the supplied move to the supplied copy of the board
        private void ApplyMove(int[] move, char[,] boardCopy, char turnColor)
        {
            throw new NotImplementedException();
        }

        // returns list of valid positions to consider, or null if none are found
        private List<int[]> GetValidMoves(char color, char[,] boardState)
        {
            throw new NotImplementedException();
        }

        private int EvaluatePosition(char[,] boardState)
        {
            throw new NotImplementedException();
        }

        private bool GameOver(char[,] boardState)
        {
            throw new NotImplementedException();
        }
    }
}
