using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
        private const double TaxaSaque = 3.50;

        // Propriedade somente leitura (número da conta não pode ser alterado)
        public int Numero { get; }

        // Propriedade leitura/escrita (nome pode ser alterado)
        public string Titular { get; set; }

        // Saldo só pode ser alterado internamente
        public double Saldo { get; private set; }

        // Construtor sem depósito inicial
        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
            Saldo = 0.0;
        }

        // Construtor com depósito inicial
        public ContaBancaria(int numero, string titular, double depositoInicial) : this(numero, titular)
        {
            Deposito(depositoInicial);
        }

        // Método para realizar depósito
        public void Deposito(double quantia)
        {
            Saldo += quantia;
        }

        // Método para realizar saque (com taxa de R$ 3.50)
        public void Saque(double quantia)
        {
            Saldo -= (quantia + TaxaSaque);
        }

        // Override do ToString para exibir os dados da conta
        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }
    }

}
