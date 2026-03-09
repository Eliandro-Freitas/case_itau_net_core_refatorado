namespace CaseItau.Domain.Entities;

public class FundType
{
    private FundType() { }

    private FundType(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; private init; }
    public string Name { get; private init; }
    public ICollection<Fund> Funds { get; private init; }

    public static FundType Create(int id, string name)
        => new(id, name);
}