using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Models.DTO;
using Seido.Utilities.SeedGenerator;

namespace DbModels;

[Table("Attractions", Schema = "supusr")]
public class AttractionDbM : Attraction, ISeed<AttractionDbM>
{
    [Key]
    public override Guid AttractionId { get; set; }
    [Required]
    [StringLength(50)]
    public override string Name { get; set; }
    [Required]
    [StringLength(100)]
    public override string Description { get; set; }

    #region adding more readability to an enum type in the database
    public virtual string strCategory
    {
        get => Category.ToString();
        set { }  //set is needed by EFC to include in the database, so I make it to do nothing
    }
    #endregion

    [NotMapped] //ignored by efc
    public override List<IComment> Comments { get => CommentsDbM?.ToList<IComment>(); set => throw new NotImplementedException(); }
    [JsonIgnore] //ignored for jsoin serialization
    public List<CommentDbM> CommentsDbM { get; set;}
    [NotMapped]
    public override IAddress Address { get => AddressDbM; set => throw new NotImplementedException(); }
    [JsonIgnore]
    public AddressDbM AddressDbM { get; set; }
    [NotMapped]
    public override IBank Bank { get => BankDbM; set => throw new NotImplementedException(); }
    [JsonIgnore]
    public BankDbM BankDbM { get;set;}

    public override AttractionDbM Seed (csSeedGenerator _seeder)
    {
        base.Seed (_seeder);
        return this;
    }



    
    public AttractionDbM UpdateFromDTO(AttractionCuDto org)
    {
        if (org == null) return null;

       
        Category = org.Category;
        Name = org.Name;
        Description = org.Description;

      
        return this;
    }

    public AttractionDbM() { }

    public AttractionDbM(AttractionCuDto org)
    {
        AttractionId = Guid.NewGuid();
        UpdateFromDTO(org);
    }
}