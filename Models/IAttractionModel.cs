namespace Models;

public enum AttractionCategory { Nature, Historical, Party, Museeum, Adventure, Cultural, Entertainment, Thrill}


public interface IAttractionModel
{
    public Guid AttractionId { get; set; }
    public string Name { get; set; }
    public  AttractionCategory Category { get; set; }
    public string Description { get; set; }
    public bool Seeded { get; set; }

    public List<IComment> Comments { get; set; }
    public IAddress Address { get; set; }
}
