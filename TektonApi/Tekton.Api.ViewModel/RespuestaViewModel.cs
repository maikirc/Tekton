namespace Tekton.Api.ViewModel
{
    public class RespuestaViewModel<T>
    {
        public T DataResult { get; set; }

        public ResultadoViewModel Resultado { get; set; }

        public RespuestaViewModel()
        {
            Resultado = new ResultadoViewModel();
        }
    }
}
