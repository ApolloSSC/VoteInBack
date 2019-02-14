using VoteIn.BL.Exceptions;
using VoteIn.BL.Interfaces.Calculateurs;
using VoteIn.Utils;

namespace VoteIn.BL.Calculateurs
{
    public class CalculatorFactory : ICalculateurFactory
    {
        #region Public Methods
        /// <summary>
        /// Gets the calculator.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="VoteInException">Unknown voting type</exception>
        public ICalculator GetCalculator(string type)
        {
            switch(type)
            {
                case Constantes.VOTING_PROCESS_MAJ: return new CalculatorMajoritaryVotingProcess();
                case Constantes.JUG_MAJ: return new CalculatorMajoritaryJudgment();
                case Constantes.VOTE_ALTER: return new CalculatorAlternativeVote();
                case Constantes.CONDOR_RANDOM: return new CalculatorCondorcetRandomise();
                default:throw new VoteInException("Unknown voting type");
            }
        }
        #endregion
    }
}