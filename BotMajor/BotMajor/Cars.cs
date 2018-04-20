using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotMajor
{
    public class Cars
    {
        public int carId { get; set; }
        public string carMark { get; set; }
        public string carModel { get; set; }
        public string carNumber { get; set; }

        public override string ToString()
        {
            return carMark + " " + carModel + " (" + carNumber + ")";
        }
    }
}