using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NFluent;
using TechTalk.SpecFlow;
using VoteIn.Model.Business.ResultModels;
using VoteIn.Model.Models;

namespace VoteIn.BL.BDD.Steps
{
    [Binding]
    public class JugementMajoritaireSteps
    {
        public ResultatMajoritaryJudgmentModel Resultat
        {
            get { return (ResultatMajoritaryJudgmentModel) ScenarioContext.Current["resultat"]; }
            set
            {
                ScenarioContext.Current["resultat"] = value;
            }
        }

        [Given(@"Une liste de scrutin")]
        public void GivenUneListeDeScrutin(Table table)
        {
            foreach (var row in table.Rows)
            {
                var scrutin = new VotingProcess()
                {
                    Id = StepHelpers.GetNextId(),
                    Name = row["VotingProcess"],
                    VotingProcessMode = new VotingProcessMode
                    {
                        Choice = new List<Choice>(),
                        Code = "jugement-majoritaire"
                    }
                };
                StepHelpers.AddScrutin(scrutin);
            }
        }

        [Given(@"les choix suivants")]
        public void GivenLesChoixSuivants(Table table)
        {
            foreach (var scrutin in StepHelpers.GetAllScrutins())
            {
                foreach (var row in table.Rows)
                {
                    scrutin.VotingProcessMode.Choice.Add(new Choice
                    {
                        Id = StepHelpers.GetNextId(),
                        Name = row["Nom"],
                        Value = int.Parse(row["Valeur"])
                    });
                }
            }
        }

        [Given(@"les votes des electeurs pour le ""(.*)""")]
        public void GivenLesVotesDesElecteurs(string nomScrutin, Table table)
        {
            var scrutin = StepHelpers.GetScrutin(nomScrutin);

            foreach (var v in table.Rows.GroupBy(r => r["Option"]))
            {

                var optionScrutin = scrutin.VotingProcessOption.First(o => o.Option.Name == v.Key);
                optionScrutin.Act = new List<Act>();

                var choice = scrutin.VotingProcessMode.Choice.ToDictionary(c => c.Name);
                var options = scrutin.VotingProcessOption.Select(o => o.Option).ToDictionary(c => c.Name);

                foreach (var row in v)
                {
                    var acte = new Act()
                    {
                        Id = StepHelpers.GetNextId(),
                        IdChoice = choice[row["Choice"]].Id,
                        IdVotingProcessOption = options[row["Option"]].Id,
                    };

                    optionScrutin.Act.Add(acte);
                }
            };
        }

        [When(@"je clôture le ""(.*)""")]
        public void WhenJeClotureLe(string votingProcessName)
        {
            var scrutin = StepHelpers.GetScrutin(votingProcessName);
            try
            {
                StepHelpers.CloreScrutin(scrutin.Guid);
                Resultat = StepHelpers.GetResultatScrutin(scrutin.Guid) as ResultatMajoritaryJudgmentModel;
            }
            catch (Exception e)
            {
                ScenarioContext.Current["ExceptionMessage"] = e.Message;
            }
        }

        [Then(@"j'obtiens la médiane pour chaque candidat")]
        public void ThenJObtiensLaMedianePourChaqueCandidat(Table table)
        {

            foreach (var row in table.Rows)
            {
                var option = row["Option"];

                var resultatIndividuel = Resultat.IndividualResults.FirstOrDefault(ri => ri.Option.Name == option);

                Check.That(resultatIndividuel.Median.Name).IsEqualTo(row["Mediane"]);
                Check.That(resultatIndividuel.PercentageScoreInfMedian).IsEqualTo(Convert.ToDecimal(row["Pourcentage inferieur"]));
                Check.That(resultatIndividuel.PercentageScoreSupMedian).IsEqualTo(Convert.ToDecimal(row["Pourcentage superieur"]));
            }
        }

        [Then(@"le détail")]
        public void ThenLeDetail(Table table)
        {
            foreach (var row in table.Rows)
            {
                var option = row["Option"];
                var choice = row["Choice"];
                var expectedNombreDeVotes = int.Parse(row["NombreDeVote"]);

                var resultatIndividuel = Resultat.IndividualResults.FirstOrDefault(ri => ri.Option.Name == option);
                var score = resultatIndividuel.Scores.FirstOrDefault(s => s.Choices.Name == choice);

                Check.That(score.Votes).IsEqualTo(expectedNombreDeVotes);
            }
        }

        [Then(@"le nom du vainqueur est '(.*)'")]
        public void ThenLeNomDuVainqueurEst(string nomCandidatVainqueur)
        {
            Check.That(Resultat.Winner.Name)
                .IsEqualTo(nomCandidatVainqueur);
        }

        [Then(@"on a pas de vainqueur")]
        public void ThenOnAPasDeVainqueur()
        {
            Check.That(Resultat.IsValidResult).IsFalse();
            Check.That(Resultat.Winner).IsNull();
        }

        [Then(@"on obtiens le message ""(.*)""")]
        public void ThenOnObtiensLeMessage(string exceptionMessage)
        {
            Check.That(ScenarioContext.Current["ExceptionMessage"]).IsEqualTo(exceptionMessage);
        }

        [Then(@"le resultat est enregistré en base de données")]
        public void ThenLeResultatEstEnregistreEnBaseDeDonnees(Table table)
        {
            Result resultat = ScenarioContext.Current["ResultatBdd"] as Result;

            foreach (var row in table.Rows)
            {
                var scrutin = StepHelpers.GetScrutin(row["VotingProcess"]);
                Check.That(resultat.IdVotingProcess).IsEqualTo(scrutin.Id);
                Check.That(resultat.NbVoters).IsEqualTo(Convert.ToInt32(row["Nombre votants"]));

                if (row["Valide"].ToLower() == "oui")
                {
                    Check.That(resultat.IsValid).IsTrue();
                }
                else
                {
                    Check.That(resultat.IsValid).IsFalse();
                }

                if (row["Vainqueur"].ToLower() != "aucun")
                {
                    var vainqueur = scrutin.VotingProcessOption.Select(os => os.Option).First(o => o.Name == row["Vainqueur"]);
                    Check.That(resultat.IdWinningOption).IsEqualTo(vainqueur.Id);
                }
                else
                {
                    Check.That(resultat.IdWinningOption).IsNull();
                }

                var json = JsonConvert.SerializeObject(Resultat.IndividualResults);
                Check.That((resultat.ScoreDetail)).IsEqualTo(json);
            }
        }
    }
}
