using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class Driver
    {
        static void Main(string[] args)
        {
            // object tracks Othello game
            OthelloGame game = new OthelloGame();

            // object controls Othello AI
            OthelloAI ai = new OthelloAI();

            // queue stores sequence of moves from a game
            Queue<string> boardTrace = new Queue<string>();

            // string stores user input for processing
            string userInput;

            // user interface main loop
            bool continueFlag = true;  // bool set to false on program exit to break while-loop
            while(continueFlag)
            {
                // display main menu text
                Console.WriteLine("Welcome to Othello\n\n1. Play Singleplayer vs. AI\n2. Play Multiplayer\n3. Show board trace of previous run\n4. Save board trace of previous run\n5. Exit Othello\n");
                Console.Write("Please enter the number of your selection: ");
                userInput = Console.ReadLine();

                // process user input
                switch (userInput)
                {
                    case "1":
                        PlaySingleplayer(game, ai, boardTrace);
                        break;

                    case "2":
                        PlayMultiplayer(game, boardTrace);
                        break;

                    case "3":
                        break;

                    case "4":
                        break;

                    case "5":
                        continueFlag = false;
                        break;

                    default:
                        Console.WriteLine("You entered an invalid selection\n");
                        break;
                }
            }
        }

        private static void PlaySingleplayer(OthelloGame game, OthelloAI ai, Queue<string> boardTrace)
        {
            throw new NotImplementedException();
        }

        private static void PlayMultiplayer(OthelloGame game, Queue<string> boardTrace)
        {

        }
    }
}
