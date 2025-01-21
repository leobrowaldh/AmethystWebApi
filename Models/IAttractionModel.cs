namespace Models;

public enum AttractionCategory { Nature, Historical, Party, Museeum}

public interface IAttractionModel
{
    public Guid Id { get; set;}
    public string Name { get; set;}
    public AttractionCategory Category { get; set;}
    public bool Seeded { get; set; }
}