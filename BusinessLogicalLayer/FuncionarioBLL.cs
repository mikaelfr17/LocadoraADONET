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
            //TODO: Validar email e Senha! As implementações não serão feitas 
            //pq a gente já viu isso 
            //999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999
            //999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999
            //999999999999999999999999999999999999999999999999999999999999999999 vezes

            //Após validar, caso esteja tudo fofinho e pronto pra funcionar, chama o banco!

            senha = HashUtils.HashPassword(senha);

            DataResponse<Funcionario> response = funcionarioDAL.Autenticar(email, senha);
            if (response.Sucesso)
            {
                User.FuncionarioLogado = response.Data[0];
            }
            return response;
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
            return funcionarioDAL.GetByID(id);
        }

        public DataResponse<Funcionario> GetData()
        {
            return funcionarioDAL.GetData();
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
