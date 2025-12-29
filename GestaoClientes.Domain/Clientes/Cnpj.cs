using System.Text.RegularExpressions;
using GestaoClientes.Domain.Common;
using GestaoClientes.Domain.Exceptions;

namespace GestaoClientes.Domain.Clientes
{
    public class Cnpj : ValueObject
    {
        public string Valor { get; private set; }

        protected Cnpj() 
        { 
            Valor = null!;
        }

        public Cnpj(string valor)
        {
            if (!Validar(valor))
                throw new DomainException("CNPJ inválido");

            Valor = ApenasNumeros(valor);
        }

        public static bool Validar(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            var numeros = ApenasNumeros(cnpj);

            if (numeros.Length != 14)
                return false;

            if (numeros.All(c => c == numeros[0]))
                return false;

            var digit1 = CalcularDigito(numeros, new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });
            var digit2 = CalcularDigito(numeros, new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });

            return numeros.EndsWith(digit1.ToString() + digit2.ToString());
        }

        private static int CalcularDigito(string numeros, int[] multiplicadores)
        {
            var soma = 0;
            for (var i = 0; i < multiplicadores.Length; i++)
            {
                soma += (numeros[i] - '0') * multiplicadores[i];
            }

            var resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }

        public static string ApenasNumeros(string valor)
        {
            return string.IsNullOrEmpty(valor) ? "" : Regex.Replace(valor, "[^0-9]", "");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }

        public override string ToString()
        {
            return Convert.ToUInt64(Valor).ToString(@"00\.000\.000\/0000\-00");
        }
        
        public static implicit operator string(Cnpj cnpj) => cnpj.Valor;
        public static explicit operator Cnpj(string valor) => new Cnpj(valor);
    }
}
