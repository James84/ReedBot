//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;
//using Microsoft.Bot.Builder.Dialogs;
//using Microsoft.Bot.Connector;

//namespace First_Bot.Dialogs
//{
//    public class LookingForDialog : IDialog<object>
//    {
//        public Task StartAsync(IDialogContext context)
//        {
//            /*ar keywords = incomingMessage.Text.ToLowerInvariant().Replace("i'm looking for ", "").Replace("jobs", "").Trim();

//            await SetConversationData(incomingMessage, "keywords", keywords);

//            reply = incomingMessage.CreateReply("Is your location Leeds?");
//            await SetConversationData(incomingMessage, "location", "leeds");*/
//        }

//        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
//        {
//            var incomingMessage = await argument;

//            PromptDialog.Text(context, AfterResetAsync, );

//            var keywords = incomingMessage.Text.ToLowerInvariant().Replace("i'm looking for ", "").Replace("jobs", "").Trim();

//            await context.PostAsync("Is your location Leeds?");
//            context.Wait(MessageReceivedAsync);
//        }


//        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
//        {
//            await context.PostAsync("Is your location Leeds?");

//            context.Wait(MessageReceivedAsync);
//        }
//    }
//}