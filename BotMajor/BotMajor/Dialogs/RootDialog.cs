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
    public class RootDialog : IDialog<object>
    {

        public static string number = "";
        public static string chatId = "defa";
        private const string CarInfo = "Информация о машине";

        private const string CallMaster = "Вызвать мастера";



        Verification verification = new Verification();
        Buttons buttons = new Buttons();


        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }
        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case CarInfo:
                        context.Call(new CarDialog(), this.ResumeAfterOptionDialog);
                        break;

                    case CallMaster:
                        context.Call(new MasterDialog(), this.ResumeAfterOptionDialog);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Упс! Слишком много попыток :( Но не волнуйтесь, я обрабатываю это исключение, и вы можете попробовать еще раз!");

                context.Wait(this.MessageReceivedAsync);
            }
        }
        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Ошибка с сообщением: {ex.Message}");
            }
            finally
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }
        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { CarInfo, CallMaster }, "Вы в главном меню. Что вы хотите?", "Вариант не найден. Пожалуйста, попробуйте еще раз", 3);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            if (Querys.FirstUser(chatId))
            {
                await context.PostAsync("Дайте доступ к Вашему номеру телефона");

                await context.Forward(new RegisterDialog(), this.ResumeAfterRegistrationDialog, chatId, CancellationToken.None);

            }
            else
            {
                var activity = await result as Activity;
                if (activity.Text.ToLower().Contains("help") || activity.Text.ToLower().Contains("support") || activity.Text.ToLower().Contains("problem"))
                {
                    //await context.Forward(new SupportDialog(), this.ResumeAfterSupportDialog, activity, CancellationToken.None);
                }
                else
                {
                    this.ShowOptions(context);
                }
            }

        }
        private async Task ResumeAfterRegistrationDialog(IDialogContext context, IAwaitable<object> result)
        {
            var ticketNumber = await result;

            //await context.PostAsync($"Thanks for contacting our support team. Your ticket number is {ticketNumber}.");
            context.Wait(this.MessageReceivedAsync);
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