using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VoteIn.DAL;
using VoteIn.IntegrationTest.Shared;
using VoteIn.Model.Models;
using Xunit;

namespace VoteIn.IntegrationTest.IntegrationTests
{
    [Collection("IT")]
    public class ScrutinTest : IntegrationTestBase
    {
        [Fact]
        public async Task CreateScrutin_JugementMajoritaire_Success()
        {
            SetupDatabase();

            var scrutin = new VotingProcess()
            {
                Name = "Jugement Majoritaire",
                Description = "test jugement majoritaire",
                Public = true,
                OpeningDate = DateTime.Now,
                Author = "Julien",
                AuthorMail = "test@test.fr",
                IdVotingProcessMode = 2,
                VotingProcessOption = new Collection<VotingProcessOption>()
                    {
                        new VotingProcessOption()
                        {
                            Option = new Option() { Name= "Emmanuel Macron", Color = "#DDDDDD" }
                        },
                        new VotingProcessOption()
                        {
                            Option = new Option() { Name= "Marine Le Pen", Color = "#000055" }
                        },
                        new VotingProcessOption()
                        {
                            Option = new Option() { Name= "Philippe Douste-Blazy", Color = "#FF851B" }
                        }
                    }

            };

            scrutin = await SendJsonResponseAsync<VotingProcess>(HttpMethod.Post, $"/api/VotingProcess", scrutin);

            var choice = await SendJsonResponseAsync<IEnumerable<Choice>>(HttpMethod.Get, $"/api/Choice", null);
            choice = choice.Where(c => c.IdVotingProcessMode == scrutin.IdVotingProcessMode).ToList();

            //Vote
            var random = new Random();

            for (var i = 0; i < 100; i++)
            {
                var vote = new
                {
                    IdScrutin = scrutin.Id,
                    OptionChoice = new[]
                    {
                        new {
                            IdOption= scrutin.VotingProcessOption.ElementAt(0).IdOption,
                            IdChoice= choice.ElementAt(random.Next(0, 7)).Id,
                        },
                        new {
                            IdOption= scrutin.VotingProcessOption.ElementAt(1).IdOption,
                            IdChoice= choice.ElementAt(random.Next(0, 7)).Id,
                        },
                        new {
                            IdOption= scrutin.VotingProcessOption.ElementAt(2).IdOption,
                            IdChoice= choice.ElementAt(random.Next(0, 7)).Id,
                    },
                    },
                    _type = "VoteMajoritaryJudgment"

                };

                await SendNoResponseAsync(HttpMethod.Post, $"/api/suffrage/sendVote", vote);
            }
            
            //Closing
            await SendNoResponseAsync(HttpMethod.Post, $"/api/scrutin/{scrutin.Id}/clore", null);
        }

        [Fact]
        public async Task CreateScrutin_ScrutinMajoritaire_Success()
        {
            SetupDatabase();

            var modeScrutins = await SendJsonResponseAsync<IEnumerable<VotingProcessMode>>(HttpMethod.Get, $"/api/ModeScrutin", null);
            var scrutinMajoritaire = modeScrutins.First(m => m.Code == "scrutin-majoritaire");

            var votingProcess = new VotingProcess()
            {
                Name = "VotingProcess Majoritaire",
                Description = "test scrutin majoritaire",
                Public = true,
                OpeningDate = DateTime.Now,
                Author = "test",
                AuthorMail = "test@test.fr",
                IdVotingProcessMode = scrutinMajoritaire.Id,
                VotingProcessOption = new Collection<VotingProcessOption>()
                    {
                        new VotingProcessOption()
                        {
                            Option = new Option() { Name= "a" }
                        },
                        new VotingProcessOption()
                        {
                            Option = new Option() { Name= "b" }
                        },
                        new VotingProcessOption()
                        {
                            Option = new Option() { Name= "c" }
                        }
                    }

            };

            votingProcess = await SendJsonResponseAsync<VotingProcess>(HttpMethod.Post, $"/api/VotingProcess", votingProcess);

            var listChoice = await SendJsonResponseAsync<IEnumerable<Choice>>(HttpMethod.Get, $"/api/Choice", null);
            var choice = listChoice.First(c => c.IdVotingProcessMode == votingProcess.IdVotingProcessMode && c.Value == 1);

            //Vote
            var votes = new int[100];
            var random = new Random();

            for(var i = 0; i < votes.Length; i++)
            {
                votes[i] = random.Next(0, votes.Length);
            }

            foreach (var value in votes)
            {
                var candidat = 0;

                if (value > 66)
                {
                    candidat = 2;
                }
                else if (value > 33)
                {
                    candidat = 1;
                }

                var vote = new
                {
                    IdScrutin = votingProcess.Id,
                    IdOption = votingProcess.VotingProcessOption.ElementAt(candidat).IdOption,
                    _type = "VoteMajoritaryVotingProcess"

                };

                await SendNoResponseAsync(HttpMethod.Post, $"/api/suffrage/sendVote", vote);
            }

            //Closing
            await SendNoResponseAsync(HttpMethod.Post, $"/api/scrutin/{votingProcess.Id}/clore", null);
        }

        private static void SetupDatabase()
        {
            var context = (VoteInContext)UnitTestStartup.Instance.Services.GetService(typeof(VoteInContext));

            context.Act.RemoveRange(context.Act);
            context.VotingProcessOption.RemoveRange(context.VotingProcessOption);
            context.Option.RemoveRange(context.Option);
            context.Result.RemoveRange(context.Result);
            context.VotingProcess.RemoveRange(context.VotingProcess);

            context.SaveChanges();
        }
    }
}
