using System.Linq;
using System.Threading.Tasks;
using Demetrios.Models;

namespace Demetrios.Services.Interfaces
{
    public interface IMinutoPostService
    {
        Task<MinutoPost> Create(MinutoPost MinutoPost);
   
        Task<MinutoPost> Update(MinutoPost MinutoPost);

        MinutoPost Get(string MinutoPostId);

        IOrderedQueryable<MinutoPost> GetAll(int? pageNumber, int? pageSize);

        IOrderedQueryable<MinutoPost> GetAllByUserAccountId(string userAccountId);

        Task<bool> Delete(string MinutoPostId);

        Task<bool> GetMinutoPostsAndCreate();
    }
}
