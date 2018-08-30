using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LuisBot.Dialogs
{
    [Serializable]
    public class LocalFilesDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
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

            return new Attachment
            {
                ContentType = "application/pdf",
                ContentUrl = pdfPath,
                Name = "Local Microsoft Bot Framework Best Practices"
            };
        }
    }
}