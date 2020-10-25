using System;
using System.Collections.Generic;
using System.Text;
using PlayerInfo0.Models;
using System.Text.RegularExpressions;

namespace PlayerInfo0
{
    public static class gLogic
    {
        public static UserModel GridInitialize(UserModel player)
        {
            List<string> Letters = new List<string>();

            Letters.Add("A");
            Letters.Add("B");
            Letters.Add("C");
            Letters.Add("D");
            Letters.Add("E");
            Letters.Add("F");
            Letters.Add("G");
            Letters.Add("H");
            Letters.Add("I");

            List<int> nums = new List<int>();

            nums.Add(1);
            nums.Add(2);
            nums.Add(3);
            nums.Add(4);
            nums.Add(5);
            nums.Add(6);
            nums.Add(7);
            nums.Add(8);
            nums.Add(9);

            foreach (string Letter in Letters)
            {
                foreach (int num in nums)
                {
                    AddGridLocation(player, Letter, num);
                }
            }

            return player;

        }

        public static bool NoPlayerShipsLeft(UserModel player2)
        {
            bool allSunk = false;
            int shipSunk = 0;

            foreach (UserGridModel shipLoc in player2.ShipLocations)
            {
                if (shipLoc.Status == GridSegStatus.Sunk)
                {
                    shipSunk += 1;
                }
            }

            if(shipSunk == 5)
            {
                allSunk = true;
            }

            return allSunk;
        }

        private static void AddGridLocation(UserModel user, string letter, int num)
        {
            UserGridModel loc = new UserGridModel();
            loc.SpotLetter = letter;
            loc.SpotNum = num;
            loc.Status = GridSegStatus.Blank;

            user.GridLocations.Add(loc);
        }

        public static (string column, int row) SplitIntoRowsAndColumns(string shot)
        {
            string column = ""; ;
            int row = 0;
            bool match = true;

            string pattern = "[j-z]";

            if (shot.Length == 2)
            {
                foreach (char s in shot)
                {
                    bool num = int.TryParse(s.ToString(), out int numeric);

                    if (num)
                    {
                        row = numeric;
                    }
                    else
                    {

                        Match t = Regex.Match(s.ToString(), pattern);
                        if (t.Value == "")
                        {
                            match = false;
                        }

                        if (!match)
                        {
                            column = s.ToString();
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Incorrect shot length. Two characters required.");
            }
            return (column, row);
        }

        public static bool ValidateShot(UserModel player, string column, int row)
        {
            bool valid = false;
            UserGridModel temp = new UserGridModel();

            if ((Char.IsLetter(column.ToCharArray()[0])))
            {
                if (!(row <= -1) && (row < 9))
                {
                    temp.SpotLetter = column;
                    temp.SpotNum = row;
                    if (player.ShotLocations.Count > 0) 
                    {
                        foreach (UserGridModel gridLoc in player.ShotLocations)
                        {
                            var match = Regex.Matches(gridLoc.SpotLetter, @"[a-i]");
                            if (!(match.Count == 0) && !(gridLoc.SpotNum == 0) && !(gridLoc.SpotNum > 9))
                            {
                                valid = true;
                            }
                        }
                    }
                    else
                    {
                        valid = true;
                    }
                }
            }
            return valid;
        }

        public static bool PlaceShip(UserModel player, string location)
        {

            UserGridModel temp = new UserGridModel();   
            bool output = false;

            (string letter, int num) = SplitIntoRowsAndColumns(location);

            bool isValidLocation = ValidGridLocShip(player, num, letter);

            if (isValidLocation)
            {               
                temp.SpotLetter = letter;
                temp.SpotNum = num;
                temp.Status = GridSegStatus.Ship;
                player.ShipLocations.Add(temp);
                output = true;
            }
            else
            {
                output = false;
            }

            return output;
        }

        private static bool ValidGridLocShip(UserModel player, int num, string letter)
        {
            bool output = true;

            foreach (var ship in player.ShipLocations)
            {
                if (ship.SpotLetter == letter.ToUpper() && ship.SpotNum == num)
                {
                    output = false;
                }
            }

            return output;
        }

        public static void MarkShotRes(UserModel player, int row, string column, bool isAHit)
        {
            UserGridModel temp = new UserGridModel();
            temp.SpotLetter = column;
            temp.SpotNum = row;

            player.ShotLocations.Add(temp);

            if (isAHit)
            {
                Console.WriteLine("Hit! Ship Sunk");
            }
            else
            {
                Console.WriteLine("Miss");
            }          
        }

        public static bool IdentifyShotRes(UserModel player2, int row, string column)
        {
            UserGridModel temp = new UserGridModel();

            bool valid = false;

            temp.SpotLetter = column;

            temp.SpotNum = row;

            foreach(UserGridModel s in player2.ShipLocations)
            {
                if ((s.SpotLetter == temp.SpotLetter) && (s.SpotNum == temp.SpotNum))
                {
                    s.Status = GridSegStatus.Sunk;                   
                    valid = true;
                }
                else if (s.Status != GridSegStatus.Sunk)
                {
                    s.Status = GridSegStatus.Miss;                    
                }
            }

            return valid;
        }
    }
}
