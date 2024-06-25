namespace VacinasApi.Vacinas
{
    public class Vacina
    {
        public Guid Id { get; init; }
        public string Nome { get; private set; }

        public bool Ativo { get; private set; }

        public Vacina(string nome)
        {
            Id = Guid.NewGuid();
            Nome = nome;
        }

        public void UpdateVacina(string nome)
        {
            Nome = nome;
        }

        public void Desativar()
        {
            Ativo = false;
        }
    }
}
