using System;
using System.Collections.Generic;

namespace cshangman
{
    class Program
    {
        static Random rnd = new Random();

        static string[] alphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };


        public static void Main(string[] args)
        {
            Console.WriteLine("***");
            Console.WriteLine("Welcome to Hangman");
            //Activates main hangman game
            Hangman(options());

        }
        //Selects word based on player options
        public static string WordSelect(int player)
        {
            //The return value of the function
            string word = "";

            //Selects a word based of the player settings
            //singleplayer = random word input
            //multiplayer = human word input
            if (player == 1)
            {
                string megaword = System.IO.File.ReadAllText("words.txt");
                string[] wl = megaword.Split('\n');
                word = wl[rnd.Next(1, 1000) - 1];
            }
            else if (player == 2)
            {
                Console.WriteLine("Enter a Word:");
                word = Console.ReadLine();
            }
            return word;
        }

        //Allows player to set main values to play
        public static int options()
        {
            //op is the integer value meant to return
            //check makes sure that the player can't input a faulty return value
            int op = 0;
            bool check = true;

            //User can input an integer value of 1 or 2
            while (check)
            {
                check = false;
                Console.WriteLine("Enter player options:");
                Console.WriteLine("(1 = Singleplayer, 2 = Multiplayer)");
                if (!int.TryParse(Console.ReadLine(), out op))
                {
                    Console.WriteLine("Wrong Input, Try Again");
                    check = true;
                }
                else if (op != 1 && op != 2)
                {
                    Console.WriteLine("Wrong Input, Try Again");
                    check = true;
                }
            }
            return op;
        }

        //Main Game Function
        public static bool Hangman(int player)
        {
            //conlives is a running total of all uniqe inputs to add to the guessed array
            int conlives = 0;
            char[] guessed = new char[26];

            //Select the word based on player options
            string word = WordSelect(player);

            //The array that is being compared to the correct entry array
            char[] main = word.ToCharArray();
            char[] correct = new char[main.Length];

            //The length of the main array
            int length = main.Length;

            //What guess did the player make so far
            string guess = "";

            //Total amount of lives
            int lives = 10;

            //Add dashes to the guess word
            foreach (char c in main)
            {
                guess += "_";
            }

            //checks if the game is complete
            bool win = false;

            bool var3 = true;
            bool var4 = true;

            //Main game loop
            while (win != true)
            {
                if (var3 && var4)
                {
                    Console.WriteLine("Your word is: " + guess);
                    Console.WriteLine("Lives left: " + lives.ToString());

                    //x is used to add guessed chars to the list
                    string x = "";
                    foreach (char c in guessed)
                    {
                        x += c + " ";
                    }

                    Console.WriteLine("Letters you have guessed: " + x);
                    Console.WriteLine("What is your guess: ");
                }

                //g is the guess and it also stops players from entering multi-charachter options
                char g = '_';
                string gs = Console.ReadLine();
                while (!char.TryParse(gs, out g))
                {
                    Console.WriteLine("Wrong Input, Try Again");
                    gs = Console.ReadLine();
                }


                //var4 Makes sure the input is a lowercase letter
                foreach (string c in alphabet)
                {
                    if (c == g.ToString())
                    {
                        var4 = true;
                        break;
                    }
                    else
                    {
                        var4 = false;
                    }

                }
                if (!var4) { Console.WriteLine("Wrong Input, Try Again"); }
                foreach (char c in guessed)
                {
                    if (c == g)
                    {
                        Console.WriteLine("Already entered, try again,");
                        var3 = false;
                        break;
                    }
                    else
                    {
                        var3 = true;
                    }

                }
                if (var3 && var4)
                {
                    Console.WriteLine("***");

                    //add stuff to guessed list
                    guessed[conlives] = g;

                    //pos is used to locate where any correct charachters are
                    //adding conlives to add another entry to guessed
                    //count is to tell how many times the correct word appears
                    int pos = 0;
                    conlives += 1;
                    int count = 0;

                    //Check for correct guesses and state if it was right or wrong
                    foreach (char c in main)
                    {
                        if (g == c)
                        {
                            correct[pos] = g;
                            count++;
                        }
                        pos++;
                    }
                    if (count > 0)
                    {
                        Console.WriteLine("Correct");
                    }
                    else
                    {
                        Console.WriteLine("Wrong");
                        lives += -1;
                    }
                    Console.WriteLine("***");

                    //Reset guess value
                    guess = "";

                    //var1 & var2 are both used in printing the correct guess words
                    bool var1 = false;
                    bool var2 = true;

                    //Change the guessed word due to a correct answer
                    foreach (char c in main)
                    {
                        foreach (char d in correct)
                        {
                            if (c == d && var2)
                            {
                                guess += c;
                                var2 = false;
                            }
                            else
                            {
                                var1 = true;
                            }
                        }
                        if (var1 && var2)
                        {
                            guess += "_";
                        }
                        var2 = true;
                    }

                    //Check if dead
                    if (lives < 0)
                    {
                        win = true;
                        Console.WriteLine("You Lost");
                        Console.WriteLine("Word was: " + word);
                    }

                    //run is used to tally up for a comparison between the word and guessed word
                    int run = 0;

                    //compare correct to main
                    for (int i = 0; i < main.Length; i++)
                    {
                        if (main[i] == correct[i])
                        {
                            run++;
                        }
                    }

                    //change win to true based on run value
                    if (run == main.Length)
                    {
                        win = true;
                        Console.WriteLine("You Won");
                        Console.WriteLine("Word was: " + word);
                    }
                }
            }
            //Make sure that the lives are not 0
            if (lives < 0)
            {
                win = false;
            }

            return win;
        }
    }
}
