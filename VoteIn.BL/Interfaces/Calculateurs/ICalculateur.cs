using VoteIn.Model.Business;
using VoteIn.Model.Business.ResultModels;

namespace VoteIn.BL.Interfaces.Calculateurs
{
    public interface ICalculator
    {
        /// <summary>
        /// Calculates the result.
        /// </summary>
        /// <param name="scrutin">The scrutin.</param>
        /// <returns></returns>
        IResultatModel CalculateResult(VotingProcessModel scrutin);
    }
}