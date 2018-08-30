using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace LuisBot.Dialogs
{
    [Serializable]
    public class InternetImageDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            var replyMessage = context.MakeMessage();

            var attachment = GetInternetAttachment();
            replyMessage.Text = "Bild aus dem Internet";

            replyMessage.Attachments = new List<Attachment> {attachment};

            JsonConvert.SerializeObject(context.PostAsync(replyMessage));

            return Task.CompletedTask;
        }

        private static Attachment GetInternetAttachment()
        {
            return new Attachment
            {
                Name = "architecture-resize.png",
                ContentType = "image/png",
                ContentUrl = "https://docs.microsoft.com/en-us/bot-framework/media/how-it-works/architecture-resize.png"
            };
        }
    }
}