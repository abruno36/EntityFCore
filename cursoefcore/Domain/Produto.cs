using CursoEFCore.ValueObjects;

namespace CursoEFCore.Domain
{
    public class Produto
    {
        public int Id { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public TipoProduto TipoProduto { get; set; }
        public bool Ativo { get; set; } = false;
    }
}