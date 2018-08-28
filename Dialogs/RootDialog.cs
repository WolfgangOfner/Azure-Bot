using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

//namespace LuisBot.Dialogs
//{
//    [Serializable]
//    public class RootDialog : IDialog<object>
//    {
//        public Task StartAsync(IDialogContext context)
//        {
//            var message = context.MakeMessage();
//            var attachment = GetHeroCard();
//            message.Attachments.Add(attachment);
//            await context.PostAsync(message);

//            // Show the list of plan  
//            context.Wait(this.ShowAnnuvalConferenceTicket);
//        }

//        public virtual async Task ShowAnnuvalConferenceTicket(IDialogContext context, IAwaitable<IMessageActivity> activity)
//        {
//            var message = await activity;

//            PromptDialog.Choice(
//                context: context,
//                resume: ChoiceReceivedAsync,
//                options: (IEnumerable<AnnuvalConferencePass>)Enum.GetValues(typeof(AnnuvalConferencePass)),
//                prompt: "Hi. Please Select Annuval Conference 2018 Pass :",
//                retry: "Selected plan not avilabel . Please try again.",
//                promptStyle: PromptStyle.Auto
//            );
//        }

//        public virtual async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<AnnuvalConferencePass> activity)
//        {
//            AnnuvalConferencePass response = await activity;
//            context.Call<object>(new AnnualPlanDialog(response.ToString()), ChildDialogComplete);

//        }

//        public virtual async Task ChildDialogComplete(IDialogContext context, IAwaitable<object> response)
//        {
//            await context.PostAsync("Thanks for select C# Corner bot for Annual Conference 2018 Registrion .");
//            context.Done(this);
//        }
//    }

//    public enum AnnuvalConferencePass
//    {
//        EarlyBird,
//        Regular,
//        DelegatePass,
//        CareerandJobAdvice,
//    }
//}