using VoteIn.Model.Business;
using VoteIn.Model.Business.ResultModels;

namespace VoteIn.BL.Interfaces.Calculateurs
{
    public interface ICalculateurFactory
    {
        /// <summary>
        /// Gets the calculator.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        ICalculator GetCalculator(string type);
    }
}