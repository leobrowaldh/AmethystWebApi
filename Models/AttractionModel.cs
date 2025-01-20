using Configuration;

namespace Models;

public class AttractionModel: IAttractionModel
{
    public virtual Guid Id { get; set;}
    public virtual string Name {  get; set;}
    public AttractionCategory Category {  get; set;}
}