using System;
using System.IO;

namespace LiarsDice //MAIN PROGRAM - MAKE SURE TO SAVE  ==============================================
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Liar's Dice";
            Menu();
            Console.ReadLine();
        }
        public struct Data
        {
            public string name;
            public int number;
            public bool useNumber;
            public int count;
            public int[] dice;
            public int colour;
            public int texture;
        }
        public static class Global
        {
            public static int numberOfTextutesAllowed = 20;
            public static string[] diceTextures = new string[30];
            public static string[,] setDiceTextures = new string[numberOfTextutesAllowed, 30];
            public static string[] diceTextureNames = new string[numberOfTextutesAllowed];
            public static int filesLoaded = 0;
            public static Data[] allPlayers = new Data[4];
            public static string diceFile = "defaultDice.txt";
            public static int prevQuantity = 0;
            public static int prevDice = 0;
            public static int totalCount;
            public static int playerCount;
            public static bool texturesLoaded;
            public static string gameMode = "standard";
            public static bool liarCalled = false;
            public static int restartingPlayer = 0;
        }

        public static ConsoleColor fontColor = ConsoleColor.White;
        public static ConsoleColor backgroundColor = ConsoleColor.Black;

        static void Menu()
        {
            int input;
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
            //If counts have not already been set:
            if (Global.playerCount < 1)
            {
                GetPlayerCount();
                GetCompCount();
                Console.Clear();
            }

            //Display settings, choose option:
            int input;
        start:
            Console.Write("---Settings---\n" +
                "1 - Change dice\n" +
                "2 - Change player colours\n" +
                "3 - Set player names\n" +
                "4 - Back to menu\n" +
                "Enter a number:\t");//gamemode still available
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
                //case 3: GameMode(); goto start;
                case 3: PlayerNames(); Console.Clear(); goto start;
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
        static void GetPlayerCount()
        {
            int playerCount;
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
                Console.WriteLine("Input was not in int format.");
                goto start;
            }

            if (playerCount < 1 || playerCount > 4)
            {

                Console.Clear();
                Console.WriteLine("Input was not within the valid range.");
                goto start;
            }
            Global.playerCount = playerCount;
        }
        static void GetCompCount()
        {
            int playerCount = Global.playerCount;
            int compCount;
        //Get AI count and validate:
        start:
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
            Global.totalCount = playerCount + compCount;
            InitialiseDice();
            RandomiseDice();
        }
        static void GameParameters()
        {
            //Get player count and validate:
            if (Global.playerCount < 1)
            {
                GetPlayerCount();
                GetCompCount();
                Console.Clear();
            }
            int playerCount = Global.playerCount;
            int count = 0;
            for (int i = 0; i < playerCount; i++)
            {
                if (Global.allPlayers[i].useNumber == true)
                {
                    string name = "Player " + (i + 1);
                    Global.allPlayers[i].name = name;
                }
                Global.allPlayers[i].number = i + 1;
            }
            for (int j = playerCount; j < Global.totalCount; j++)
            {
                count++;
                Global.allPlayers[j].name = "Computer";
                Global.allPlayers[j].number = count;
            }
            RunGame();
        }
        static void ChangeTextures(bool error)
        {
            if (error == true)
            {
                Console.WriteLine("File not found\n");
            }
            
            Console.Clear();
            string[] filename = new string[Global.numberOfTextutesAllowed];
            FileInfo files = new FileInfo(Path.GetFileName(".txt"));
            string path = files.DirectoryName;
            int foreachCounter = 0;
            foreach (string file in Directory.EnumerateFiles(path, "*.txt"))
            {
                string hold2 = @"\";
                string[] hold = file.Split(Convert.ToChar(hold2));
                filename[foreachCounter] = hold[hold.Length - 1];   
                Console.WriteLine(foreachCounter + ": " + filename[foreachCounter]);
                Global.diceTextureNames[foreachCounter] = filename[foreachCounter];
                //Read in textures:
                string[] temp = new string[30];
                try
                {
                    using (StreamReader sr = new StreamReader(file))
                    {
                        for (int i = 0; i < 30; i++)
                        {
                            temp[i] = sr.ReadLine();
                            Global.setDiceTextures[foreachCounter, i] = temp[i];
                        }
                    }
                    Global.texturesLoaded = true;
                    Global.filesLoaded++;
                }
                catch
                {
                    ChangeTextures(true);
                }


                //Output examples for each text file:
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        Console.Write(Global.setDiceTextures[foreachCounter,(j * 5) + i] + "\t");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();

                
                foreachCounter++;
            }

            for (int i = 0; i < Global.playerCount; i++)
            {
                Console.WriteLine($"{Global.allPlayers[i].name} choose a text file:");
                Global.allPlayers[i].texture = int.Parse(Console.ReadLine());
            }


            Console.ReadLine();
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
                //"2 - Inverted\n" +
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
                case 1: Global.gameMode = "standard"; break;
                //case 2: Global.gameMode = "inverted"; break;
                default: Console.WriteLine("Error - invalid input"); Console.ReadLine(); Console.Clear(); goto start;
            }
        }

        static void RunGame() //RUN COMP GUESSING
        {
            //Start game:
            if (Global.allPlayers[0].dice.Length == 0)
            {
                InitialiseDice();
                RandomiseDice();
            }

            if (Global.texturesLoaded == false)
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
                for (int i = 0; i < Global.playerCount; i++)
                {
                    //Only start at restarting player on first re run:
                    if (firstRun == true)
                    {
                        i = Global.restartingPlayer;
                        firstRun = false;
                    }

                    //Run the game:
                    Console.Clear();
                    if (Global.allPlayers[i].count > 0)
                    {
                        Console.WriteLine($"{Global.allPlayers[i].name}'s turn\nHide the screen, then press enter.");
                        Console.ReadLine();
                        Console.Clear();
                        DisplayDice(i);
                        GetPrediction(i);
                    }

                    if (Global.liarCalled == true)
                    {
                        int remainingPlayers = 0;
                        int pl = 0;
                        int winner = -1;
                        string winnerName = null;
                        while (pl < Global.playerCount && remainingPlayers < 2)
                        {
                            if (Global.allPlayers[pl].count > 0)
                            {
                                remainingPlayers++;
                                winner = Global.allPlayers[pl].number;
                                winnerName = Global.allPlayers[pl].name;
                            }
                            pl++;
                        }

                        if (remainingPlayers < 2)
                        {
                            Console.Clear();
                            Console.WriteLine($"Game over!\n{winnerName} {winner} is the winner!");
                            Console.ReadLine();
                        }

                        Global.liarCalled = false;
                        goto reset;
                    }
                }

                //*NOW RUN COMP GUESSING*
            }
        }
        static void InitialiseDice()
        {
            //Load dice array:
            for (int i = 0; i < Global.allPlayers.Length; i++)
            {
                Global.allPlayers[i].dice = new int[5];
                Global.allPlayers[i].useNumber = true;
            }
            for (int player = 0; player < Global.totalCount; player++)
            {
                for (int die = 0; die < 5; die++)
                {
                    Global.allPlayers[player].dice[die] = 0;
                }
                Global.allPlayers[player].count = 5;
            }
        }
        static void LoadTextures()
        {
            //Read in textures:
            string[] temp = new string[30];
            try
            {
                using (StreamReader sr = new StreamReader(Global.diceFile))
                {
                    for (int i = 0; i < 30; i++)
                    {
                        temp[i] = sr.ReadLine();
                        Global.diceTextures[i] = temp[i];
                    }
                }
                Global.texturesLoaded = true;
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
            for (int player = 0; player < Global.totalCount; player++)
            {
                for (int die = 0; die < Global.allPlayers[player].count; die++)
                {
                    Global.allPlayers[player].dice[die] = rnd.Next(1, 7);
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
            dV = Global.allPlayers[player].dice;

            Console.WriteLine($"{Global.allPlayers[player].name}'s turn:\n");

            switch (Global.allPlayers[Global.allPlayers[player].number - 1].colour)
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
                for (int j = 0; j < Global.allPlayers[player].count; j++)
                {
                    Console.Write(Global.setDiceTextures[Global.allPlayers[player].texture, ((dV[j] - 1) * 5) + i] + "\t");
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
                Global.allPlayers[i].colour = 0;
            }

            Console.Clear();
            Console.WriteLine("---Change Colours---");
            OutputColours();
            for (int i = 0; i < Global.playerCount; i++)
            {
                Console.Write($"{Global.allPlayers[i].name} - choose a colour set: ");
                Global.allPlayers[i].colour = Convert.ToInt32(Console.ReadLine());
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
        static void PlayerNames()
        {
            Console.WriteLine("---Player Names---");
            string tempName = null;
            int playerCheckCounter = 1;
            for (int i = 0; i < Global.playerCount; i++)
            {
                start:
                Console.Write($"Player {i + 1}, enter your name: ");
                tempName = Console.ReadLine();
                for (int j = 0; j < playerCheckCounter; j++)
                {
                    if(Global.allPlayers[j].name == tempName)
                    {
                        Console.WriteLine("error- username has already been used"); goto start;
                    }
                }
                Global.allPlayers[i].useNumber = false;
                Global.allPlayers[i].name = tempName;
                playerCheckCounter++;

            }
        }

        static void GetPrediction(int i)
        {
            //Check for liar call and validate:
            if (Global.prevQuantity > 0)
            {
                Console.WriteLine($"Previous call was {Global.prevQuantity} {Global.prevDice}'s.\n");
            retry:
                Console.Write("\nWould you like to call liar? (Y/N): ");
                string input = Console.ReadLine();
                if (input == "Y" || input == "y")
                {
                    LiarCalled(i);
                    Global.liarCalled = true;
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
                else if (!((dice > Global.prevDice) || (dice == Global.prevDice && quantity > Global.prevQuantity)))
                {
                    Console.WriteLine("Either quantity or the dice must increase.");
                    goto error;
                }
                else if (quantity > 0 && quantity < 21 && dice > 0 && dice < 7)
                {
                    Global.prevQuantity = quantity;
                    Global.prevDice = dice;
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
            while (Global.allPlayers[predictor].count <= 0)
            {
                if (predictor < 0)
                {
                    predictor = Global.playerCount - 1;
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
            if ((Global.prevQuantity > count && Global.gameMode == "standard") || (Global.prevQuantity <= count && Global.gameMode == "inverted"))
            {
                Global.allPlayers[predictor].count--;
                Global.allPlayers[predictor].dice[Global.allPlayers[predictor].count] = 0;

                //Determine appropriate message:
                if (Global.gameMode == "standard")
                {
                    Console.WriteLine($"Challenger ({Global.allPlayers[challenger].name}) wins! There were only {count} {Global.prevDice}'s.");
                    Global.restartingPlayer = predictor;
                }
                else
                {
                    Console.WriteLine($"Predictor ({Global.allPlayers[predictor].name}) wins! There were {count} {Global.prevDice}'s.");
                    Global.restartingPlayer = challenger;
                }

                Console.WriteLine($"Predictor ({Global.allPlayers[predictor].name}) now has {Global.allPlayers[predictor].count} dice.");
            }
            else if ((Global.prevQuantity <= count && Global.gameMode == "standard") || Global.prevQuantity > count && Global.gameMode == "standard")
            {
                Global.allPlayers[challenger].count--;
                Global.allPlayers[challenger].dice[Global.allPlayers[challenger].count] = 0;

                //Determine appropriate message:
                if (Global.gameMode == "standard")
                {
                    Console.WriteLine($"Predictor ({Global.allPlayers[predictor].name}) wins! There were {count} {Global.prevDice}'s.");
                    Global.restartingPlayer = challenger;
                }
                else
                {
                    Console.WriteLine($"Challenger ({Global.allPlayers[challenger].name}) wins! There were only {count} {Global.prevDice}'s.");
                    Global.restartingPlayer = predictor;
                }

                Console.WriteLine($"Challenger ({Global.allPlayers[challenger].name}) now has {Global.allPlayers[challenger].count} dice.");
            }
            //Reset call:
            Global.prevQuantity = 0;
            Global.prevDice = 0;
            Console.WriteLine("\nPress enter to continue.");
            Console.ReadLine();
        }
        static void CountDice(ref int count)
        {
            for (int player = 0; player < Global.playerCount; player++)
            {
                for (int dice = 0; dice < Global.allPlayers[player].count; dice++)
                {
                    if (Global.allPlayers[player].dice[dice] == Global.prevDice)
                    {
                        count++;
                    }
                    else if (Global.allPlayers[player].dice[dice] == 1)
                    {
                        count++;
                    }
                }
            }
        }
    }
}
// Luke was here
