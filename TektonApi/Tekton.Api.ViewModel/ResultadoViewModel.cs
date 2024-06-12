namespace Tekton.Api.ViewModel
{
    public class ResultadoViewModel
    {
        public bool Ok { get; set; }

        public List<string> Mensajes { get; set; }

        public bool ErrorValidacion { get; set; }

        public int StatusCode { get; set; }

        public ResultadoViewModel()
        {
            Ok = false;
            Mensajes = new List<string>();
            ErrorValidacion = false;
        }
    }
}
