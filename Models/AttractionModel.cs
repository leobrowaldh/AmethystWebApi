using Configuration;

namespace Models;

public class AttractionModel: IAttractionModel
{
    public virtual Guid AttractionId { get; set; }
    public virtual string Name {  get; set; }
    public AttractionCategory Category {  get; set; }
    public bool Seeded { get; set; }
}