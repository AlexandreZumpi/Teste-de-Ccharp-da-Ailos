namespace Questao5.Application.Commands.Requests
{
    public class MovimentarContaCommand
    {
        public int NumeroConta { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; } // C ou D
    }
}
