namespace VacinasApi.Models
{
    public class Posto
    {
        public Guid Id { get; init; }
        public string Nome { get; set; }
        public bool Ativo { get; private set; }
        public List<Vacina> Vacinas { get; set; }

        public Posto()
        {
            Ativo = true;
            Vacinas = new List<Vacina>();
        }

        public Posto(string nome)
        {
            Nome = nome;
            Ativo = true;
            Vacinas = new List<Vacina>();
        }
        public void Desativar()
        {
            Ativo = false;
        }
    }
}
