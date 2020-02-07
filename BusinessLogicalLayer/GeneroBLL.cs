using DataAccessLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    /// <summary>
    /// Classe responsável pelas regras de negócio 
    /// da entidade Gênero.
    /// </summary>
    public class GeneroBLL : IEntityCRUD<Genero>
    {
        public Response Insert(Genero item)
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
                    ctx.Generos.Add(item);
                    ctx.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                response.Erros.Add("Não foi possível cadastrar o gênero");
                response.Sucesso = false;
                return response;
            }

            return response;

        }
        public Response Update(Genero item)
        {
            Response response = new Response();

            using (LocacaoDbContext ctx = new LocacaoDbContext())
            {
                try
                {

                    ctx.Entry<Genero>(item).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();

                }
                catch (Exception ex)
                {
                    response.Erros.Add("Não foi possível atualizar o cadastro do gênero");
                    response.Sucesso = false;
                    return response;
                }
                response.Sucesso = true;
                return response;

            }
        }
        public Response Delete(int id)
        {
            Response response = new Response();

            using (LocacaoDbContext ctx = new LocacaoDbContext())
            {
                try
                {
                    Genero g = new Genero();
                    g.ID = id;
                    ctx.Entry<Cliente>(g).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    response.Erros.Add("Não foi possível deletar o cadastro do gênero");
                    response.Sucesso = false;
                    return response;
                }
                response.Sucesso = true;
                return response;
            }
        }

        public DataResponse<Genero> GetData()
        {
            return dal.GetData();
        }

        public DataResponse<Genero> GetByID(int id)
        {
            return dal.GetByID(id);
        }
        private Response Validate(Genero item)
        {
            Response response = new Response();
            if (string.IsNullOrWhiteSpace(item.Nome))
            {
                response.Erros.Add("O nome do gênero deve ser informado.");
            }
            else
            {
                //Remove espaços em branco no começo e no final da string.
                item.Nome = item.Nome.Trim();
                //Remove espaços extras entre as palavras, ex: "A      B", ficaria "A B".
                item.Nome = Regex.Replace(item.Nome, @"\s+", " ");
                if (item.Nome.Length < 2 || item.Nome.Length > 50)
                {
                    response.Erros.Add("O nome do gênero deve conter entre 2 e 50 caracteres");
                }
            }
            return response;
        }
    }
}
