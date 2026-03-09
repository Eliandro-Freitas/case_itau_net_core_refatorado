using CaseItau.Domain.Extensions;

namespace CaseItau.Domain.ValueObjects;

public sealed class Document
{
    public string Value { get; }

    private Document(string value)
        => Value = value;

    public static Document Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("O CNPJ é obrigatório.");

        var digits = new string([.. value.Where(char.IsDigit)]);

        if (digits.Length != 14)
            throw new ArgumentException("O CNPJ deve conter exatamente 14 dígitos numéricos.");

        if (!digits.IsValidCnpj())
            throw new ArgumentException("O CNPJ informado é inválido.");

        return new Document(digits);
    }

    public override bool Equals(object obj)
        => obj is Document other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}