using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Seido.Utilities.SeedGenerator;
using Models.DTO;


namespace DbModels;
[Table("Visitors", Schema = "supusr")]
public class VisitorDbM : Visitor, ISeed<VisitorDbM>
{
    [Key]
    public override Guid VisitorId { get; set; }

    #region adding more readability to an enum type in the database
    public virtual string strRole
    {
        get => Role.ToString();
        set { }  //set is needed by EFC to include in the database, so I make it to do nothing
    }
    #endregion
    
    [NotMapped]
    public override List<IAttractionModel> AttractionModels { get => AttractionModelsDbM?.ToList<IAttractionModel>(); set => throw new NotImplementedException(); }

    [JsonIgnore]
    [Required]
    public  List<AttractionModelDbM>  AttractionModelsDbM { get; set; } = null;

    public override VisitorDbM Seed (csSeedGenerator _seeder)
    {
        base.Seed (_seeder);
        return this;
    }

    public VisitorDbM UpdateFromDTO(VisitorCuDto org)
    {
        if (org == null) return null;

        Role = org.Role;
        VisitorFirstName = org.FirstName;
        VisitorLastName = org.LastName;
        VisitorEmail = org.Email;

        return this;
    }

    public VisitorDbM() { }
    public VisitorDbM(VisitorCuDto org)
    {
        VisitorId = Guid.NewGuid();
        UpdateFromDTO(org);
    }
}
