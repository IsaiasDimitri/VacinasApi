namespace VacinasApi.ViewModels
{
    public class UpdateVacinaViewModel
    {
        public string Nome { get; set; }
        public string Fabricante { get; set; }
        public string Lote { get; set; }
        public int Quantidade { get; set; }
        public DateTime Validade { get; set; }
        public Guid? PostoId { get; set; }
    }
}
