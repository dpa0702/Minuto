using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Demetrios.Context;
using Demetrios.Models;
using Demetrios.Repositories.Interfaces;

namespace Demetrios.Repositories
{
    public class MinutoPostRepository : IMinutoPostRepository
    {
        private readonly IServiceScope _scope;
        private readonly MinutoPostDatabaseContext _databaseContext;

        public MinutoPostRepository(IServiceProvider services)
        {
            _scope = services.CreateScope();

            _databaseContext = _scope.ServiceProvider.GetRequiredService<MinutoPostDatabaseContext>();
        }

        public async Task<bool> Create(MinutoPost MinutoPost)
        {
            var success = false;

            _databaseContext.MinutoPosts.Add(MinutoPost);

            var numberOfItemsCreated = await _databaseContext.SaveChangesAsync();

            if (numberOfItemsCreated == 1)
                success = true;

            return success;
        }

        public async Task<bool> Update(MinutoPost MinutoPost)
        {
            var success = false;

            var existingMinutoPost = Get(MinutoPost.Id);

            if (existingMinutoPost != null)
            {
                existingMinutoPost.Nome = MinutoPost.Nome;
                existingMinutoPost.Canal = MinutoPost.Canal;
                existingMinutoPost.Valor = MinutoPost.Valor;
                existingMinutoPost.Obs = MinutoPost.Obs;
                existingMinutoPost.DataAlteracao = MinutoPost.DataAlteracao;

                _databaseContext.MinutoPosts.Attach(existingMinutoPost);

                var numberOfItemsUpdated = await _databaseContext.SaveChangesAsync();

                if (numberOfItemsUpdated == 1)
                    success = true;
            }

            return success;
        }

        public MinutoPost Get(string MinutoPostId)
        {
            var result = _databaseContext.MinutoPosts
                                .Where(x => x.Id == MinutoPostId)
                                .FirstOrDefault();

            return result;
        }

        public IOrderedQueryable<MinutoPost> GetAll(int? pageNumber, int? pageSize)
        {
            var result = _databaseContext.MinutoPosts
                                .Skip(pageNumber ?? 0)
                                .Take(pageSize ?? 10)
                                .OrderByDescending(x => x.DataAlteracao);

            return result;
        }

        public IOrderedQueryable<MinutoPost> GetAllByUserAccountId(string userAccountId)
        {
            var result = _databaseContext.MinutoPosts
                                .Where(x => x.Id == userAccountId)
                                .OrderByDescending(x => x.DataAlteracao);

            return result;
        }

        public async Task<bool> Delete(string MinutoPostId)
        {
            var success = false;

            var existingMinutoPost = Get(MinutoPostId);

            if (existingMinutoPost != null)
            {
                _databaseContext.MinutoPosts.Remove(existingMinutoPost);

                var numberOfItemsDeleted = await _databaseContext.SaveChangesAsync();

                if (numberOfItemsDeleted == 1)
                    success = true;
            }

            return success;
        }
    }
}
