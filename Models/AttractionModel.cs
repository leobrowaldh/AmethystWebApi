using Configuration;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class AttractionModel: IAttractionModel, ISeed<AttractionModel>
{
    public virtual Guid AttractionId { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public AttractionCategory Category {  get; set; }
    public IComment Comments { get; set; }
    public IAddress Address { get; set; }

    public bool Seeded { get; set; }

    public virtual AttractionModel Seed(csSeedGenerator rnd)
    {
        this.Name = rnd.FromList(SeedingStrings.AttractionFirstName) + " " + rnd.FromList(SeedingStrings.AttractionLastName);
        this.Category = rnd.FromEnum<AttractionCategory>();
        this.Seeded = true;
        return this;
    }
}