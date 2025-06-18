namespace Questao5.Domain.Language
{
    public class BusinessException : Exception
    {
        public string Tipo { get; }

        public BusinessException(string tipo, string mensagem) : base(mensagem)
        {
            Tipo = tipo;
        }
    }
}
