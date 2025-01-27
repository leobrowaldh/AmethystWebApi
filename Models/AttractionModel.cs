using Configuration;

namespace Models;

public class AttractionModel: IAttractionModel
{
    public virtual Guid AttractionId { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public AttractionCategory Category {  get; set; }
    public IComment Comments { get; set; }
    public IAddress Address { get; set; }

    public bool Seeded { get; set; }
}