using VoteIn.Model.Business.ResultModels;

namespace VoteIn.BL.Interfaces.Services
{
    public interface IScrutinMajoritaireService
    {
        /// <summary>
        /// Calculers the resultat.
        /// </summary>
        /// <param name="scrutinId">The scrutin identifier.</param>
        /// <returns></returns>
        ResultatMajorityVotingProcessModel CalculerResultat(int scrutinId);
    }
}