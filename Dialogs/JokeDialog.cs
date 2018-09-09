using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Microsoft.Bot.Sample.LuisBot
{
    [Serializable]
    public class JokeDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
           context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            // Confirmation that we're in the JokeDialog, forwarded from the LUIS dialog
            var response = "What time does the duck wake up? At the quack of dawn!";

           await  context.PostAsync(response);
        }
    }
}