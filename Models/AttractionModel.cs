using Configuration;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class AttractionModel: IAttractionModel
{
    public virtual Guid AttractionId { get; set; }
    public virtual string Name {  get; set; }
    public AttractionCategory Category {  get; set; }
    public bool Seeded { get; set; }

    public virtual List<IComment> Comments { get; set; }
    public virtual AttractionModel Seed (csSeedGenerator seeder)
    {
        AttractionId = Guid.NewGuid();
        Name = $"Attraction {seeder.MusicAlbumName}";
        Category = rnd.FromEnum<AttractionCategory>();

        return this;
    }
}