using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotMajor
{
    using System;
    using Microsoft.Bot.Builder.FormFlow;

    [Serializable]
    public class CarInfoQuery
    {
        [Prompt("Please enter your {&}")]
        public string Destination { get; set; }

        [Prompt("When do you want to {&}?")]
        public DateTime CheckIn { get; set; }

        [Numeric(1, int.MaxValue)]
        [Prompt("How many {&} do you want to stay?")]
        public int Nights { get; set; }
    }
}