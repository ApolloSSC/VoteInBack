using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using VoteIn.Bot.Services;

namespace VoteIn.Bot.Dialogs
{
    public enum TypesScrutin
    {
        [Description("Jugement majoritaire")]
        JugementMajoritaire,
        [Description("Vote alternatif")]
        VoteAlternatif,
        [Description("Scrutin majoritaire")]
        ScrutinMajoritaire
    };

    [Serializable]
    public class PollCreator
    {
        [Prompt("Saisissez le nom de l'élection")]
        public string Nom;
        [Prompt("Choisissez le type de scrutin {||}")]
        public TypesScrutin? TypeScrutin;
        [Prompt("Saisissez la liste de vos candidats séparés par des virgules")]
        public string Candidats;

        public static IForm<PollCreator> BuildForm()
        {


            OnCompletionAsyncDelegate<PollCreator> createPoll = async (context, state) =>
            {
                try
                {
                    var apiService = new ApiService();
                    var scrutin = await apiService.CreatePoll(state.Nom, state.TypeScrutin, state.Candidats, context.Activity.From.Name);
                    List<CardAction> cardButtons = new List<CardAction>();

                    CardAction plButton = new CardAction()
                    {
                        Value = $"https://votein.net/poll/{scrutin.Id}",
                        Type = "openUrl",
                        Title = "Voter"
                    };

                    cardButtons.Add(plButton);

                    HeroCard plCard = new HeroCard()
                    {
                        Title = $"Le scrutin {scrutin.Nom} a été créé",
                        //Subtitle = $"{scrutin.ModeScrutin.Nom}",
                        Buttons = cardButtons
                    };
                    var message = context.MakeMessage();
                    message.Attachments.Add(plCard.ToAttachment());
                    await context.PostAsync(message);
                }
                catch (Exception ex)
                {
                    await context.PostAsync("Erreur lors de la création du scrutin.");
                }

            };

            return new FormBuilder<PollCreator>()
                    .Message("Bienvenue sur la création d'un scrutin")
                    .Field(nameof(Nom))
                    .Field(nameof(TypeScrutin))
                    .Field(nameof(Candidats))
                    .Confirm(async (state) =>
                    {
                        return new PromptAttribute($"Confirmez vous la création du scrutin {state.Nom} de type {state.TypeScrutin.Value} avec les candidats: {state.Candidats}?");
                    })
                    .OnCompletion(createPoll)
                    .Build();
        }
    };
}