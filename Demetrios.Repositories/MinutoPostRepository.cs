using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Demetrios.Context;
using Demetrios.Models;
using Demetrios.Repositories.Interfaces;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

            var existingMinutoPost = Get(MinutoPost.id);

            if (existingMinutoPost != null)
            {
                existingMinutoPost.link = MinutoPost.link;
                existingMinutoPost.description = MinutoPost.description;
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
                                .Where(x => x.id == MinutoPostId)
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
                                .Where(x => x.id == userAccountId)
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

        public async Task<bool> GetMinutoPostsAndCreate()
        {
            var result = XDocument.Load("https://www.minutoseguros.com.br/blog/feed/")
                .Descendants("item")
                .Select(i => new
                {
                    Title = (string)i.Element("title"),
                    Description = (string)i.Element("description"),
                    Link = (string)i.Element("link"),
                    Category = i.Descendants("category").ToList(),
                    Encoded = (string)i.Element("{http://purl.org/rss/1.0/modules/content/}encoded"),
                })
                .ToList();

            var success = false;

            for (int i = 0; i < result.Count(); i++)
            {
                string aux = RetiraArtigosPreposicoes(result[i].Encoded);

                List<PrincipaisPalavras> listaCategorias = new List<PrincipaisPalavras>();
                for (int o = 0; o < result[i].Category.Count(); o++)
                {
                    PrincipaisPalavras categoria = new PrincipaisPalavras();
                    categoria.descricao = result[i].Category[o].ToString().Replace("<category><![CDATA[","").Replace("]]></category>", "").ToLower();
                    categoria.ocorrencias = aux.IndexOf(categoria.descricao.ToLower()) == -1 ? 0 : Regex.Matches(aux, categoria.descricao.ToLower()).Count;
                    listaCategorias.Add(categoria);
                }
                
                MinutoPost minutoPost = new MinutoPost();
                minutoPost.title = result[i].Title;
                minutoPost.description = result[i].Description;
                minutoPost.link = result[i].Link;
                minutoPost.description = aux;
                minutoPost.quantidade = aux.Count(k => k.ToString() == " ") + 1;
                minutoPost.pPalavras = listaCategorias;
                _databaseContext.MinutoPosts.Add(minutoPost);
            }

            var numberOfItemsCreated = await _databaseContext.SaveChangesAsync();

            if (numberOfItemsCreated == 1)
                success = true;

            return success;
        }

        public string RetiraArtigosPreposicoes(string a)
        {
            return a.ToLower()
                //a.Replace("A ", " ").Replace(" A ", " ").Replace(" a "," ")
                //.Replace("As ", " ").Replace(" As ", " ").Replace(" as ", " ")
                //.Replace("O ", " ").Replace(" O ", " ").Replace(" o ", " ")
                //.Replace("Os ", " ").Replace(" Os ", " ").Replace(" os ", " ")
                //.Replace("Um ", " ").Replace(" Um ", " ").Replace(" um ", " ")
                //.Replace("Uma ", " ").Replace(" Uma ", " ").Replace(" uma ", " ")
                //.Replace("Uns ", " ").Replace(" Uns ", " ").Replace(" uns ", " ")
                //.Replace("Umas ", " ").Replace(" Umas ", " ").Replace(" umas ", " ")

                //.Replace("Ao ", " ").Replace(" Ao ", " ").Replace(" ao ", " ")
                //.Replace("Aos ", " ").Replace(" Aos ", " ").Replace(" aos ", " ")
                //.Replace("À ", " ").Replace(" À ", " ").Replace(" à ", " ")
                //.Replace("Às ", " ").Replace(" Às ", " ").Replace(" às ", " ")

                //.Replace("De ", " ").Replace(" De ", " ").Replace(" de ", " ")
                //.Replace("Do ", " ").Replace(" Do ", " ").Replace(" do ", " ")
                //.Replace("Dos ", " ").Replace(" Dos ", " ").Replace(" dos ", " ")
                //.Replace("Da ", " ").Replace(" Da ", " ").Replace(" da ", " ")
                //.Replace("Das ", " ").Replace(" Das ", " ").Replace(" das ", " ")
                //.Replace("Dum ", " ").Replace(" Dum ", " ").Replace(" dum ", " ")
                //.Replace("Duns ", " ").Replace(" Duns ", " ").Replace(" duns ", " ")
                //.Replace("Duma ", " ").Replace(" Duma ", " ").Replace(" duma ", " ")
                //.Replace("Dumas ", " ").Replace(" Dumas ", " ").Replace(" dumas ", " ")

                //.Replace("Em ", " ").Replace(" Em ", " ").Replace(" em ", " ")
                //.Replace("No ", " ").Replace(" No ", " ").Replace(" no ", " ")
                //.Replace("Nos ", " ").Replace(" Nos ", " ").Replace(" nos ", " ")
                //.Replace("Na ", " ").Replace(" Na ", " ").Replace(" na ", " ")
                //.Replace("Nas ", " ").Replace(" Nas ", " ").Replace(" nas ", " ")
                //.Replace("Num ", " ").Replace(" Num ", " ").Replace(" num ", " ")
                //.Replace("Nuns ", " ").Replace(" Nuns ", " ").Replace(" nuns ", " ")
                //.Replace("Numa ", " ").Replace(" Numa ", " ").Replace(" numa ", " ")
                //.Replace("Numas ", " ").Replace(" Numas ", " ").Replace(" numas ", " ")

                //.Replace("Por ", " ").Replace(" Por ", " ").Replace(" por ", " ")
                //.Replace("Per ", " ").Replace(" Per ", " ").Replace(" per ", " ")
                //.Replace("Pelo ", " ").Replace(" Pelo ", " ").Replace(" pelo ", " ")
                //.Replace("Pelos ", " ").Replace(" Pelos ", " ").Replace(" pelos ", " ")
                //.Replace("Pela ", " ").Replace(" Pela ", " ").Replace(" pela ", " ")
                //.Replace("Pelas ", " ").Replace(" Pelas ", " ").Replace(" pelas ", " ")

                ;
        }
    }
}