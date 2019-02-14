using System.Threading.Tasks;

namespace VoteIn
{
    public interface ITypedHubClient
    {
        Task VoteAdded();
    }
}