using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace LuisBot.Dialogs
{
    [Serializable]
    public class AttachmentDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            var replyMessage = context.MakeMessage();

            var attachment = GetinternetMP3Attachment();
            replyMessage.Text = "Attach Image from Internet";

            replyMessage.Attachments = new List<Attachment> {attachment};

            JsonConvert.SerializeObject(context.PostAsync(replyMessage));

            return Task.CompletedTask;
        }

        private static Attachment GetLocalAttachment()
        {
            var imagePath = HttpContext.Current.Server.MapPath("~/images/demo.gif");

            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            return new Attachment
            {
                Name = "demo.gif",
                ContentType = "image/gif",
                ContentUrl = $"data:image/gif;base64,{imageData}"
            };
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

        public static Attachment GetinternetFileAttachment()
        {
            return new Attachment
            {
                ContentType = "application/pdf",
                ContentUrl =
                    "https://qconlondon.com/london-2017/system/files/presentation-slides/microsoft_bot_framework_best_practices.pdf",
                Name = "Microsoft Bot Framework Best Practices"
            };
        }

        public static Attachment GetLocalFileAttachment()
        {
            var pdfPath = HttpContext.Current.Server.MapPath("~/File/BotFramework.pdf");
            Attachment attachment = new Attachment();
            attachment.ContentType = "application/pdf";
            attachment.ContentUrl = pdfPath;
            attachment.Name = "Local Microsoft Bot Framework Best Practices";
            return attachment;
        }

        public static Attachment GetinternetVideoAttachment()
        {

            Attachment attachment = new Attachment();
            attachment = new VideoCard("Build a great conversationalist", "Bot Demo Video",
                    "Build a great conversationalist",
                    media: new[] {new MediaUrl(@"https://bot-framework.azureedge.net/videos/skype-hero-050517-sm.mp4")})
                .ToAttachment();
            return attachment;
        }

        public static Attachment GetinternetYoutubeAttachment()
        {
            return new Attachment
            {
                ContentType = "video/mp4",
                ContentUrl = "https://youtu.be/RaNDktMQVWI"
            };
        }

        public static Attachment GetinternetMP3Attachment()
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