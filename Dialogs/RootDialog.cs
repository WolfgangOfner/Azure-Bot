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
        static bool subscribedToNewsletter;
        int count = 0;
        private bool finished = false;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as string;

            

            if (count == 0)
            {
                count++;
                if (subscribedToNewsletter)
                {
                    await context.PostAsync("Möchten Sie sich vom Newsletter abmelden?");
                }
                else
                {
                    await context.PostAsync("Möchten Sie sich am Newsletter anmelden?");
                }
            }
            else if (count == 1)
            {
                if (subscribedToNewsletter && message.Equals("ja", StringComparison.OrdinalIgnoreCase))
                {
                    subscribedToNewsletter = false;
                    await context.PostAsync("Sie wurden vom Newsletter abgemeldet");
                }
                else if (!subscribedToNewsletter && message.Equals("ja", StringComparison.OrdinalIgnoreCase))
                {
                    await context.PostAsync("Geben Sie bitte Ihre E-Mail Adresse ein");
                    count++;
                }
                else if (message.Equals("nein", StringComparison.OrdinalIgnoreCase))
                {
                    await context.PostAsync("Ich habe nichts gemacht");
                    finished = true;
                }
                else
                {
                    await context.PostAsync($"Sorry ich habe Sie nicht verstanden {message}");
                }
            }
            else if (count == 2)
            {
                await context.PostAsync("Sie haben den Newsletter abonniert");
                subscribedToNewsletter = true;
                finished = true;
            }

            if (!finished)
            {
                context.Wait(MessageReceivedAsync);
            }
        }

        //private async Task ResumeAfterJokeDialog(IDialogContext context, IAwaitable<object> result)
        //{
        //    //PromptDialog.Text(context, WaitForJokeAnswer, "Again?");
        //}

        //private async Task WaitForJokeAnswer(IDialogContext context, IAwaitable<string> result)
        //{
        //    string test = await result;

        //    if (test.Equals("yes", StringComparison.OrdinalIgnoreCase))
        //    {
        //        await context.PostAsync("yes");
        //    }
        //    else
        //    {
        //        await context.PostAsync("no");
        //    }

        //    context.Wait(MessageReceived);

        //}
    }
    }