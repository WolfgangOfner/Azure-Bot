using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LuisBot.Dialogs;
using LuisBot.Helper;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"],
            ConfigurationManager.AppSettings["LuisAPIKey"],
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
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
            await context.PostAsync("Let's see...I know a good joke...");

            await context.Forward(new JokeDialog(), null, context.Activity, CancellationToken.None);
        }

        [LuisIntent("�ffnungszeiten")]
        public async Task �ffnungszeitenIntent(IDialogContext context, LuisResult result)
        {
            var city = result.Entities.First(x => x.Type.Equals("Stadt", StringComparison.OrdinalIgnoreCase));
            string answer;

            if (city.Entity.Equals("Z�rich", StringComparison.OrdinalIgnoreCase))
            {
                answer = "Die �ffnungszeiten f�r die Filliale in Z�rich sind Montag bis Freitag 8 Uhr - 18 Uhr, Samstags 8 Uhr - 17 Uhr. \n Das Wetter ist regnerisch, bringen Sie einen Regenschirm";
            }
            else if (city.Entity.Equals("Luzern", StringComparison.OrdinalIgnoreCase))
            {
                answer = "Die �ffnungszeiten f�r die Filliale in Luzern sind Montag bis Freitag 9 Uhr - 19 Uhr, Samstags 9 Uhr - 18 Uhr";
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
            var gender = Category.Women;

            if (result.Entities[0] != null)
            {
                switch (result.Entities[0].Entity.ToLower())
                {
                    case "m�nner":
                        gender = Category.Men;
                        break;
                    case "frauen":
                        gender = Category.Women;
                        break;
                }
            }

            await context.Forward(new CarouselCardsDialog(gender), null, context.Activity, CancellationToken.None);
        }

        [LuisIntent("Neuheiten")]
        public async Task NeuheitenIntent(IDialogContext context, LuisResult result)
        {
            var gender = Category.Women;

            if (result.Entities[0] != null)
            {
                switch (result.Entities[0].Entity.ToLower())
                {
                    case "m�nner":
                        gender = Category.Men;
                        break;
                    case "frauen":
                        gender = Category.Women;
                        break;
                }
            }

            await context.Forward(new CarouselCardsDialog(gender, true), null, context.Activity, CancellationToken.None);
        }

        [LuisIntent("")]
        public async Task UnknownInputIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Ich bin noch im Training von verstehe Sie leider nicht.");
        }
    }
}