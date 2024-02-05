// See https://aka.ms/new-console-template for more information
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

//using System;
//using System.Collections.Generic;
//using System.Linq;


namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            /*using var db = new Data.ApplicationContext();
            db.Database.Migrate();
            var existe = db.Database.GetPendingMigrations().Any();
            if(existe)
            {
                // 
            }*/

            //InserirDados();
            //InserirDadosEmMassa();
            //ConsultarDados();
            //CadastrarPedido();
            //ConsultarPedidoCarregamentoAdiantado();
            AtualizarDados();
            //RemoverRegistro();
        }

        private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.Find(2);

            if (cliente != null)
            {
                db.Entry(cliente).State = EntityState.Deleted;
                db.SaveChanges();
                Console.WriteLine($"Cliente {cliente.Nome}, excluído com sucesso!");
            }
            else
            {
                Console.WriteLine("Cliente não localizado!");
            }
        }

        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.Find(4);

            if (cliente != null)
            {
                var clienteDesconectado = new
                {
                    Nome = "Fernando Mendes",
                    Telefone = "11993936885"
                };

                db.Attach(cliente);
                db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

                db.SaveChanges();
                Console.WriteLine($"Cliente {cliente.Nome}, atualizado com sucesso!");
            }
            else
            {
                Console.WriteLine("Cliente não localizado!");
            }

        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            var pedidos = db
                .Pedidos
                .Include(p => p.Itens)
                .ThenInclude(p => p.Produto)
                .ToList();

            Console.WriteLine($"Quantidades de pedidos: {pedidos.Count}");
        }

        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                 {
                     new PedidoItem
                     {
                         ProdutoId = produto.Id,
                         Desconto = 0,
                         Quantidade = 1,
                         Valor = 10,
                     }
                 }
            };

            db.Pedidos.Add(pedido);

            db.SaveChanges();
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();
            var consultaPorSintaxe = (from c in db.Clientes where c.Id>0 select c).ToList();
            var consultaPorMetodo = db.Clientes
                .Where(p => p.Id > 0)
                .OrderBy(p => p.Nome)
                .ToList();

            foreach (var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando Cliente: {cliente.Nome}");
                //db.Clientes.Find(cliente.Id);
                db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Rafael Almeida",
                CEP = "99999000",
                Cidade = "Itabaiana",
                Estado = "SE",
                Telefone = "99000001111",
            };

            var listaClientes = new[]
            {
                new Cliente
                {
                    Nome = "Antonio Bruno",
                    CEP = "09720470",
                    Cidade = "São Bernardo do Campo",
                    Estado = "SP",
                    Telefone = "11993936877",
                },
                new Cliente
                {
                    Nome = "Teste 2",
                    CEP = "99999000",
                    Cidade = "Itabaiana",
                    Estado = "SE",
                    Telefone = "99000001116",
                },
            };


            using var db = new Data.ApplicationContext();
            //db.AddRange(produto, cliente);
            //db.Set<Cliente>().AddRange(listaClientes);
            db.Clientes.AddRange(listaClientes);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();
            //db.Produtos.Add(produto);
            //db.Set<Produto>().Add(produto);
            //db.Entry(produto).State = EntityState.Added;
            db.Add(produto);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }
    }
}
