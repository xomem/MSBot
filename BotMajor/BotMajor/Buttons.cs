using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace BotMajor
{
    [Serializable]
    public class Buttons
    {


        List<string> menu = new List<string>() { "Информация о машине", "Вызвать мастера" };


        //var choseCarButtons = new List<CardAction>();

        //for (int i = 0; i < cars.Count; i++)
        //{
        //    choseCarButtons.Add(new CardAction
        //    {
        //        Title = cars[i],
        //        Value = cars[i] + " ,Index + " + i,
        //    });
        //}



        // return our reply to the user
        //await context.PostAsync($"You sent {activity.Text} which was {length} characters");




        public Activity MainMenuAct(Activity act)
        {
            var mainMenu = act.CreateReply("Главное меню");
            mainMenu.Type = ActivityTypes.Message;
            mainMenu.TextFormat = TextFormatTypes.Plain;


            var choseCarButtons = menu.Select((i, v) => new CardAction { Title = i, Value =Convert.ToString(i)}).ToList();

            mainMenu.Attachments.Add(
                        new HeroCard
                        {
                            Buttons = choseCarButtons
                        }.ToAttachment()
                    );
            return mainMenu;
        }
        public Activity CarInfo(Activity act, List<Cars> cars)
        {
            var mainMenu = act.CreateReply("Выберете автомобиль");
            mainMenu.Type = ActivityTypes.Message;
            mainMenu.TextFormat = TextFormatTypes.Plain;

            var choseCarButtons = cars.Select(car => new CardAction { Title = car.ToString(), Value = car.carId.ToString() }).ToList();

            mainMenu.Attachments.Add(
                new HeroCard
                {
                    Buttons = choseCarButtons
                }.ToAttachment()

            );
            return mainMenu;
        }
    }
}