using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace LuisBot.Dialogs
{
    [Serializable]
    public class Mp3Dialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            var replyMessage = context.MakeMessage();

            var attachment = GetInternetMP3Attachment();
            replyMessage.Text = "MP3 aus dem Internet";

            replyMessage.Attachments = new List<Attachment> {attachment};

            JsonConvert.SerializeObject(context.PostAsync(replyMessage));

            return Task.CompletedTask;
        }

        public static Attachment GetInternetMP3Attachment()
        {
            return new Attachment
            {
                ContentType = "audio/mpeg3",
                ContentUrl =
                    "http://video.ch9.ms/ch9/f979/40088849-aa88-45d4-93d5-6d1a6a17f979/TestingBotFramework.mp3",
                Name = "Testing the Bot Framework Mp3"
            };
        }
    }
}