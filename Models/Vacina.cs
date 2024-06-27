using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VacinasApi.Models
{
    public class Vacina
    {
        public Guid Id { get; init; }
        public string Nome { get; set; }
        public bool Ativo { get; private set; }
        public string Fabricante { get; set; }
        public string Lote { get; set; }
        public int Quantidade { get; set; }
        public DateTime Validade { get; set; }
        public Guid? PostoId { get; set; }
        public Posto Posto { get; set; }

        public Vacina()
        {
            Id = Guid.NewGuid();
            Nome = string.Empty;
            Ativo = true;
            Fabricante = string.Empty;
            Lote = string.Empty;
            Quantidade = 0;
            Validade = DateTime.MinValue;
        }

        public Vacina(string nome, string fabricante, string lote, int quantidade, DateTime validade)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Ativo = true;
            Fabricante = fabricante;
            Lote = lote;
            Quantidade = quantidade;
            Validade = validade;
        }

        public void DefinirPosto(Posto posto)
        {
            Posto = posto ?? throw new ArgumentNullException(nameof(posto));
            PostoId = posto.Id;
        }

        public void Desativar()
        {
            Ativo = false;
        }
    }
}
