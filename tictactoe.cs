using System;

namespace cstictactoe
{
    class Program
    {

        //Options: 0 = (grid size), 1 = (player count)
        static int[] oplist = options();

        //Following on from the options
        static int size = oplist[0];
        static int com = oplist[1];

        //For use in the computer algorithim
        //comrun is how many computer turns there have been
        //prev is a store of recent moves
        static int comrun = 0;
        static int[] prev = new int[2];

        //Turn and moves determine the charachters to place on the board and how may times that has occoured
        static char turn = 'x';
        static int moves = 0;

        //Tic tac to grid
        static char[,] grid = new char[size, size];

        //Determines if game is on
        static bool gameon = true;

        public static void Main(string[] args)
        {
            //Console.Clear();
            Console.WriteLine("Enter 'esc' to end game\n");
            Console.WriteLine("***");
            Console.WriteLine("\nTIC TAC TOE\n");

            //Determines what turn is the computer's
            char comturn = '-';

            //Change comturn based of the player options
            if (com == 2)
            {
                comturn = 'o';
            }
            if (com == 3)
            {
                comturn = 'x';
            }

            //Add dashes to grid
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    grid[x, y] = '-';
                }
            }

            while (moves < grid.Length && gameon == true)
            {
                Console.WriteLine("***");
                Console.WriteLine("Player " + turn + "'s turn\n");

                //Used to help check and convert inputs to positions on grid
                int sx = 0;
                int sy = 0;
                string movex = " ";
                string movey = " ";

                //Player's turn
                if (com == 1 || comturn != turn)
                {
                    Console.WriteLine("Enter x-position");
                    movex = Console.ReadLine();
                    if (movex != "esc")
                    {
                        Console.WriteLine("Enter y-position");
                        movey = Console.ReadLine();
                    }
                }
                else
                {
                    // Computer's turn
                    int[] cm = RunCom(comturn);
                    movex = cm[0].ToString();
                    movey = cm[1].ToString();
                }

                if (movex == "esc" || movey == "esc")
                {
                    Console.WriteLine("\nEnding Game ...");
                    gameon = false;
                }
                //Check if the input was an integer
                else if (!int.TryParse(movex, out sx))
                {
                    Console.WriteLine("\nImproper input, Try again\n");
                }
                else if (!int.TryParse(movey, out sy))
                {
                    Console.WriteLine("\nImproper input, Try again\n");
                }
                else if (sx >= 1 && sx <= size && sy >= 1 && sy <= size)
                {

                    //Checks if grid position is empty and places the marker
                    if (grid[sx - 1, sy - 1] == '-')
                    {
                        grid[sx - 1, sy - 1] = turn;
                        Console.Clear();
                        Console.WriteLine(PrintGrid());
                        prev[0] = sx;
                        prev[1] = sy;

                        //Checks if there has been a win, if not, switches turn
                        if (CheckWin() == true)
                        {
                            Console.WriteLine("Player " + turn + " has won!");
                            gameon = false;
                        }
                        else
                        {
                            if (turn == 'x')
                            {
                                turn = 'o';
                            }
                            else
                            {
                                turn = 'x';
                            }
                            moves++;
                        }

                    }
                    else
                    {
                        Console.WriteLine("\nImproper input, Try again\n");
                    }
                }
                else
                {
                    Console.WriteLine("\nImproper input, Try again\n");
                }
            }

        }

        static bool CheckWin()
        {
            //cor is used to check individual sets of grid size
            //def is the default marker to make sure it counts only one type of marker
            //run is a total of how many times one marker occours in a set of grid size
            char[] cor = new char[size];
            char def = ' ';
            int run = 0;

            //Column Wins
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    cor[y] = grid[x, y];
                }
                foreach (char c in cor)
                {
                    if (def == ' ')
                    {
                        if (c == 'x' || c == 'o')
                        {
                            def = c;
                            run++;
                        }
                    }
                    else if (def == c)
                    {
                        run++;
                    }

                }

                if (run == size)
                {
                    return true;
                }
                run = 0;
                def = ' ';

            }

            //Row Wins
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    cor[x] = grid[x, y];
                }
                foreach (char c in cor)
                {
                    if (def == ' ')
                    {
                        if (c == 'x' || c == 'o')
                        {
                            def = c;
                            run++;
                        }
                    }
                    else if (def == c)
                    {
                        run++;
                    }

                }

                if (run == size)
                {
                    return true;
                }
                run = 0;
                def = ' ';

            }

            //Diagonal Wins
            for (int i = 0; i < size; i++)
            {
                cor[i] = grid[i, i];
            }
            foreach (char c in cor)
            {
                if (def == ' ')
                {
                    if (c == 'x' || c == 'o')
                    {
                        def = c;
                        run++;
                    }
                }
                else if (def == c)
                {
                    run++;
                }

            }

            if (run == size)
            {
                return true;
            }
            run = 0;
            def = ' ';

            //Reverse Diagonal Wins
            for (int i = 0; i < size; i++)
            {
                cor[i] = grid[size - i - 1, i];
            }
            foreach (char c in cor)
            {
                if (def == ' ')
                {
                    if (c == 'x' || c == 'o')
                    {
                        def = c;
                        run++;
                    }
                }
                else if (def == c)
                {
                    run++;
                }

            }

            if (run == size)
            {
                return true;
            }
            run = 0;
            def = ' ';

            //No win
            return false;

        }

        //Prints grig using for loop
        static string PrintGrid()
        {
            string gridstring = "";
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    gridstring += grid[x, y] + " ";
                }
                gridstring += "\n";
            }
            return gridstring;
        }

        //Helps set the grid size and Player options
        static int[] options()
        {
            //The bool is used to repeat the loop is the wrong options are entered
            //The int list is the option list returned and put into the size and player variables
            bool whilecheck = true;
            int[] ops = new int[2];

            //Alter grid size from 1 to infinity
            Console.WriteLine("Enter Grid Size: ");
            while (whilecheck)
            {
                whilecheck = false;
                if (!int.TryParse(Console.ReadLine(), out ops[0]))
                {
                    Console.WriteLine("\nImproper input, Try again\n");
                    whilecheck = true;
                }
                else if (ops[0] < 1)
                {
                    Console.WriteLine("\nImproper input, Try again\n");
                    whilecheck = true;
                }
            }

            //Player options from 1 to 3
            //1 = no computer
            //2 = computer goes last
            //3 = computer goes first)
            Console.WriteLine("Enter play options:"); Console.WriteLine("1 = no computer, 2 = computer goes last, 3 = computer goes first ");
            whilecheck = true;
            while (whilecheck)
            {
                whilecheck = false;
                if (!int.TryParse(Console.ReadLine(), out ops[1]))
                {
                    Console.WriteLine("\nImproper input, Try again\n");
                    whilecheck = true;
                }
                else if (ops[1] < 1 || ops[1] > 3)
                {
                    Console.WriteLine("\nImproper input, Try again\n");
                    whilecheck = true;
                }
            }
            return ops;
        }

        //Algorithim function, runs when comturn is equal to turn
        static int[] RunCom(char comturn)
        {
            //commove is the set of where the computer's next move is
            //The middle decimal is the centre point of the grid, will be rounded
            int[] commove = new int[2];
            decimal middle = Convert.ToDecimal(size) / 2;

            //Move if no threats

            //Corners, then centre, then any other position(s)
            if (grid[0, 0] == '-')
            {
                commove[0] = 1;
                commove[1] = 1;
                comrun++;
            }
            else if (grid[size - 1, 0] == '-')
            {
                commove[0] = size;
                commove[1] = 1;
                comrun++;
            }
            else if (grid[size - 1, size - 1] == '-')
            {
                commove[0] = size;
                commove[1] = size;
                comrun++;
            }
            else if (grid[0, size - 1] == '-')
            {
                commove[0] = 1;
                commove[1] = size;
                comrun++;
            }
            else if (grid[Convert.ToInt32(middle) - 1, 1] == '-')
            {
                commove[0] = Convert.ToInt32(middle);
                commove[1] = Convert.ToInt32(middle);
                comrun++;
            }
            else
            {
                //Scan rest of grid for a position
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        if (grid[x, y] == '-')
                        {
                            commove[0] = x + 1;
                            commove[1] = y + 1;
                            comrun++;
                        }
                    }
                }
            }

            // Check For Checks

            //dash checks is there is a dash in the set
            //cor is used to check individual sets of grid size
            //def is the default marker to make sure it counts only one type of marker
            //run is a total of how many times one marker occours in a set of grid size
            //winning helps prioritize winning moves. If one is found, all other options immediately stop
            //valst is a store to help determine position of the empty space
            bool dash = false;
            char[] cor = new char[size];
            char def = ' ';
            int run = 0;
            bool winning = true;
            int valst = 0;

            //Column Wins
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    cor[y] = grid[x, y];
                    if (cor[y] == '-')
                    {
                        valst = y;
                    }
                }
                foreach (char c in cor)
                {
                    if (def == ' ')
                    {
                        if (c == 'x' || c == 'o')
                        {
                            def = c;
                            run++;
                        }
                    }
                    else if (def == c)
                    {
                        run++;
                    }

                    if (c == '-')
                    {
                        dash = true;
                    }

                }

                if (winning && dash && run == size - 1)
                {
                    if (def == comturn)
                    {
                        winning = false;
                    }
                    commove[0] = x + 1;
                    commove[1] = valst + 1;
                }
                run = 0;
                dash = false;
                def = ' ';

            }

            //Row Wins
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    cor[x] = grid[x, y];
                    if (cor[x] == '-')
                    {
                        valst = x;
                    }
                }
                foreach (char c in cor)
                {
                    if (def == ' ')
                    {
                        if (c == 'x' || c == 'o')
                        {
                            def = c;
                            run++;
                        }
                    }
                    else if (def == c)
                    {
                        run++;
                    }

                    if (c == '-')
                    {
                        dash = true;
                    }

                }

                if (winning && dash && run == size - 1)
                {
                    if (def == comturn)
                    {
                        winning = false;
                    }
                    commove[1] = y + 1;
                    commove[0] = valst + 1;
                }
                run = 0;
                dash = false;
                def = ' ';

            }

            //Diagonal Wins
            for (int i = 0; i < size; i++)
            {
                cor[i] = grid[i, i];
                if (cor[i] == '-')
                {
                    valst = i;
                }
            }
            foreach (char c in cor)
            {
                if (def == ' ')
                {
                    if (c == 'x' || c == 'o')
                    {
                        def = c;
                        run++;
                    }
                }
                else if (def == c)
                {
                    run++;
                }

                if (c == '-')
                {
                    dash = true;
                }

            }

            if (winning && dash && run == size - 1)
            {
                if (def == comturn)
                {
                    winning = false;
                }
                commove[1] = valst + 1;
                commove[0] = valst + 1;
            }
            run = 0;
            dash = false;
            def = ' ';

            //Reverse Diagonal Wins
            for (int i = 0; i < size; i++)
            {
                cor[i] = grid[size - i - 1, i];
                if (cor[i] == '-')
                {
                    valst = i;
                }
            }
            foreach (char c in cor)
            {
                if (def == ' ')
                {
                    if (c == 'x' || c == 'o')
                    {
                        def = c;
                        run++;
                    }
                }
                else if (def == c)
                {
                    run++;
                }

                if (c == '-')
                {
                    dash = true;
                }

            }

            if (winning && dash && run == size - 1)
            {
                if (def == comturn)
                {
                    winning = false;
                }
                commove[1] = valst + 1;
                commove[0] = size - valst;
            }
            run = 0;
            dash = false;
            def = ' ';

            //Return the computer's move
            return commove;
        }
    }
}
