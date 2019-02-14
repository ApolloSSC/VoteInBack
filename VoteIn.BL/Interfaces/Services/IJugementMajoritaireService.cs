using VoteIn.Model.Business.ResultModels;

namespace VoteIn.BL.Interfaces.Services
{
    public interface IJugementMajoritaireService
    {
        /// <summary>
        /// Calculers the resultat.
        /// </summary>
        /// <param name="scrutinId">The scrutin identifier.</param>
        /// <returns></returns>
        ResultatMajoritaryJudgmentModel CalculerResultat(int scrutinId);
    }
}