using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LuisBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        static string username;
        int count = 0;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as Activity;

            if (count > 0)
                username = message.Text;

            if (string.IsNullOrEmpty(username))
            {
                await context.PostAsync("what is your good name");
                count++;
            }
            else
            {
                await context.PostAsync($"Hello {username} , How may help You");

            }

            context.Wait(MessageReceivedAsync);
        }
    }
    }