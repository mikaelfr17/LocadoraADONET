using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using DataAccessLayer;
using Entities.ResultSets;
using Entities.Enums;

namespace BusinessLogicalLayer
{
    public class FilmeBLL : IEntityCRUD<Filme>, IFilmeService
    {
        public Response Delete(int id)
        {
            Response response = new Response();

            using (LocacaoDbContext ctx = new LocacaoDbContext())
            {
                try
                {
                    Filme f = new Filme();
                    f.ID = id;
                    ctx.Entry<Filme>(f).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    response.Erros.Add("Não foi possível deletar o cadastro do filme");
                    response.Sucesso = false;
                    return response;
                }
                response.Sucesso = true;
                return response;
            }
        }

        public DataResponse<Filme> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public DataResponse<Filme> GetData()
        {
            DataResponse<Filme> response = new DataResponse<Filme>();

            using (LocacaoDbContext ctx = new LocacaoDbContext())
            {
                try
                {
                    ctx.Filmes.OrderBy(f => f.Nome);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    response.Erros.Add("Não foi encontrar o filme");
                    response.Sucesso = false;
                    return response;
                }
                response.Sucesso = true;
                return response;
            }
        }

        public DataResponse<FilmeResultSet> GetFilmes()
        {
            throw new NotImplementedException();
        }

        public DataResponse<FilmeResultSet> GetFilmesByClassificacao(Classificacao classificacao)
        {
            DataResponse<FilmeResultSet> response = new DataResponse<FilmeResultSet>();

            using (LocacaoDbContext ctx = new LocacaoDbContext())
            {
                try
                {
                    ctx.Filmes.Find(classificacao);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    response.Erros.Add("Não foi encontrar o cadastro do cliente");
                    response.Sucesso = false;
                    return response;
                }
                response.Sucesso = true;
                return response;
            }
        }

        public DataResponse<FilmeResultSet> GetFilmesByGenero(int genero)
        {
            throw new NotImplementedException();
        }

        public DataResponse<FilmeResultSet> GetFilmesByName(string nome)
        {
            throw new NotImplementedException();
        }

        public Response Insert(Filme item)
        {
            Response response = Validate(item);
            if (response.Erros.Count > 0)
            {
                response.Sucesso = false;
                return response;
            }

            try
            {
                using (LocacaoDbContext ctx = new LocacaoDbContext())
                {
                    ctx.Filmes.Add(item);
                    ctx.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                response.Erros.Add("Não foi possível cadastrar o filme");
                response.Sucesso = false;
                return response;
            }

            return response;
        }

        public Response Update(Filme item)
        {
            Response response = new Response();

            using (LocacaoDbContext ctx = new LocacaoDbContext())
            {
                try
                {

                    ctx.Entry<Filme>(item).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();

                }
                catch (Exception ex)
                {
                    response.Erros.Add("Não foi possível atualizar o cadastro do filme");
                    response.Sucesso = false;
                    return response;
                }
                response.Sucesso = true;
                return response;

            }
        }

        private Response Validate(Filme item)
        {
            Response response = new Response();

            if (item.Duracao <= 10)
            {
                response.Erros.Add("Duração não pode ser menor que 10 minutos.");
            }

            if (item.DataLancamento == DateTime.MinValue
                                    ||
                item.DataLancamento > DateTime.Now)
            {
                response.Erros.Add("Data inválida.");
            }

            return response;
        }
    }
}
