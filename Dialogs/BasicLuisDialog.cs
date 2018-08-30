using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LuisBot.Dialogs;
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
            await context.Forward(new InternetImageDialog(), null, context.Activity, CancellationToken.None);
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
        public async Task MP3DialogIntent(IDialogContext context, LuisResult result)
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

            //context.MakeMessage().Attachments.Add(new Attachment()
            // {
            //     ContentUrl = "https://upload.wikimedia.org/wikipedia/en/a/a6/Bender_Rodriguez.png",
            //     ContentType = "image/png",
            //     Name = "Bender_Rodriguez.png"
            // });

            // await context.PostAsync(context.Activity.AsMessageActivity());
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

            await context.Forward(new JokeDialog(), ResumeAfterJokeDialog, context.Activity, CancellationToken.None);
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

        private async Task ResumeAfterJokeDialog(IDialogContext context, IAwaitable<object> result)
        {
            //PromptDialog.Text(context, WaitForJokeAnswer, "Again?");
        }

        private async Task WaitForJokeAnswer(IDialogContext context, IAwaitable<string> result)
        {
            var test = await result;

            if (test.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                await context.PostAsync("yes");
            }
            else
            {
                await context.PostAsync("no");
            }

            context.Wait(MessageReceived);
        }
    }
}