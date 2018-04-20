using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;


namespace BotMajor.Dialogs
{

    [Serializable]
    public class CarDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //await context.PostAsync("Welcome to the Hotels finder!");
            if (Querys.MoreOneCar(RootDialog.chatId))
            {
                ShowCarsOptions(context);
                //context.Call(cars, this.ResumeAfterCarFormDialog);

                //var cars = await this.GetCarsAsync(Querys.GetCarsByChatID(RootDialog.chatId).ToList());
            }
            else
            {
                //var hotelsFormDialog = FormDialog.FromForm(this.BuildCarForm, FormOptions.PromptInStart);
               await GetCarInfo(context);
                //context.Call(SelectCarFrom, this.ResumeAfterCarFormDialog);
            }

        }
        private async Task GetCarInfo(IDialogContext context)
        {
            string carNumber = StringEngine.ExtractFromBrackets(Querys.GetCarByChatId(RootDialog.chatId));
            //await context.PostAsync(optionSelected);
            await context.PostAsync("Дата ТО - " + Convert.ToString(Querys.GetTIDate(carNumber)));
            context.Done<object>(null);
        }
        private async Task OnCarOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                
                string optionSelected = await result; // одна из машин
                string carNumber = StringEngine.ExtractFromBrackets(optionSelected);
                //await context.PostAsync(optionSelected);
                await context.PostAsync("Дата ТО - " + Convert.ToString(Querys.GetTIDate(carNumber)));
                context.Done<object>(null);
                //context.Call(new RootDialog(), this.ResumeAfterOptionDialog);
                //switch (optionSelected)
                //{

                //    //case result[0]:
                //    //    context.Call(new CarDialog(), this.ResumeAfterOptionDialog);
                //    //    break;

                //    //case CallMaster:
                //    //    context.Call(new MasterDialog(), this.ResumeAfterOptionDialog);
                //    //    break;
                //}
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Упс! Слишком много попыток :( Но не волнуйтесь, я обрабатываю это исключение, и вы можете попробовать еще раз!" + ex);

                //context.Wait(this.MessageReceivedAsync);
            }
        }
        private void ShowCarsOptions(IDialogContext context)
        {
            //string[] cars = GetCarsAsync().ToString;
            //GetCarsAsync();
            //PromptDialog.Choice(context, this.OnCarOptionSelected, new List<string>() {cars.Select(car => car.ToString()) }, "Вы в главном меню. Что вы хотите", "Вариант не найден. Пожалуйста, попробуйте еще раз", 3);
            var carsList = Querys.GetCarsByChatID(RootDialog.chatId);
            PromptDialog.Choice(context, this.OnCarOptionSelected, carsList.Select(car => car.ToString()).ToList(), "Выбирете автомобиль", "Вариант не найден. Пожалуйста, попробуйте еще раз", 3);
        }


        //private async Task<IEnumerable<Cars>> GetCarsAsync()
        //{
        //    var carsList = Querys.GetCarsByChatID(RootDialog.chatId);
        //    Cars cars = new Cars();
        //    //var cars = new List<Cars>();

        //    // Filling the hotels results manually just for demo purposes
        //    for (int i = 1; i < carsList.ToList().Count; i++)
        //    {
        //        Cars car = new Cars()
        //        {


        //            //car.carId = cars.
        //            //Name = $"{searchQuery.Destination} Hotel {i}",
        //            //Location = searchQuery.Destination,
        //            //Rating = random.Next(1, 5),
        //            //NumberOfReviews = random.Next(0, 5000),
        //            //PriceStarting = random.Next(80, 450),
        //            //Image = $"https://placeholdit.imgix.net/~text?txtsize=35&txt=Hotel+{i}&w=500&h=260"
        //        };

        //        //carsList.Add(car);
        //    }

        //    //carsList.Sort((h1, h2) => h1.carMark.CompareTo(h2.carModel));

        //    return carsList;
        //}
















        //private IForm<CarInfoQuery> BuildCarForm()
        //{
        //    OnCompletionAsyncDelegate<CarInfoQuery> processHotelsSearch = async (context, state) =>
        //    {
        //        await context.PostAsync($"Ok. Searching for Hotels in {state.Destination} from {state.CheckIn.ToString("MM/dd")} to {state.CheckIn.AddDays(state.Nights).ToString("MM/dd")}...");
        //    };

        //    return new FormBuilder<CarInfoQuery>()
        //        .Field(nameof(CarInfoQuery.Destination))
        //        .Message("Looking for hotels in {Destination}...")
        //        .AddRemainingFields()
        //        .OnCompletion(processHotelsSearch)
        //        .Build();
        //}
        private async Task ResumeAfterCarOptionDialog(IDialogContext context, IAwaitable<CarInfoQuery> result)
        {
            try
            {
                var searchQuery = await result;

                //var cars = await this.GetCarsAsync();

                //await context.PostAsync($"I found in total {cars.Count()} hotels for your dates:");

                var resultMessage = context.MakeMessage();
                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();

                //foreach (var car in cars)
                //{
                //    HeroCard heroCard = new HeroCard()
                //    {
                //        Title = car.carMark + car.carModel,
                //        //Subtitle = $"{hotel.Rating} starts. {hotel.NumberOfReviews} reviews. From ${hotel.PriceStarting} per night.",
                //        //Images = new List<CardImage>()
                //        //{
                //        //    new CardImage() { Url = hotel.Image }
                //        //},
                //        //Buttons = new List<CardAction>()
                //        //{
                //        //    new CardAction()
                //        //    {
                //        //        Title = "More details",
                //        //        Type = ActionTypes.OpenUrl,
                //        //        Value = $"https://www.bing.com/search?q=hotels+in+" + HttpUtility.UrlEncode(car.Location)
                //        //    }
                //        //}
                //    };

                //    resultMessage.Attachments.Add(heroCard.ToAttachment());
                //}

                await context.PostAsync(resultMessage);
            }
            catch (FormCanceledException ex)
            {
                string reply;

                if (ex.InnerException == null)
                {
                    reply = "You have canceled the operation. Quitting from the HotelsDialog";
                }
                else
                {
                    reply = $"Oops! Something went wrong :( Technical Details: {ex.InnerException.Message}";
                }

                await context.PostAsync(reply);
            }
            finally
            {
                context.Done<object>(null);
            }
        }

        //private async Task SelectCarFrom(IDialog contex, IAwaitable<Cars> result)
        //{
        //    foreach (var car in result)
        //    {
        //        HeroCard heroCard = new HeroCard()
        //        {
        //            Title = car.carMark + car.carModel,
        //            //Subtitle = $"{hotel.Rating} starts. {hotel.NumberOfReviews} reviews. From ${hotel.PriceStarting} per night.",
        //            //Images = new List<CardImage>()
        //            //{
        //            //    new CardImage() { Url = hotel.Image }
        //            //},
        //            //Buttons = new List<CardAction>()
        //            //{
        //            //    new CardAction()
        //            //    {
        //            //        Title = "More details",
        //            //        Type = ActionTypes.OpenUrl,
        //            //        Value = $"https://www.bing.com/search?q=hotels+in+" + HttpUtility.UrlEncode(car.Location)
        //            //    }
        //            //}
        //        };

        //        resultMessage.Attachments.Add(heroCard.ToAttachment());
        //    }
        //}

        private async Task ResumeAfterCarFormDialog(IDialogContext context, IAwaitable<CarInfoQuery> result)
        {
            try
            {
                var searchQuery = await result;

                //var cars = await this.GetCarsAsync();

                //await context.PostAsync($"I found in total {cars.Count()} hotels for your dates:");

                var resultMessage = context.MakeMessage();
                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();

                //foreach (var car in cars)
                //{
                //    HeroCard heroCard = new HeroCard()
                //    {
                //        Title = car.carMark + car.carModel,
                //        //Subtitle = $"{hotel.Rating} starts. {hotel.NumberOfReviews} reviews. From ${hotel.PriceStarting} per night.",
                //        //Images = new List<CardImage>()
                //        //{
                //        //    new CardImage() { Url = hotel.Image }
                //        //},
                //        //Buttons = new List<CardAction>()
                //        //{
                //        //    new CardAction()
                //        //    {
                //        //        Title = "More details",
                //        //        Type = ActionTypes.OpenUrl,
                //        //        Value = $"https://www.bing.com/search?q=hotels+in+" + HttpUtility.UrlEncode(car.Location)
                //        //    }
                //        //}
                //    };

                //    resultMessage.Attachments.Add(heroCard.ToAttachment());
                //}

                await context.PostAsync(resultMessage);
            }
            catch (FormCanceledException ex)
            {
                string reply;

                if (ex.InnerException == null)
                {
                    reply = "You have canceled the operation. Quitting from the HotelsDialog";
                }
                else
                {
                    reply = $"Oops! Something went wrong :( Technical Details: {ex.InnerException.Message}";
                }

                await context.PostAsync(reply);
            }
            finally
            {
                context.Done<object>(null);
            }
        }
      
    }


    //private IForm<HotelsQuery> BuildCarForm()
    //{
    //    OnCompletionAsyncDelegate<HotelsQuery> processHotelsSearch = async (context, state) =>
    //    {
    //        await context.PostAsync($"Ok. Searching for Hotels in {state.Destination} from {state.CheckIn.ToString("MM/dd")} to {state.CheckIn.AddDays(state.Nights).ToString("MM/dd")}...");
    //    };

    //    return new FormBuilder<HotelsQuery>()
    //        .Field(nameof(HotelsQuery.Destination))
    //        .Message("Looking for hotels in {Destination}...")
    //        .AddRemainingFields()
    //        .OnCompletion(processHotelsSearch)
    //        .Build();
    //}
}