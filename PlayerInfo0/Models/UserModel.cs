using System;
using System.Collections.Generic;

namespace PlayerInfo0.Models
{
    public class UserModel
    {
        public string username { get; set; } 

        public List<UserGridModel> ShipLocations { get; set; } = new List<UserGridModel>();

        public List<UserGridModel> ShotLocations { get; set; } = new List<UserGridModel>();

        public List<UserGridModel> GridLocations { get; set; } = new List<UserGridModel>();
    }
}
