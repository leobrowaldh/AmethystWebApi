using Configuration;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class Visitor:IVisitor, ISeed<Visitor>
{
    public virtual Guid VisitorId { get; set; }
    public VisitorRole Role { get; set; }
    
    public string VisitorFirstName { get; set; }
    public string VisitorLastName { get; set; }
    public string VisitorEmail { get; set; }

    //Navigation properties
    public virtual List<IAttractionModel> AttractionModels { get; set; }

    #region Seeder
    public bool Seeded { get; set; } = false;

    public virtual Visitor Seed (csSeedGenerator seeder)
    {
        Seeded = true;
        VisitorId = Guid.NewGuid();
        
        Role = seeder.FromEnum<VisitorRole>();
        VisitorFirstName = seeder.FirstName;
        VisitorLastName = seeder.LastName;
        VisitorEmail = seeder.Email(VisitorFirstName, VisitorLastName);

        return this;
    }
    #endregion
    
}