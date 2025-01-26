using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Models.DTO;
using Seido.Utilities.SeedGenerator;

namespace DbModels;

[Table("Attractions", Schema = "supusr")]
public class AttractionModelDbM : AttractionModel, ISeed<AttractionModelDbM>
{
    [Key]
    public override Guid Id { get; set; }
    [Required]
    [StringLength(50)]
    public override string Name { get; set; }

    #region adding more readability to an enum type in the database
    public virtual string strCategory
    {
        get => Category.ToString();
        set { }  //set is needed by EFC to include in the database, so I make it to do nothing
    }
    #endregion

    public AttractionModelDbM Seed(csSeedGenerator rnd)
    {
        this.Name = rnd.FromList(SeedingStrings.AttractionFirstName) + " " + rnd.FromList(SeedingStrings.AttractionLastName);
        this.Category = rnd.FromEnum<AttractionCategory>();
        this.Seeded = true;
        return this;
    }



    
    public AttractionModelDbM UpdateFromDTO(AttractionCuDto org)
    {
        if (org == null) return null;

       
        Category = org.Category;
        Name = org.Name;

      
        return this;
    }

    public AttractionModelDbM() { }

    public AttractionModelDbM(AttractionCuDto org)
    {
        Id = Guid.NewGuid();
        UpdateFromDTO(org);
    }
}