using System.Collections.Generic;
using System.Linq;
using VoteIn.Model.Models;
using VoteIn.Utils;

namespace VoteIn.DAL
{
    public static class DbInitializer
    {
        /// <summary>
        /// Seeds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public static void Seed(VoteInContext context)
        {
            if (!context.VotingProcessMode.Any())
            {
                context.VotingProcessMode.AddRange(
                    new List<VotingProcessMode>
                    {
                        new VotingProcessMode
                        {
                            Code = Constantes.JUG_MAJ,
                            Numeric = false,
                            Choice = new List<Choice>
                            {
                                new Choice
                                {
                                    Name = Constantes.JUG_MAJ_EXCEL,
                                    Value = 6
                                },
                                new Choice
                                {
                                    Name = Constantes.JUG_MAJ_TB,
                                    Value = 5
                                },
                                new Choice
                                {
                                    Name = Constantes.JUG_MAJ_B,
                                    Value = 4
                                },
                                new Choice
                                {
                                    Name = Constantes.JUG_MAJ_AB,
                                    Value = 3
                                },
                                new Choice
                                {
                                    Name = Constantes.JUG_MAJ_CORR,
                                    Value = 2
                                },
                                new Choice
                                {
                                    Name = Constantes.JUG_MAJ_PASS,
                                    Value = 1
                                },
                                new Choice
                                {
                                    Name = Constantes.JUG_MAJ_AREJ,
                                    Value = 0
                                },
                            }
                        },
                        new VotingProcessMode
                        {
                            Code = Constantes.VOTING_PROCESS_MAJ,
                            Numeric = false,
                            Choice = new List<Choice>
                            {
                                new Choice
                                {
                                    Name = Constantes.VOTING_PROCESS_MAJ_CHOOSEN,
                                    Value = 1
                                },
                                new Choice
                                {
                                    Name = Constantes.VOTING_PROCESS_MAJ_REJECTED,
                                    Value = 0
                                }
                            }
                        },
                        new VotingProcessMode
                        {
                            Code = Constantes.VOTE_ALTER,
                            Numeric = true
                        }
                    }
                );
            }

            context.SaveChanges();
        }
    }
}
