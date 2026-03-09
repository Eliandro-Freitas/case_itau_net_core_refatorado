namespace CaseItau.Domain.Extensions;

public static class DocumentExtensions
{
    public static bool IsValidCnpj(this string document)
    {
        if (string.IsNullOrEmpty(document))
            return false;

        document = new string([.. document.Where(char.IsDigit)]);

        if (document.Length != 14 || document.Distinct().Count() == 1)
            return false;

        int[] multiplier1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplier2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        var sum = multiplier1.Select((m, i) => int.Parse(document[i].ToString()) * m).Sum();
        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;

        sum = multiplier2.Select((m, i) => int.Parse(document[i].ToString()) * m).Sum();
        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;

        return document[12] == digit1.ToString()[0] && document[13] == digit2.ToString()[0];
    }

    public static string OnlyDigits(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return new([.. value.Where(char.IsDigit)]);
    }
}