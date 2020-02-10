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
    public class ClienteBLL : IEntityCRUD<Cliente>
    {
        public Response Delete(int id)
        {
            Response response = new Response();

            using (LocacaoDbContext ctx = new LocacaoDbContext())
            {
                try
                {
                    Cliente c = new Cliente();
                    c.ID = id;
                    ctx.Entry<Cliente>(c).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    response.Erros.Add("Não foi possível deletar o cadastro do cliente");
                    response.Sucesso = false;
                    return response;
                }
                response.Sucesso = true;
                return response;
            }
        }

        public DataResponse<Cliente> GetByID(int id)
        {
            DataResponse<Cliente> response = new DataResponse<Cliente>();

            using (LocacaoDbContext ctx = new LocacaoDbContext())
            {
                try
                {
                    Cliente c = new Cliente();
                    ctx.Clientes.Find(id);
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

        public DataResponse<Cliente> GetData()
        {
            DataResponse<Cliente> response = new DataResponse<Cliente>();

            using (LocacaoDbContext ctx = new LocacaoDbContext())
            {
                try
                {
                    Cliente c = new Cliente();
                    ctx.Clientes.OrderBy(cli => cli.Nome);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    response.Erros.Add("Não foi encontrar o cadastro docliente");
                    response.Sucesso = false;
                    return response;
                }
                response.Sucesso = true;
                return response;
            }
        }

        public Response Insert(Cliente item)
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
                    ctx.Clientes.Add(item);
                    ctx.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                response.Erros.Add("Não foi possível cadastrar o cliente");
                response.Sucesso = false;
                return response;
            }

            return response;
        }

        public Response Update(Cliente item)
        {
            Response response = new Response();

            using (LocacaoDbContext ctx = new LocacaoDbContext())
            {
                try
                {

                    ctx.Entry<Cliente>(item).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();

                }
                catch (Exception ex)
                {
                    response.Erros.Add("Não foi possível deletar o cadastro do cliente");
                    response.Sucesso = false;
                    return response;
                }
                response.Sucesso = true;
                return response;

            }
        }

        private Response Validate(Cliente item)
        {
            Response response = new Response();
            if (string.IsNullOrWhiteSpace(item.Nome))
            {
                response.Erros.Add("O nome do cliente deve ser informado.");
            }
            else
            {
                //Remove espaços em branco no começo e no final da string.
                item.Nome = item.Nome.Trim();
                //Remove espaços extras entre as palavras, ex: "A      B", ficaria "A B".
                item.Nome = Regex.Replace(item.Nome, @"\s+", " ");
                if (item.Nome.Length < 2 || item.Nome.Length > 50)
                {
                    response.Erros.Add("O nome do cliente deve conter entre 2 e 50 caracteres");
                }
            }
            if (string.IsNullOrWhiteSpace(item.Email))
            {
                response.Erros.Add("O email do cliente deve ser informado.");
            }
            else
            {
                //Remove espaços em branco no começo e no final da string.
                item.Email = item.Email.Trim();
                //Remove espaços extras entre as palavras, ex: "A      B", ficaria "A B".
                item.Email = Regex.Replace(item.Email, @"\s+", " ");
                if (item.Email.Length < 5 || item.Email.Length > 50)
                {
                    response.Erros.Add("O email do cliente deve conter entre 2 e 50 caracteres");
                }
            }
            return response;
        }
    }
}



