using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace First_Bot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity incomingMessage)
        {
            try
            {
                Activity reply;

                if (incomingMessage.Type == ActivityTypes.Message)
                {
                    if (incomingMessage.Text.ToLowerInvariant() == "hello" || incomingMessage.Text.ToLowerInvariant() == "hi")
                    {

                        reply =
                            incomingMessage.CreateReply(
                                $"<b>Hello {incomingMessage.From.Name}</b>, this is ReedBot. How can I help?");

                        reply.TextFormat = "xml";
                    }
                    else if (incomingMessage.Text.ToLowerInvariant() == "search")
                    {

                        reply =
                            incomingMessage.CreateReply();
                        
                        reply.Attachments = new List<Attachment>
                        {
                            CreateAttachment("http://www.reed.co.uk/jobs", "http://www.reed.co.uk/resources/images/campaign-2015/header-logo.png")
                        };
                    }
                    else
                    {
                        reply = incomingMessage.CreateReply("I'm sorry ReedBot does not understand. Please try again");
                    }
                }
                else
                {
                    reply = HandleSystemMessage(incomingMessage);
                }

                if (reply != null)
                {
                    var connector = new ConnectorClient(new Uri(incomingMessage.ServiceUrl));

                    await connector.Conversations.ReplyToActivityAsync(reply);
                }

                var response = Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static Attachment CreateAttachment(string contentUrl, string imageUrl)
        {
            var cardButtons = new List<CardAction>();

            var cardImages = new List<CardImage> {new CardImage(imageUrl)};

            var plButton = new CardAction
            {
                Value = contentUrl,
                Type = "openUrl",
                Title = "reed.co.uk"
            };

            cardButtons.Add(plButton);

            var plCard = new HeroCard()
            {
                //Title = $"Click on the button to run a search",
                Subtitle = "Click on the button to run a search",
                Images = cardImages,
                Buttons = cardButtons
            };

            var plAttachment = plCard.ToAttachment();

            return plAttachment;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                return message.CreateReply($"You are typing...");
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    } 
}