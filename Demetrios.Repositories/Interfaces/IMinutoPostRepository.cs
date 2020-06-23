using System.Linq;
using System.Threading.Tasks;
using Demetrios.Models;

namespace Demetrios.Repositories.Interfaces
{
    public interface IMinutoPostRepository
    {
        Task<bool> Create(MinutoPost MinutoPost);

        Task<bool> Update(MinutoPost MinutoPost);

        MinutoPost Get(string MinutoPostId);

        IOrderedQueryable<MinutoPost> GetAll(int? pageNumber, int? pageSize);

        IOrderedQueryable<MinutoPost> GetAllByUserAccountId(string userAccountId);

        Task<bool> Delete(string MinutoPostId);

        Task<bool> GetMinutoPostsAndCreate();

    }
}
