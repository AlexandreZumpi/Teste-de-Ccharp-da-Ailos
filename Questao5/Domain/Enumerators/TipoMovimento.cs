namespace Questao5.Domain.Enumerators
{
    public static class TipoMovimento
    {
        public const string Credito = "C";
        public const string Debito = "D";

        public static bool IsValid(string tipo)
        {
            return tipo.ToUpper() == Credito || tipo.ToUpper() == Debito;
        }
    }
}
