using System.ComponentModel.DataAnnotations;
using VacinasApi.Models;

namespace VacinasApi.ViewModels
{
    public class CreatePostoViewModel
    {
        public string Nome { get; set; }
        public List<CreateVacinaViewModel> Vacinas { get; set; }
    }
}
