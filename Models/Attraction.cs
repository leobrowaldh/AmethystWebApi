using Configuration;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class Attraction: IAttraction, ISeed<Attraction>
{
    public virtual Guid AttractionId { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public AttractionCategory Category {  get; set; }
    public virtual List<IComment> Comments { get; set; }
    public virtual IAddress Address { get; set; }

    public bool Seeded { get; set; }

     public virtual IBank Bank { get; set; }
   
    public virtual Attraction Seed(csSeedGenerator rnd)
    {
        this.Name = rnd.FromList(SeedingStrings.AttractionFirstName) + " " + rnd.FromList(SeedingStrings.AttractionLastName);
        this.Category = rnd.FromEnum<AttractionCategory>();
        this.Seeded = true;
        return this;
    }
}