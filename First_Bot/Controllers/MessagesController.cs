using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI.WebControls;
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
                                CardActionHelpers.BuildHeroCard(new List<CardAction>
                                                                {
                                                                    CardActionHelpers.BuildCardAction("openUrl", 
                                                                                                      "see my jobs", 
                                                                                                      $"http://www.reed.co.uk/jobs?keywords={keywords}&location={location}")
                                                                }, 
                                subTitle: $"We have {searchData.TotalResults} jobs for {keywords} jobs in {location}.")
                                .ToAttachment()
                            };
                        }
                        else
                        {
                            reply = incomingMessage.CreateReply("Either keywords or location are empty. Please try again.");
                        }
                    }
                    else if (incomingMessage.Text.ToLowerInvariant() == "get started")
                    {
                        reply = incomingMessage.CreateReply("What job are you looking for?");
                    }
                    else
                    {
                        reply = incomingMessage.CreateReply("I'm sorry I do not understand. What are you looking for?");

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

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                var reply = message.CreateReply("");
                reply.Attachments = new List<Attachment>
                {
                    CardActionHelpers.BuildTumbnailCard(new List<CardAction>
                    {
                        CardActionHelpers.BuildCardAction("imBack", "Get started", "Get started")
                    },
                    new List<CardImage>
                    {
                        CardActionHelpers.BuildCardImage("Jimmy Job Bot", url: "http://www.reed.co.uk//upload/images/jimmy_job_bot_blue.jpg")
                    },
                    subTitle: "Welcome to job bot, how can I help?")
                    .ToAttachment()
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