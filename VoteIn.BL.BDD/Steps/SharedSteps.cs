using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using VoteIn.Model.Models;

namespace VoteIn.BL.BDD.Steps
{
    [Binding]
    public class SharedSteps
    {
        [StepArgumentTransformation(@"\[(.*)\]")]
        public string[] StringListTransform(string list) => list.Split(',').Select(s => s.Trim()).ToArray();

        [Given(@"les options suivantes pour le scrutin ""(.*)""")]
        public void GivenLesOptionsSuivantesDuScrutin(string votingProcessName, Table table)
        {
            var votingProcess = StepHelpers.GetScrutin(votingProcessName);
            votingProcess.VotingProcessOption = table.Rows.Select(row => CreateOptionScrutin(row["Nom"])).ToList();
        }

        private static VotingProcessOption CreateOptionScrutin(string optionName)
            => new VotingProcessOption
            {
                Id = StepHelpers.GetNextId(),
                Option = new Option()
                {
                    Id = StepHelpers.GetNextId(),
                    Name = optionName
                },
                Act = new List<Act>()
            };
    }
}
