namespace VacinasApi.Postos
{
    public class Posto
    {
        public Guid Id { get; init; }
        public string Nome { get; private set; }

        public bool Ativo { get; private set; }

        public Posto(string nome) 
        { 
            Nome = nome; 
            Id = Guid.NewGuid();
        }

        public void UpdatePosto(string nome)
        {
            Nome = nome; 
        }

        public void Desativar()
        {
            Ativo = false;
        }
    }
}
