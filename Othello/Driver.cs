using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Othello
{
    class Driver
    {
        // debug mode
        public static bool DEBUG_MODE = false;

        static void Main(string[] args)
        {
            // object tracks Othello game
            OthelloGame game = new OthelloGame();

            // queue stores sequence of moves from a game
            Queue<string> boardTrace = null;

            // string stores user input for processing
            string userInput;

            // user interface main loop
            bool continueFlag = true;  // bool set to false on program exit to break while-loop
            while(continueFlag)
            {
                Console.Clear();

                // display main menu text
                Console.WriteLine("Welcome to Othello\n\n1. Play Singleplayer vs. AI\n2. Play Multiplayer\n3. Show board trace of previous run\n4. Save board trace of previous run\n5. Exit Othello\n");
                Console.Write("Please enter the number of your selection: ");
                userInput = Console.ReadLine();

                // process user input
                switch (userInput)
                {
                    case "1":
                        boardTrace = new Queue<string>();
                        InitSingleplayer(game, boardTrace);
                        break;

                    case "2":
                        boardTrace = new Queue<string>();
                        PlayMultiplayer(game, boardTrace);
                        break;

                    case "3":
                        if (boardTrace != null)
                        {
                            for (int i = 0; i < boardTrace.Count; i++)
                            {
                                Console.WriteLine(boardTrace.Peek());
                                boardTrace.Enqueue(boardTrace.Dequeue());
                            }
                        }
                        else Console.WriteLine("Game must be played or board trace must be loaded before viewing.");
                        break;

                    case "4":
                        break;

                    case "5":
                        continueFlag = false;
                        break;

                    default:
                        Console.WriteLine("You entered an invalid selection. Press enter to continue...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void InitSingleplayer(OthelloGame game, Queue<string> boardTrace)
        {
            char playerColor = '\0', aiColor = '\0';
            string userInput;
            bool validInput = false;
            OthelloAI ai = null;

            // loop until user gives a valid color selection
            while (!validInput)
            {
                Console.Clear();
                Console.Write("Pick the human player color (B/W): ");
                userInput = Console.ReadLine();

                // if users input matches one of the selection options, assign colors
                if (userInput.Length == 1 && Regex.IsMatch(userInput, "[BbWw]"))
                {
                    // if human chose black, assign human black color and ai white color
                    if (Regex.IsMatch(userInput, "[Bb]"))
                    {
                        // initialize colors
                        playerColor = '@';
                        aiColor = 'O';

                        // initialize AI
                        ai = new OthelloAI(game, false);
                    }
                    else // if human chose white, do the reverse
                    {
                        // initialize colors
                        playerColor = 'O';
                        aiColor = '@';

                        // initialize AI
                        ai = new OthelloAI(game, true);
                    }
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Entered color selection was invalid, please try again.\nPress enter to continue...");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("Player pieces are: " + playerColor + "\nAI pieces are: " + aiColor + "\nPress enter to begin...");
            Console.ReadLine();

            bool playerIsBlack = playerColor == '@';
            PlaySingleplayer(game, ai, boardTrace, playerIsBlack);
        }

        private static void PlaySingleplayer(OthelloGame game, OthelloAI ai, Queue<string> boardTrace, bool playerIsBlack)
        {
            string userInput;

            // begin gameplay
            game.ResetBoard();

            // bool tracks whether player entered move or a command such as toggling alpha-beta
            bool validMoveEntered;

            // game loops until neither black nor white have remaining moves
            bool blackHasMoves; // cache result of game.HasLegalMoves('B') to avoid recomputation
            while ((blackHasMoves = game.HasLegalMoves('@')) && game.HasLegalMoves('O'))
            {
                // execution instructions for when player is black
                if (playerIsBlack)
                {
                    if (blackHasMoves)
                    {
                        // loop until black player enters a legal move
                        do
                        {
                            game.PrintBoard();
                            Console.WriteLine("Enter move as ColRow, -1 to toggle Alpha-Beta Pruning, or -2 to toggle debug mode");
                            Console.Write("\nBlack, enter your selection: ");
                            userInput = Console.ReadLine();
                            switch (userInput)
                            {
                                case "-1":
                                    ai.ToggleAlphaBeta();
                                    validMoveEntered = false;
                                    Console.WriteLine($"Alpha-Beta Active is now {ai.AlphaBetaActive}\nPress enter to continue...");
                                    Console.ReadLine();
                                    break;

                                case "-2":
                                    DEBUG_MODE = !DEBUG_MODE;
                                    validMoveEntered = false;
                                    Console.WriteLine($"Debug mode is now {DEBUG_MODE}\nPress enter to continue...");
                                    Console.ReadLine();
                                    break;

                                default:
                                    validMoveEntered = game.PlayMove(userInput);
                                    break;
                            }
                        } while (!validMoveEntered);
                        boardTrace.Enqueue(userInput);
                    }
                    else
                    {
                        Console.WriteLine("Black has no moves. Press enter to pass...");
                        Console.ReadLine();
                    }

                    // wait for user to let ai move
                    game.PrintBoard();
                    Console.WriteLine("White's turn");

                    if (game.HasLegalMoves('O'))
                    {
                        int[] aiMove = ai.ChooseMove();
                        game.PlayMove(aiMove);
                        string aiMoveFormatted = FormatAIMove(aiMove);
                        boardTrace.Enqueue(aiMoveFormatted);
                    }
                    else
                    {
                        Console.WriteLine("White has no moves. Press enter to pass...");
                        Console.ReadLine();
                    }
                }
                else // execution instructions for when player is white 
                {
                    // wait for user to let ai move
                    game.PrintBoard();
                    Console.WriteLine("Black's turn");

                    if (blackHasMoves)
                    {
                        int[] aiMove = ai.ChooseMove();
                        game.PlayMove(aiMove);
                        string aiMoveFormatted = FormatAIMove(aiMove);
                        boardTrace.Enqueue(aiMoveFormatted);
                    }
                    else
                    {
                        Console.WriteLine("Black has no moves. Press enter to pass...");
                        Console.ReadLine();
                    }

                    if (game.HasLegalMoves('O'))
                    {
                        // loop until white player enters a legal move
                        do
                        {
                            game.PrintBoard();
                            Console.WriteLine("Enter move as ColRow, -1 to toggle Alpha-Beta Pruning, or -2 to toggle debug mode");
                            Console.Write("\nWhite, enter your move/selection: ");
                            userInput = Console.ReadLine();
                            switch (userInput)
                            {
                                case "-1":
                                    ai.ToggleAlphaBeta();
                                    validMoveEntered = false;
                                    Console.WriteLine($"Alpha-Beta Active is now {ai.AlphaBetaActive}\nPress enter to continue...");
                                    Console.ReadLine();
                                    break;

                                case "-2":
                                    DEBUG_MODE = !DEBUG_MODE;
                                    validMoveEntered = false;
                                    Console.WriteLine($"Debug mode is now {DEBUG_MODE}\nPress enter to continue...");
                                    Console.ReadLine();
                                    break;

                                default:
                                    validMoveEntered = game.PlayMove(userInput);
                                    break;
                            }
                        } while (!validMoveEntered);
                        boardTrace.Enqueue(userInput);
                    }
                    else
                    {
                        Console.WriteLine("White has no moves. Press enter to pass...");
                        Console.ReadLine();
                    }
                }
            }
            Console.WriteLine("There are no more legal moves. Press enter to continue...");
            Console.ReadLine();

            // score game
            int blackScore = 0, whiteScore = 0;
            foreach (char piece in game.BoardState)
            {
                if (piece == '@')
                    blackScore++;
                else if (piece == 'O')
                    whiteScore++;
            }

            // declare winner
            if (blackScore > whiteScore)
                Console.WriteLine("Black Wins!");
            else if (whiteScore > blackScore)
                Console.WriteLine("White Wins!");
            else
                Console.WriteLine("Game Tied!");

            Console.WriteLine($"Black Score: {blackScore}\nWhite Score: {whiteScore}");
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }

        private static void PlayMultiplayer(OthelloGame game, Queue<string> boardTrace)
        {
            game.ResetBoard();

            string userInput;

            // game loops until neither black nor white have remaining moves
            bool blackHasMoves; // cache result of game.HasLegalMoves('B') to avoid recomputation
            while ((blackHasMoves = game.HasLegalMoves('@')) && game.HasLegalMoves('O'))
            {
                if (blackHasMoves)
                {
                    // loop until black player enters a legal move
                    do
                    {
                        game.PrintBoard();
                        Console.Write("\nBlack, enter your move: ");
                    } while (!game.PlayMove(userInput = Console.ReadLine()));
                    boardTrace.Enqueue(userInput);
                }
                else
                {
                    Console.WriteLine("Black has no moves. Press enter to pass...");
                    Console.ReadLine();
                }

                if (game.HasLegalMoves('O'))
                {
                    // loop until white player enters a legal move
                    do
                    {
                        game.PrintBoard();
                        Console.Write("\nWhite, enter your move: ");
                    } while (!game.PlayMove(userInput = Console.ReadLine()));
                    boardTrace.Enqueue(userInput);
                }
                else
                {
                    Console.WriteLine("White has no moves. Press enter to pass...");
                    Console.ReadLine();
                }
            }
            Console.WriteLine("There are no more legal moves. Press enter to continue...");
            Console.ReadLine();

            // score game
            int blackScore = 0, whiteScore = 0;
            foreach (char piece in game.BoardState)
            {
                if (piece == '@')
                    blackScore++;
                else if (piece == 'O')
                    whiteScore++;
            }

            // declare winner
            if (blackScore > whiteScore)
                Console.WriteLine("Black Wins!");
            else if (whiteScore > blackScore)
                Console.WriteLine("White Wins!");
            else
                Console.WriteLine("Game Tied!");

            Console.WriteLine($"Black Score: {blackScore}\nWhite Score: {whiteScore}");
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }

        private static string FormatAIMove(int[] aiMove)
        {
            char column = (char)(aiMove[0] + 65);
            char row = (char)(aiMove[1] + 49);
            return "" + column + row;
        }
    }
}
