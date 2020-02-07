﻿using DataAccessLayer;
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
            }

            return response;
        }
        public Response Delete(int id)
        {
            Response response = new Response();
            if (id <= 0)
            {
                response.Erros.Add("ID do cliente não foi informado.");
            }
            if (response.Erros.Count != 0)
            {
                response.Sucesso = false;
                return response;
            }
            return dal.Delete(id);
        }

        public Response Update(Cliente item)
        {
            Response response = Validate(item);
            //TODO: Verificar a existência desse gênero na base de dados
            //generoBLL.LerID(item.GeneroID);
            //Verifica se tem erros!
            if (response.Erros.Count != 0)
            {
                response.Sucesso = false;
                return response;
            }
            return dal.Update(item);
        }

        public DataResponse<Cliente> GetData()
        {
            return dal.GetData();
        }

        public DataResponse<Cliente> GetByID(int id)
        {
            return dal.GetByID(id);
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
