using System.Collections.Generic;
using System.Linq;
using NFluent;
using TechTalk.SpecFlow;
using VoteIn.Model.Business.ResultModels;
using VoteIn.Model.Models;

namespace VoteIn.BL.BDD.Steps
{
    [Binding]
    public class ScrutinMajoritaireSteps
    {
        public ResultatMajorityVotingProcessModel Resultat
        {
            get => ScenarioContext.Current["resultat"] as ResultatMajorityVotingProcessModel;
            set => ScenarioContext.Current["resultat"] = value;
        }

        private readonly static Choice Choisi = new Choice
        {
            Id = 8,
            IdVotingProcessMode = 4,
            Name = "Choisi",
            Value = 1
        };

        [Given(@"Un nouveau scrutin majoritaire ""(.*)""")]
        public void GivenUnNouveauScrutinMajoritaire(string nomScrutin)
        {
            StepHelpers.AddScrutin(new VotingProcess
            {
                Id = StepHelpers.GetNextId(),
                Name = nomScrutin,
                VotingProcessMode = new VotingProcessMode
                {
                    Code = "scrutin-majoritaire",
                    Choice = new[] { Choisi }
                },
                Suffrage = new List<Suffrage>()
            });
        }

        [Given(@"on est au second tour du scrutin ""(.*)""")]
        public void GivenOnEstAuSecondTourDuScrutin(string nomScrutin)
        {
            var scrutin = StepHelpers.GetScrutin(nomScrutin);
            scrutin.IdPreviousVotingProcess = StepHelpers.GetNextId();
        }

        [Given(@"un electeur vote ""(.*)"" pour le scrutin majoritaire ""(.*)""")]
        public void GivenUnElecteurVotePourLeScrutinMajoritaire(string vote, string nomScrutin)
        {
            var scrutin = StepHelpers.GetScrutin(nomScrutin);
            var optionScrutin = scrutin.VotingProcessOption.Single(os => os.Option.Name == vote);

            AddVoteToOption(scrutin, optionScrutin);
        }

        [Given(@"un electeur vote blanc pour le scrutin majoritaire ""(.*)""")]
        public void GivenUnElecteurVoteBlancPourLeScrutinMajoritaire(string nomScrutin)
        {
            var scrutin = StepHelpers.GetScrutin(nomScrutin);
            var optionVoteBlanc = scrutin.VotingProcessOption.SingleOrDefault(os => os.Option == null);
            if (optionVoteBlanc == null)
            {
                optionVoteBlanc = new VotingProcessOption
                {
                    Id = StepHelpers.GetNextId(),
                    Act = new List<Act>()
                };
                scrutin.VotingProcessOption.Add(optionVoteBlanc);
            }

            AddVoteToOption(scrutin, optionVoteBlanc);
        }

        private static void AddVoteToOption(VotingProcess scrutin, VotingProcessOption optionScrutin)
        {
            var acte = new Act
            {
                Id = StepHelpers.GetNextId(),
                VotingProcessOption = optionScrutin,
                Choice = Choisi,
                Value = Choisi.Value
            };
            var suffrage = new Suffrage
            {
                Id = StepHelpers.GetNextId(),
                Act = new[] { acte },
                VotingProcess = scrutin
            };

            optionScrutin.Act.Add(acte);
            scrutin.Suffrage.Add(suffrage);
        }

        [When(@"je clôture le scrutin majoritaire ""(.*)""")]
        public void WhenJeClotureLeScrutinMajoritaire(string nomScrutin)
        {
            var scrutin = StepHelpers.GetScrutin(nomScrutin);
            StepHelpers.CloreScrutin(scrutin.Guid);
            Resultat = StepHelpers.GetResultatScrutin(scrutin.Guid) as ResultatMajorityVotingProcessModel;
        }

        [Then(@"""(.*)"" est désigné comme vainqueur")]
        public void ThenEstDesigneCommeVainqueur(string vainqueur)
        {
            Check.That(Resultat.Winner).IsNotNull();
            Check.That(Resultat.Winner.Name).IsEqualTo(vainqueur);
        }

        [Then(@"j'obtiens le résultat suivant")]
        public void ThenJObtiensLeResultatSuivant(Table table)
        {
            foreach (var row in table.Rows)
            {
                var option = row["Option"];
                var nombreDeVote = int.Parse(row["Nombre de vote"]);
                var pourcentage = decimal.Parse(row["pourcentage"]);

                var resultatOption = Resultat.IndividualResults.Single(ri => ri.Option.Name == option);

                Check.That(resultatOption.Percentage).IsEqualTo(pourcentage);
                Check.That(resultatOption.Votes).IsEqualTo(nombreDeVote);
            }
        }

        [Then(@"il n'y a pas de vainqueur")]
        public void ThenIlNYAPasDeVainqueur()
        {
            Check.That(Resultat.Winner).IsNull();
        }

        [Then(@"le résultat est valide")]
        public void ThenLeResultatEstValide()
        {
            Check.That(Resultat.IsValidResult).IsTrue();
        }

        [Then(@"le résultat n'est pas valide")]
        public void ThenLeResultatNEstPasValide()
        {
            Check.That(Resultat.IsValidResult).IsFalse();
        }
    }
}
