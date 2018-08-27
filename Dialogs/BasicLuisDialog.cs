using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Chronic;
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

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Greeting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
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
            await context.PostAsync($"Hello");
        }

        [LuisIntent("Öffnungszeiten")]
        public async Task ÖffnungszeitenIntent(IDialogContext context, LuisResult result)
        {
            var test = result.Entities.First(x => x.Type.Equals("Stadt", StringComparison.OrdinalIgnoreCase));
            string answer;
            
            if (test.Entity.Equals("Zürich", StringComparison.OrdinalIgnoreCase))
            {
                answer =
                    "Die Öffnungszeiten für die Filliale in Zürich sind Montag bis Freitag 8 Uhr - 18 Uhr, Samstags 8 Uhr - 17 Uhr";
            }
            else if (test.Entity.Equals("Luzern", StringComparison.OrdinalIgnoreCase))
            {
                answer =
                    "Die Öffnungszeiten für die Filliale in Luzern sind Montag bis Freitag 9 Uhr - 19 Uhr, Samstags 9 Uhr - 18 Uhr";
            }
            else
            {
                answer = "In der von Ihnen angegebenen Stadt befindet sich noch keine unserer Fillialen";
            }

            await context.PostAsync($"Stadt: {test}, Antwort:{answer}");
        }
    }
}