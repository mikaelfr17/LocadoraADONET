using BusinessLogicalLayer.Security;
using DataAccessLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    public class FuncionarioBLL : IEntityCRUD<Funcionario>, IFuncionarioService
    {

        public DataResponse<Funcionario> Autenticar(string email, string senha)
        {
            DataResponse<Funcionario> response = new DataResponse<Funcionario>();


            using (LocacaoDbContext ctx = new LocacaoDbContext())
            {
                try
                {
                    ctx.Funcionarios.Where(c => c.Email == email && c.Senha == senha);
                    ctx.SaveChanges();

                }
                catch (Exception ex)
                {
                    response.Erros.Add("Usuario ou senha invalidos!!");
                    response.Sucesso = false;
                    return response;
                }
                senha = HashUtils.HashPassword(senha);

                if (response.Sucesso)
                {
                    User.FuncionarioLogado = response.Data[0];
                }
                return response;

            }
            //TODO: Validar email e Senha! As implementações não serão feitas 
            //pq a gente já viu isso 
            //999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999
            //999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999
            //999999999999999999999999999999999999999999999999999999999999999999 vezes
            //Após validar, caso esteja tudo fofinho e pronto pra funcionar, chama o banco!
        }//VERIFICAR MAIS TARDE

        public Response Delete(int id)
        {
            Response response = new Response();

            using (LocacaoDbContext ctx = new LocacaoDbContext())
            {
                try
                {
                    Funcionario func = new Funcionario();
                    func.ID = id;
                    ctx.Entry<Funcionario>(func).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    response.Erros.Add("Não foi possível deletar o cadastro do funcionario");
                    response.Sucesso = false;
                    return response;
                }
                response.Sucesso = true;
                return response;
            }
        }

        public DataResponse<Funcionario> GetByID(int id)
        {
            DataResponse<Funcionario> response = new DataResponse<Funcionario>();

            using (LocacaoDbContext ctx = new LocacaoDbContext())
            {
                try
                {
                    Cliente c = new Cliente();
                    ctx.Funcionarios.Find(id);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    response.Erros.Add("Não foi encontrar o cadastro do funcionario");
                    response.Sucesso = false;
                    return response;
                }
                response.Sucesso = true;
                return response;
            }
        }

        public DataResponse<Funcionario> GetData()
        {
            DataResponse<Funcionario> response = new DataResponse<Funcionario>();

            using (LocacaoDbContext ctx = new LocacaoDbContext())
            {
                try
                {
                    ctx.Funcionarios.OrderBy(cli => cli.Nome);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    response.Erros.Add("Não foi encontrar o cadastro do Funcionário");
                    response.Sucesso = false;
                    return response;
                }
                response.Sucesso = true;
                return response;
            }
        }

        public Response Insert(Funcionario item)
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
                    ctx.Funcionarios.Add(item);
                    ctx.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                response.Erros.Add("Não foi possível cadastrar o funcionario");
                response.Sucesso = false;
                return response;
            }

            return response;
        }

        public Response Update(Funcionario item)
        {
            Response response = new Response();

            using (LocacaoDbContext ctx = new LocacaoDbContext())
            {
                try
                {

                    ctx.Entry<Funcionario>(item).State = System.Data.Entity.EntityState.Deleted;
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
        private Response Validate(Funcionario item)
        {
            Response response = new Response();

            if (string.IsNullOrWhiteSpace(item.CPF))
            {
                response.Erros.Add("O cpf deve ser informado");
            }
            else
            {
                item.CPF = item.CPF.Trim();
                if (!item.CPF.IsCpf())
                {
                    response.Erros.Add("O cpf informado é inválido.");
                }
            }

            string validacaoSenha = SenhaValidator.ValidateSenha(item.Senha, item.DataNascimento);
            if (validacaoSenha != "")
            {
                response.Erros.Add(validacaoSenha);
            }
            return response;
        }
    }
}
