using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerInfo0.Models
{
    public class UserGridModel
    {
        public string SpotLetter { get; set; }
        public int SpotNum { get; set; }

        public GridSegStatus Status { get; set; } = GridSegStatus.Blank;
        
    }
}
