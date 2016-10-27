using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using First_Bot.Controllers.API;
using Microsoft.Bot.Connector;

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
            //SearchResultsWrapper data = Search.GetJobs(incomingMessage.Text);
            //Console.WriteLine(data);

            try
            {
                Activity reply;

                if (incomingMessage.Type == ActivityTypes.Message)
                {
                    //                    Bot: Welcome did you know (intro)
                    //Bot: Hi how can i help you
                    //Alex: I’m looking for < junior UX jobs >
                    //Bot: Current Location ? Leeds ? (show map)
//                    Alex: No, < London >
//                    Bot: Show results

                    if (incomingMessage.Text.ToLowerInvariant().Contains("i'm looking for"))
                    {
                        var keywords = incomingMessage.Text.ToLowerInvariant().Replace("i'm looking for ", "").Replace("jobs", "").Trim();

                        await SetConversationData(incomingMessage, "keywords", keywords);

                        reply = incomingMessage.CreateReply("Is your location Leeds?");
                        await SetConversationData(incomingMessage, "location", "leeds");
                    }
                    else if (incomingMessage.Text.ToLowerInvariant().Contains("no it's"))
                    {
                        var location = incomingMessage.Text.ToLowerInvariant().Replace("no it's", "");

                        await SetConversationData(incomingMessage, "location", location);

                        var conversationData = await GetConversationData(incomingMessage);

                        var keywords = conversationData.GetProperty<string>("keywords");

                        if (!string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(keywords))
                        {
                            var searchData = Search.GetJobs(keywords, location);

                            reply = incomingMessage.CreateReply();

                            reply.Attachments = new List<Attachment>
                            {
                                CreateAttachment($"http://www.reed.co.uk/jobs?keywords={keywords}&location={location}", 
                                                 "see my jobs", subTitle: $"We have {searchData.TotalResults} jobs for {keywords} jobs in {location}.")
                            };
                        }
                        else
                        {
                            reply = incomingMessage.CreateReply("Either keywords or location are empty. Please try again.");
                        }
                    }
                    else
                    {
                        reply = incomingMessage.CreateReply("Hi, how can I help you?");
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
                var reply = incomingMessage.CreateReply($"I'm sorry there has been an error. {ex.Message}");

                if (reply != null)
                {
                    var connector = new ConnectorClient(new Uri(incomingMessage.ServiceUrl));

                    await connector.Conversations.ReplyToActivityAsync(reply);
                }

                var response = Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
        }

        private static async Task<BotData> GetConversationData(Activity incomingMessage)
        {
            var stateClient = incomingMessage.GetStateClient();

            return await stateClient
                .BotState
                .GetConversationDataAsync(incomingMessage.ChannelId, incomingMessage.From.Id);
        }

        private static async Task SetConversationData(Activity incomingMessage, string key, string value)
        {
            var stateClient = incomingMessage.GetStateClient();

            var conversationData = await GetConversationData(incomingMessage);

            conversationData.SetProperty(key, value);

            stateClient.BotState.SetConversationData(incomingMessage.ChannelId,
                incomingMessage.From.Id,
                conversationData);
        }

        private static Attachment CreateAttachment(string buttonLink = "", string buttonTitle = "", string imageUrl = "", string subTitle = "")
        {
            var plCard = new HeroCard();

            if (!string.IsNullOrEmpty(subTitle))
                plCard.Subtitle = subTitle;

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
                var reply = message.CreateReply("Welcome to job bot, how can I help?");

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

/*
 
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
                            incomingMessage.CreateReply("Let's search!");
                        
                        reply.Attachments = new List<Attachment>
                        {
                            CreateAttachment("http://www.reed.co.uk/jobs", "reed.co.uk", "http://www.reed.co.uk/resources/images/campaign-2015/header-logo.png")
                        };
                    }
                    else
                    {
                        reply = incomingMessage.CreateReply("I'm sorry ReedBot does not understand. Please try again");
                    }     
*/
