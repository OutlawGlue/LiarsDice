using System;
using System.IO;

namespace LiarsDice //MAIN PROGRAM - MAKE SURE TO SAVE  ==============================================
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu();
            Console.ReadLine();
        }
        public struct data
        {
            public string playerType;
            public int number;
            public int count;
            public int[] dice;
            public int colour;
        }
        public static class global
        {
            public static string[] diceTextures = new string[30];
            public static data[] allPlayers = new data[4];
            public static string diceFile = "defaultDice.txt";
            public static int prevQuantity = 0;
            public static int prevDice = 0;
            public static int totalCount;
            public static int playerCount = 4;
            public static bool texturesLoaded;
            public static string gameMode = "standard";
            public static bool liarCalled = false;
            public static int restartingPlayer = 0;
        }

        public static ConsoleColor fontColor = ConsoleColor.White;
        public static ConsoleColor backgroundColor = ConsoleColor.Black;

        static void Menu()
        {
            int input = 0;
        //Display menu, choose option:
        start:
            Console.Write("---Welcome to Liar's Dice---\n" +
                "1 - Play\n" +
                "2 - Rules\n" +
                "3 - Settings\n" +
                "4 - Exit\n" +
                "Enter a number:\t");
            try
            {
                input = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Error - invalid input");
                Console.ReadLine();
                Console.Clear(); goto start;
            }
            Console.Clear();

            switch (input)
            {
                case 1: GameParameters(); break;
                case 2: Rules(); goto start;
                case 3: Settings(); goto start;
                case 4: Environment.Exit(1); break;
                default: Console.WriteLine("Error - invalid input"); Console.ReadLine(); Console.Clear(); goto start;
            }
        }
        static void Settings()
        {
            //Display settings, choose option:
            int input = 0;
        start:
            Console.Write("---Settings---\n" +
                "1 - Change dice\n" +
                "2 - Change player colours\n" +
                "3 - Change game mode\n" +
                "4 - Back to menu\n" +
                "Enter a number:\t");
            try
            {
                input = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Error - invalid input");
                Console.ReadLine();
                Console.Clear(); goto start;
            }
            Console.Clear();

            switch (input)
            {
                case 1: ChangeTextures(false); break;
                case 2: SetPlayerColour(); Console.Clear(); goto start;
                case 3: GameMode(); goto start;
                case 4: Menu(); break;
                default: Console.WriteLine("Error - invalid input"); Console.ReadLine(); Console.Clear(); goto start;
            }
        }
        static void Rules()
        {
            //Display rules:
            Console.Clear();
            Console.WriteLine("---Rules---\n" +
                "Setup: Each player rolls their five dice, keeping their results secret from other players.\n\n" +
                "The first player starts by making a call. A call consists of two parts:\n" +
                "\t Quantity: The number of dice they believe are showing a specific number.\n" +
                "\t Number: The number they are calling (e.g. 3 4's)\n\n" +
                "Subsequent calls: The next player can either:\n" +
                "\tRaise the call: Increase the quantity of the same number (e.g. 4 4's) \n" +
                "\tChange the number: Change the number being called on (e.g. 3 5's) \n" +
                "\tChallenge: Accuse the previous player of lying. \n\n" +
                "Challenge: If a player challenges, all players reveal their dice.\n" +
                "\tIf the challenger is correct: The player who made the false call losses 1 dice.\n" +
                "\tIf the challenger is incorrect: The challenger looses 1 dice.\n\n" +
                "Continuing the Game: The player to the left of the person who was eliminated...\n" +
                "(or the player who successfully raised the call) starts a new round by making a new call.\n\n" +
                "0 dice: when a player has no more dice they are out of the eliminated...\n\n" +
                "Game End: The game continues until only one player remains.");
            Console.ReadLine();
            Console.Clear();
        }
        static void GameParameters()
        {
            //Get player count and validate:
            int playerCount = 0;
            int compCount = 0;
        start:
            Console.WriteLine("Up to 4 players can play... \n" +
                "how many players do you want?");
            try
            {
                playerCount = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {

                Console.Clear();
                Console.Write("Input was not in int format.");
                goto start;
            }

            if (playerCount < 1 || playerCount > 4)
            {

                Console.Clear();
                Console.WriteLine("Input was not within the valid range.");
                goto start;
            }

            //Get AI count and validate:
            if (4 - playerCount > 0)
            {
                Console.WriteLine("up to {0} computer(s) can play...\n" +
                    "how many computers do you want", 4 - playerCount);
                try
                {
                    compCount = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {

                    Console.Clear();
                    Console.Write("Input was not in int format.");
                    goto start;
                }

                if (compCount <= 0 && playerCount == 1 || compCount > 3 || compCount < 0)
                {
                    Console.Clear();
                    Console.WriteLine("Input was not within the valid range.");
                    goto start;
                }
            }
            //TEMPORARY UNTIL AI CODE MADE!!!--------------------------------
            compCount = 0;
            //TEMPORARY UNTIL AI CODE MADE!!!--------------------------------

            global.totalCount = playerCount + compCount;
            global.playerCount = playerCount;
            int count = 0;
            for (int i = 0; i < playerCount; i++)
            {
                global.allPlayers[i].playerType = "Player";
                global.allPlayers[i].number = i + 1;
            }
            for (int j = playerCount; j < global.totalCount; j++)
            {
                count++;
                global.allPlayers[j].playerType = "Computer";
                global.allPlayers[j].number = count;
            }
            RunGame(playerCount);
        }
        static void ChangeTextures(bool error)
        {
            //Display error if required:
            Console.Clear();
            Console.WriteLine("---Change Dice---");
            if (error == true)
            {
                Console.WriteLine("File not found.\n");
            }
        retry:
            //Get filename, validate and update:
            Console.WriteLine("Please enter the filename:");
            string filename = Console.ReadLine();
            if (filename.Contains(".txt"))
            {
                global.diceFile = filename;
            }
            else if (filename != null)
            {
                global.diceFile = filename + ".txt";
            }
            else
            {
                Console.WriteLine("Please enter a filename.");
                goto retry;
            }
            LoadTextures();
            Console.Clear();
            Menu();
        }
        static void GameMode()
        {
            //Display game mode, choose option:
            int input = 0;
        start:
            Console.Write("---Game Mode---\n" +
                "1 - Standard\n" +
                "2 - Inverted\n" +
                "Enter a number:\t");
            try
            {
                input = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Error - invalid input");
                Console.ReadLine();
                Console.Clear(); goto start;
            }
            Console.Clear();

            switch (input)
            {
                case 1: global.gameMode = "standard"; break;
                case 2: global.gameMode = "inverted"; break;
                default: Console.WriteLine("Error - invalid input"); Console.ReadLine(); Console.Clear(); goto start;
            }
        }

        static void RunGame(int playerCount) //RUN COMP GUESSING, COUNT DICE SUBROUTINE
        {
            //Start game:
            InitialiseDice();
            RandomiseDice();
            if (global.texturesLoaded == false)
            {
                LoadTextures();
            }
            //Loop to run game indefinitely:
            bool gameOver = false;
        reset:
            RandomiseDice();
            bool firstRun = true;
            while (!gameOver)
            {
                for (int i = 0; i < playerCount; i++)
                {
                    //Only start at restarting player on first re run:
                    if (firstRun == true)
                    {
                        i = global.restartingPlayer;
                        firstRun = false;
                    }

                    //Run the game:
                    Console.Clear();
                    if (global.allPlayers[i].count > 0)
                    {
                        Console.WriteLine($"Player {i + 1}'s turn\nHide the screen, then press enter.");
                        Console.ReadLine();
                        Console.Clear();
                        DisplayDice(i);
                        GetPrediction(i);
                    }

                    if (global.liarCalled == true)
                    {
                        global.liarCalled = false;
                        int remainingPlayers = 0;
                        for (int j = 0; j < global.playerCount; j++)
                        {
                            if (global.allPlayers[j].count > 0)
                            {
                                remainingPlayers++;
                            }
                        }

                        if (remainingPlayers >= 2)
                        {
                            goto reset;
                        }
                        else
                        {
                            Console.WriteLine("Game over!");
                            Console.ReadLine();
                        }
                    }
                }

                //*NOW RUN COMP GUESSING*
            }
        }
        static void InitialiseDice()
        {
            //Load dice array:
            for (int i = 0; i < global.allPlayers.Length; i++)
            {
                global.allPlayers[i].dice = new int[5];
            }
            for (int player = 0; player < global.totalCount; player++)
            {
                for (int die = 0; die < 5; die++)
                {
                    global.allPlayers[player].dice[die] = 0;
                }
                global.allPlayers[player].count = 5;
            }
        }
        static void LoadTextures()
        {
            //Read in textures:
            string[] temp = new string[30];
            try
            {
                using (StreamReader sr = new StreamReader(global.diceFile))
                {
                    for (int i = 0; i < 30; i++)
                    {
                        temp[i] = sr.ReadLine();
                        global.diceTextures[i] = temp[i];
                    }
                }
                global.texturesLoaded = true;
            }
            catch
            {
                ChangeTextures(true);
            }
        }
        static void RandomiseDice()
        {
            //Give each player 5 new dice values:
            Random rnd = new Random();
            for (int player = 0; player < global.totalCount; player++)
            {
                for (int die = 0; die < global.allPlayers[player].count; die++)
                {
                    global.allPlayers[player].dice[die] = rnd.Next(1, 7);
                }
            }
        }

        static void DisplayDice(int player)
        {
            fontColor = ConsoleColor.White;
            backgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = fontColor;
            Console.BackgroundColor = backgroundColor;

            //Output each dice of selected player:
            int[] dV = new int[5];
            dV = global.allPlayers[player].dice;

            Console.WriteLine($"Player {global.allPlayers[player].number}'s turn:\n");

            switch (global.allPlayers[global.allPlayers[player].number - 1].colour)
            {
                case 1: fontColor = ConsoleColor.White; backgroundColor = ConsoleColor.Black; break;
                case 2: fontColor = ConsoleColor.Black; backgroundColor = ConsoleColor.White; break;
                case 3: fontColor = ConsoleColor.Red; backgroundColor = ConsoleColor.Black; break;
                case 4: fontColor = ConsoleColor.Yellow; backgroundColor = ConsoleColor.Black; break;
                case 5: fontColor = ConsoleColor.Green; backgroundColor = ConsoleColor.Black; break;
                case 6: fontColor = ConsoleColor.Cyan; backgroundColor = ConsoleColor.Black; break;
                case 7: fontColor = ConsoleColor.Blue; backgroundColor = ConsoleColor.Black; break;
                case 8: fontColor = ConsoleColor.Magenta; backgroundColor = ConsoleColor.Black; break;
                case 9: fontColor = ConsoleColor.DarkBlue; backgroundColor = ConsoleColor.DarkGray; break;
                case 10: fontColor = ConsoleColor.DarkMagenta; backgroundColor = ConsoleColor.DarkGray; break;
                default: break;
            }
            Console.ForegroundColor = fontColor; Console.BackgroundColor = backgroundColor;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < global.allPlayers[player].count; j++)
                {
                    Console.Write(global.diceTextures[((dV[j] - 1) * 5) + i] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        static void SetPlayerColour()
        {
            for (int i = 0; i < 3; i++)
            {
                global.allPlayers[i].colour = 0;
            }

            Console.Clear();
            Console.WriteLine("---Change Colours---");
            OutputColours();
            for (int i = 0; i < global.playerCount; i++)
            {
                Console.Write("Player {0} - choose a colour set: ", i + 1);
                global.allPlayers[i].colour = Convert.ToInt32(Console.ReadLine());
            }
        }

        static void OutputColours()
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("Black(1)  ");

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("White(2)  ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("Red(3)  ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("Yellow(4)  ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("Green(5)  ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("Cyan(6)  ");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("Blue(7)  ");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("Magenta(8)  ");

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("Blue(9)  ");

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("Magenta(10)  ");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void GetPrediction(int i) //CALL LIAR SUBROUTINE
        {
            //Check for liar call and validate:
            if (global.prevQuantity > 0)
            {
                Console.WriteLine($"Previous call was {global.prevQuantity} {global.prevDice}'s.\n");
            retry:
                Console.Write("\nWould you like to call liar? (Y/N): ");
                string input = Console.ReadLine();
                if (input == "Y" || input == "y")
                {
                    LiarCalled(i);
                    global.liarCalled = true;
                    goto returnLiar;
                }
                else if (input == "N" || input == "n")
                {

                }
                else
                {
                    Console.WriteLine("Invalid input.");
                    goto retry;
                }
            }
            else
            {
                Console.WriteLine("No previous call.");
            }

        //Get prediction and validate:
        error:
            try
            {
                Console.Write("\nPrediction:\nHow many dice? ");
                int quantity = int.Parse(Console.ReadLine());
                Console.Write("Of what dice? ");
                int dice = int.Parse(Console.ReadLine());

                if (quantity < 1 || quantity > 20 || dice < 1 || dice > 6)
                {
                    Console.WriteLine("Input was not within the valid range.");
                    goto error;
                }
                else if (!((dice > global.prevDice) || (dice == global.prevDice && quantity > global.prevQuantity)))
                {
                    Console.WriteLine("Either quantity or the dice must increase.");
                    goto error;
                }
                else if (quantity > 0 && quantity < 21 && dice > 0 && dice < 7)
                {
                    global.prevQuantity = quantity;
                    global.prevDice = dice;
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                    goto error;
                }
            }
            catch
            {
                Console.WriteLine("Input was not in int format.");
                goto error;
            }
        returnLiar:
            ;
        }
        static void LiarCalled(int challenger)
        {
            int count = 0;
            int predictor = challenger - 1;
            while (global.allPlayers[predictor].count <= 0)
            {
                if (predictor < 0)
                {
                    predictor = global.playerCount - 1;
                }
                else
                {
                    predictor--;
                }
            }

            CountDice(ref count);
            Console.Clear();
            Console.WriteLine("---Liar Called---");

            //If predictor should lose a dice:
            if ((global.prevQuantity > count && global.gameMode == "standard") || (global.prevQuantity <= count && global.gameMode == "inverted"))
            {
                global.allPlayers[predictor].count--;
                global.allPlayers[predictor].dice[global.allPlayers[predictor].count] = 0;

                //Determine appropriate message:
                if (global.gameMode == "standard")
                {
                    Console.WriteLine($"Challenger ({global.allPlayers[challenger].playerType} {challenger + 1}) wins! There were only {count} {global.prevDice}'s.");
                    global.restartingPlayer = predictor;
                }
                else
                {
                    Console.WriteLine($"Predictor ({global.allPlayers[predictor].playerType} {predictor + 1}) wins! There were {count} {global.prevDice}'s.");
                    global.restartingPlayer = challenger;
                }

                Console.WriteLine($"Predictor ({global.allPlayers[predictor].playerType} {predictor + 1}) now has {global.allPlayers[predictor].count} dice.");
            }
            else if ((global.prevQuantity <= count && global.gameMode == "standard") || global.prevQuantity > count && global.gameMode == "standard")
            {
                global.allPlayers[challenger].count--;
                global.allPlayers[challenger].dice[global.allPlayers[challenger].count] = 0;

                //Determine appropriate message:
                if (global.gameMode == "standard")
                {
                    Console.WriteLine($"Predictor ({global.allPlayers[predictor].playerType} {predictor + 1}) wins! There were {count} {global.prevDice}'s.");
                    global.restartingPlayer = challenger;
                }
                else
                {
                    Console.WriteLine($"Challenger ({global.allPlayers[challenger].playerType} {challenger + 1}) wins! There were only {count} {global.prevDice}'s.");
                    global.restartingPlayer = predictor;
                }

                Console.WriteLine($"Challenger ({global.allPlayers[challenger].playerType} {challenger + 1}) now has {global.allPlayers[challenger].count} dice.");
            }
            //Reset call:
            global.prevQuantity = 0;
            global.prevDice = 0;
            Console.WriteLine("\nPress enter to continue.");
            Console.ReadLine();
        }
        static void CountDice(ref int count)
        {
            for (int player = 0; player < global.playerCount; player++)
            {
                for (int dice = 0; dice < global.allPlayers[player].count; dice++)
                {
                    if (global.allPlayers[player].dice[dice] == global.prevDice)
                    {
                        count++;
                    }
                    else if (global.allPlayers[player].dice[dice] == 1)
                    {
                        count++;
                    }
                }
            }
        }
    }
}
