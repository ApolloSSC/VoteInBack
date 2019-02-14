using VoteIn.Model.Business;
using VoteIn.Model.Business.ResultModels;
using VoteIn.Model.Models;

namespace VoteIn.BL.Interfaces.Mapper
{
    public interface IMapperService
    {
        /// <summary>
        /// Maps the scrutin to scrutin model.
        /// </summary>
        /// <param name="scrutin">The scrutin.</param>
        /// <returns></returns>
        VotingProcessModel MapScrutinToScrutinModel(VotingProcess scrutin);
        /// <summary>
        /// Maps the resultat model to resultat.
        /// </summary>
        /// <param name="scrutinModel">The scrutin model.</param>
        /// <param name="resultatModel">The resultat model.</param>
        /// <returns></returns>
        Result MapResultatModelToResultat(VotingProcessModel scrutinModel, IResultatModel resultatModel);
        /// <summary>
        /// Maps the resultat to resultat model.
        /// </summary>
        /// <param name="scrutin">The scrutin.</param>
        /// <param name="resultat">The resultat.</param>
        /// <returns></returns>
        IResultatModel MapResultatToResultatModel(VotingProcess scrutin, Result resultat);
    }
}