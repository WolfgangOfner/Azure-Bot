using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Microsoft.Bot.Sample.LuisBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        ///     POST: api/Messages
        ///     receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            // check if activity is of type message
            if (activity.GetActivityType() == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new BasicLuisDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }

            var request = Request.CreateResponse(HttpStatusCode.OK);
            return request;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            var messageType = message.GetActivityType();

            switch (messageType)
            {
                case ActivityTypes.DeleteUserData:
                    break;

                case ActivityTypes.ConversationUpdate:
                    IConversationUpdateActivity update = message;
                    var client = new ConnectorClient(new Uri(message.ServiceUrl), new MicrosoftAppCredentials());

                    if (update.MembersAdded != null && update.MembersAdded.Any())
                    {
                        foreach (var newMember in update.MembersAdded)
                        {
                            if (newMember.Id != message.Recipient.Id)
                            {
                                var reply = message.CreateReply();
                                reply.Text = "Hallo. Ich bin der PKZ Chat Bot Trainee. Wie kann ich Ihnen helfen?";
                                client.Conversations.ReplyToActivityAsync(reply);
                            }
                        }
                    }

                    break;

                case ActivityTypes.ContactRelationUpdate:
                    break;

                case ActivityTypes.Typing:
                    // Handle knowing that the user is typing
                    break;

                case ActivityTypes.Ping:
                    break;

                default:
                    break;
            }

            return null;
        }
    }
}