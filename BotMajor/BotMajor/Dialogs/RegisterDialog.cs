using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;

namespace BotMajor.Dialogs
{
    [Serializable]
    public class RegisterDialog : IDialog<object>
    {

        Verification verification = new Verification();


        public Task StartAsync(IDialogContext context)
        {
            context.Wait(Registration);

            return Task.CompletedTask;
        }
        
        
        private async Task Registration(IDialogContext context, IAwaitable<object> result)
        {

            string numberr = result.ToString();
            await context.PostAsync(numberr);
            if (verification.NumberValidation(result.ToString()))
            {
                string number = result.ToString();
                if (Querys.RegistUser(number ,RootDialog.chatId))
                {
                    await context.PostAsync("Спасибо");
                    MessagesController.userStage = MessagesController.UserStage.MainMenu;
                }
            }
            else
            {
                await context.PostAsync("Номер введен не верно");
            }
        }
    }
}













//if (activity.Text == "Информация о машине" && MessagesController.userStage == MessagesController.UserStage.MainMenu)
//{
//    MessagesController.userStage = MessagesController.UserStage.CarSelect;
//}
//string ig = ignore.FirstOrDefault(x => x == activity.Text);
//if (ig == null)
//{
//    if (!Querys.FirstUser(chatId))
//    {
//        MessagesController.userStage = MessagesController.UserStage.MainMenu;
//    }
//}

//else if (MessagesController.userStage == MessagesController.UserStage.FirstStart)
//{
//    await context.PostAsync("Дайте доступ к Вашему номеру телефона");
//    MessagesController.userStage = MessagesController.UserStage.WaitNumber;


//    context.Wait(MessageReceivedAsync);
//}
//else if (MessagesController.userStage == MessagesController.UserStage.WaitNumber)
//{

//    if (verification.NumberValidation(activity.Text))
//    {
//        number = activity.Text;
//        if (Querys.RegistUser(number, chatId))
//        {
//            await context.PostAsync("Спасибо");
//            MessagesController.userStage = MessagesController.UserStage.MainMenu;                    
//        }
//    }
//    else
//    {
//        await context.PostAsync("Номер введен не верно");
//    }

//}
//if(MessagesController.userStage == MessagesController.UserStage.MainMenu && !verification.IsDigitsOnly(activity.Text))
//{
//    await context.PostAsync(buttons.MainMenuAct(activity));
//    context.Wait(MessageReceivedAsync);
//}
//else if (MessagesController.userStage == MessagesController.UserStage.CarSelect && !verification.IsDigitsOnly(activity.Text))
//{
//    var cars = Querys.GetCarsByChatID(chatId).ToList();


//    await context.PostAsync(buttons.CarInfo(activity, cars));


//    context.Wait(MessageReceivedAsync);
//    //await context.PostAsync(Convert.ToString(MessagesController.userStage));

//}
//else if(MessagesController.userStage == MessagesController.UserStage.CarInfo)
//{
//    var info = Querys.TechnicalInspection(chatId);

//    await context.PostAsync(info);


//    context.Wait(MessageReceivedAsync);
//}
//await context.PostAsync(Convert.ToString(MessagesController.userStage));