using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace LuisBot.Dialogs
{
    [Serializable]
    public class NewsletterDialog : IDialog<object>
    {
        private static bool _subscribedToNewsletter;
        private int _count;
        private bool _finished;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var answer = await result;
            var message = answer.Text;

            if (string.IsNullOrWhiteSpace(message))
            {
                await context.PostAsync("Bitte geben Sie etwas ein");
                context.Wait(MessageReceivedAsync);
            }

            switch (_count)
            {
                case 0:
                    _count++;
                    if (_subscribedToNewsletter)
                    {
                        await context.PostAsync("Möchten Sie sich vom Newsletter abmelden?");
                    }
                    else
                    {
                        await context.PostAsync("Möchten Sie sich am Newsletter anmelden?");
                    }

                    break;
                case 1:
                    if (_subscribedToNewsletter && message.Equals("ja", StringComparison.OrdinalIgnoreCase))
                    {
                        _subscribedToNewsletter = false;
                        await context.PostAsync("Sie wurden vom Newsletter abgemeldet");
                        _finished = true;
                    }
                    else if (!_subscribedToNewsletter && message.Equals("ja", StringComparison.OrdinalIgnoreCase))
                    {
                        await context.PostAsync("Geben Sie bitte Ihre E-Mail Adresse ein");
                        _count++;
                    }
                    else if (message.Equals("nein", StringComparison.OrdinalIgnoreCase))
                    {
                        await context.PostAsync("Ich habe nichts gemacht");
                        _finished = true;
                    }
                    else
                    {
                        await context.PostAsync("Sorry ich habe Sie nicht verstanden");
                    }

                    break;
                case 2:
                    await context.PostAsync("Geben Sie bitte Ihren Namen ein");
                    _count++;
                    break;
                case 3:
                    await context.PostAsync($"Herzlich willkommen, {message}. Sie haben den Newsletter abonniert");
                    _subscribedToNewsletter = true;
                    _finished = true;
                    break;
            }

            if (!_finished)
            {
                context.Wait(MessageReceivedAsync);
            }
            else
            {
                context.EndConversation("");
            }
        }
    }
}