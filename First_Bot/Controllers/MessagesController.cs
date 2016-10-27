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
            Controllers.API.Search.GetJobs(incomingMessage.Text);
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
                            CreateAttachment("http://www.reed.co.uk/jobs", "reed.co.uk", "http://www.reed.co.uk/resources/images/campaign-2015/header-logo.png")
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

        private static Attachment CreateAttachment(string buttonLink = "", string buttonTitle = "", string imageUrl = "")
        {
            var plCard = new HeroCard();

            if (!string.IsNullOrEmpty(imageUrl))
            {
                plCard.Images = new List<CardImage> { new CardImage(imageUrl)};
            }

            if (!string.IsNullOrEmpty(buttonLink))
            {
                var plButton = new CardAction
                {
                    Value = buttonLink,
                    Type = "openUrl",
                    Title = buttonTitle
                };
                plCard.Buttons = new List<CardAction> {plButton};
            }

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
                var reply = message.CreateReply("Hello welcome to reed.co.uk. How can we help?");
                reply.Attachments = new List<Attachment>
                {
                    CreateAttachment(buttonLink: "http://www.reed.co.uk/jobs", buttonTitle: "Search", imageUrl: "")
                };

                return reply;
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