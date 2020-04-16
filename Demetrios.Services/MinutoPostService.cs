using System;
using System.Linq;
using System.Threading.Tasks;
using Demetrios.Models;
using Demetrios.Repositories.Interfaces;
using Demetrios.Services.Interfaces;

namespace Demetrios.Services
{
    public class MinutoPostService : IMinutoPostService
    {
        private readonly IMinutoPostRepository _repository;

        public MinutoPostService(IMinutoPostRepository repository)
        {
            _repository = repository;
        }

        public async Task<MinutoPost> Create(MinutoPost MinutoPost)
        {
            MinutoPost.DataAlteracao = DateTime.UtcNow;

            var success = await _repository.Create(MinutoPost);

            if (success)
                return MinutoPost;
            else
                return null;
        }

        public async Task<MinutoPost> Update(MinutoPost MinutoPost)
        {
            MinutoPost.DataAlteracao = DateTime.UtcNow;

            var success = await _repository.Update(MinutoPost);

            if (success)
                return MinutoPost;
            else
                return null;
        }

        public MinutoPost Get(string MinutoPostId)
        {
            var result = _repository.Get(MinutoPostId);

            return result;
        }

        public IOrderedQueryable<MinutoPost> GetAll(int? pageNumber, int? pageSize)
        {
            var result = _repository.GetAll(pageNumber, pageSize);

            return result;
        }

        public IOrderedQueryable<MinutoPost> GetAllByUserAccountId(string userAccountId)
        {
            var result = _repository.GetAllByUserAccountId(userAccountId);

            return result;
        }

        public async Task<bool> Delete(string MinutoPostId)
        {
            var success = await _repository.Delete(MinutoPostId);

            return success;
        }
    }
}
