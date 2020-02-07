using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    class teste
    {
        public Response Insert(Cliente item)
        {
            Response response = Validate(item);
            //Se encontramos erros de validação, retorne-os!
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

        //fsodfosdjaklgjfsdjagfjçgjdf

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
