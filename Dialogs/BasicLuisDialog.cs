using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LuisBot.Dialogs;
using LuisBot.Helper;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        private Activity _activity;
        public BasicLuisDialog(Activity activity) : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"],
            ConfigurationManager.AppSettings["LuisAPIKey"],
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
            _activity = activity;
        }

        [LuisIntent("InternetDatei")]
        public async Task InternetDateiIntent(IDialogContext context, LuisResult result)
        {
            await context.Forward(new InternetFileAttachmentDialog(), null, context.Activity, CancellationToken.None);
        }

        [LuisIntent("InternetBild")]
        public async Task InternetBildIntent(IDialogContext context, LuisResult result)
        {
            await context.Forward(new InternetImageDialog(), null, context.Activity, CancellationToken.None);
        }

        [LuisIntent("InternetVideo")]
        public async Task InternetVideoIntent(IDialogContext context, LuisResult result)
        {
            await context.Forward(new InternetVideoDialog(), null, context.Activity, CancellationToken.None);
        }

        [LuisIntent("Mp3")]
        public async Task Mp3DialogIntent(IDialogContext context, LuisResult result)
        {
            await context.Forward(new Mp3Dialog(), null, context.Activity, CancellationToken.None);
        }

        [LuisIntent("Youtube")]
        public async Task YoutubeIntent(IDialogContext context, LuisResult result)
        {
            await context.Forward(new YoutubeDialog(), null, context.Activity, CancellationToken.None);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Greeting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await ShowLuisResult(context, result);
        }

        [LuisIntent("CardAction")]
        public async Task CardActionIntent(IDialogContext context, LuisResult result)
        {
            var reply = _activity.CreateReply("I have colors in mind, but need your help to choose the best one.");
            reply.Type = ActivityTypes.Message;
            reply.TextFormat = TextFormatTypes.Plain;

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){ Title = "Blue", Type=ActionTypes.ImBack, Value="Blue" },
                    new CardAction(){ Title = "Red", Type=ActionTypes.ImBack, Value="Red" },
                    new CardAction(){ Title = "Green", Type=ActionTypes.ImBack, Value="Green" }
                }
            };
        }

        [LuisIntent("Newsletter")]
        public async Task NewsletterIntent(IDialogContext context, LuisResult result)
        {
            await context.Forward(new NewsletterDialog(), null, context.Activity, CancellationToken.None);
        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Joke ")]
        public async Task JokeIntent(IDialogContext context, LuisResult result)
        {
            //await context.PostAsync($"Tow mice chewing on a film roll. One of them goes: \"I think the book was better.\"... Sorry I am not funny");
            var message = $"Let's see...I know a good joke...";

            await context.PostAsync(message);

            await context.Forward(new JokeDialog(), null, context.Activity, CancellationToken.None);
        }

        [LuisIntent("Öffnungszeiten")]
        public async Task ÖffnungszeitenIntent(IDialogContext context, LuisResult result)
        {
            var city = result.Entities.First(x => x.Type.Equals("Stadt", StringComparison.OrdinalIgnoreCase));
            string answer;

            if (city.Entity.Equals("Zürich", StringComparison.OrdinalIgnoreCase))
            {
                answer =
                    "Die Öffnungszeiten für die Filliale in Zürich sind Montag bis Freitag 8 Uhr - 18 Uhr, Samstags 8 Uhr - 17 Uhr";
            }
            else if (city.Entity.Equals("Luzern", StringComparison.OrdinalIgnoreCase))
            {
                answer =
                    "Die Öffnungszeiten für die Filliale in Luzern sind Montag bis Freitag 9 Uhr - 19 Uhr, Samstags 9 Uhr - 18 Uhr";
            }
            else
            {
                answer = "In der von Ihnen angegebenen Stadt befindet sich noch keine unserer Fillialen";
            }

            await context.PostAsync($"{answer}");
        }

        [LuisIntent("Bestseller")]
        public async Task BeststellerIntent(IDialogContext context, LuisResult result)
        {
            await context.Forward(new CarouselCardsDialog(Category.Men), null, context.Activity, CancellationToken.None);
        }
    }
}