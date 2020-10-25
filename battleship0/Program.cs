using System;
using System.Collections.Generic;
using PlayerInfo0;
using PlayerInfo0.Models;

namespace battleship0
{
    class Program
    {
        static void Main(string[] args)
        {
            Intro();
            UserModel player1 = CreatePlayer("Player 1");
            UserModel player2 = CreatePlayer("Player 2");
            UserModel winner = null;

            do
            {
                RecordPlayerShot(player1, player2);
               

                bool continueGame = gLogic.NoPlayerShipsLeft(player2);

                if (!continueGame)
                {
                    (player1, player2) = (player2, player1);
                }
                else
                {
                    winner = player1;
                }

            } while (winner == null);

            IdentifyWinner(winner);
        }

        private static void IdentifyWinner(UserModel winner)
        {
            Console.WriteLine($"{winner.username} is the winner.");
        }

        private static void RecordPlayerShot(UserModel player1, UserModel player2)
        {
            int hitCounter = 0;
            bool isValidShot = false;
            int row = 0;
            string column = "";
            
            do
            {
                string shot = AskForShot();
                (column, row) = gLogic.SplitIntoRowsAndColumns(shot);
                isValidShot = gLogic.ValidateShot(player1, column, row);
                

                if (!isValidShot)
                {
                    Console.WriteLine("Invalid shot; please enter a new set of coordinates.");
                }
                
            } while (!isValidShot);

            bool isAHit = gLogic.IdentifyShotRes(player2, row, column);

            gLogic.MarkShotRes(player1, row, column, isAHit);

        }

        private static string AskForShot()
        {
            Console.Write("Please enter shot coordinate selection: ");
            string output = Console.ReadLine();

            return output;
        }

        public static void Intro()
        {
            Console.WriteLine("Welcome to Battleship Lite Base!");
            Console.WriteLine();
        }

        public static UserModel CreatePlayer(string playerTitle)
        {
            UserModel output = new UserModel();

            Console.WriteLine($"Player info for {playerTitle}: ");

            AskForUsername(output);

            gLogic.GridInitialize(output);

            PlaceShips(output);
            
            Console.Clear();

            return output;
        }


        public static UserModel AskForUsername(UserModel output)
        {
            string store = Console.ReadLine();
            bool isValidName = true;
            var nope = "";
            foreach (char s in store)
            {
                if (!(Char.IsLetter(s)) && (s.Equals(" ")))
                {
                    Console.WriteLine("Spaces and alphabetical characters only please");
                    isValidName = false;
                }
            }

            if (isValidName)
            {
                output.username = store;
            }
            else
            {
                output.username = nope;
            }

            return output;
        }

        public static void PlaceShips(UserModel player)
        {
            do
            {
                Console.WriteLine($"Please specify a location for ship num { player.ShipLocations.Count }. You may enter two figures, one a letter from a-i, the other a number from 1-9 in any order.");
                string location = Console.ReadLine();

                bool isViableLoc = true;
               
               
                isViableLoc = gLogic.PlaceShip(player, location);

                if (!isViableLoc)
                {
                    Console.WriteLine("Not a valid location. Please try again.");
                }

            } while (player.ShipLocations.Count < 5);
        }
        
    }
}
