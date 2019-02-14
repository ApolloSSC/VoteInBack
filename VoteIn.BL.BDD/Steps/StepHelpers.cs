using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Moq;
using TechTalk.SpecFlow;
using UnityAutoMoq;
using VoteIn.BL.Calculateurs;
using VoteIn.BL.Interfaces;
using VoteIn.BL.Interfaces.Calculateurs;
using VoteIn.BL.Interfaces.Mapper;
using VoteIn.BL.Interfaces.Services;
using VoteIn.BL.Mapper;
using VoteIn.BL.Services;
using VoteIn.Model.Business.ResultModels;
using VoteIn.Model.Models;

namespace VoteIn.BL.BDD.Steps
{
    public static class StepHelpers
    {
        private static int idGenerator = 0;
        public static int GetNextId() => idGenerator++;

        private static T GetOrAddInCurrentContext<T>(string key)
            where T : new()
        {
            if (!ScenarioContext.Current.TryGetValue(key, out var result))
            {
                result = new T();
                ScenarioContext.Current[key] = result;
            }

            return (T) result;
        }

        private static Dictionary<string, VotingProcess> ScrutinsParNom => GetOrAddInCurrentContext<Dictionary<string, VotingProcess>>("scrutins");

        public static IEnumerable<VotingProcess> GetAllScrutins() => ScrutinsParNom.Values;

        public static VotingProcess GetScrutin(string nomScrutin) => ScrutinsParNom[nomScrutin];

        public static VotingProcess AddScrutin(VotingProcess scrutin) => ScrutinsParNom[scrutin.Name] = scrutin;

        public static void CloreScrutin(Guid guid) => GetScrutinService().CloreScrutin(guid);

        public static IResultatModel GetResultatScrutin(Guid guid) => GetScrutinService().GetResultat(guid);

        private static IVotingProcessService GetScrutinService()
        {
            var container = new UnityAutoMoqContainer();
            container.RegisterType<IVotingProcessService, ScrutinService>();
            container.RegisterType<ICalculateurFactory, CalculatorFactory>();
            container.RegisterType<IMapperService, MapperService>();

            var repoMock = container.GetMock<IVotingProcessRepository>();
            repoMock.Setup(m => m.GetVotingProcess())
                    .Returns(ScrutinsParNom.Values.AsQueryable());

            var resultatRepoMock = container.GetMock<IRepository<Result>>();
            resultatRepoMock.Setup(m => m.Add(It.IsAny<Result>()))
                            .Callback((Result resultat) => ScenarioContext.Current["ResultatBdd"] = resultat);

            resultatRepoMock.Setup(r => r.GetAll("", null))
                            .Returns(() => new[] { ScenarioContext.Current["ResultatBdd"] as Result }.AsQueryable());

            return container.Resolve<IVotingProcessService>();
        }
    }
}
