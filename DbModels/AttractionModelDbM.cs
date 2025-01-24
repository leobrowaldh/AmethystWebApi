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
    public override Guid AttractionId { get; set; }
    [Required]
    [StringLength(50)]
    public override string Name { get; set; }

    [NotMapped]
    public override List<IComment> Comments { get => CommentsDbM?.ToList<IComment>(); set => throw new NotImplementedException(); }

    [JsonIgnore]
    public List<CommentDbM> CommentsDbM { get; set; }

    public override AttractionModelDbM Seed (csSeedGenerator _seeder)
    {
        base.Seed (_seeder);
        return this;
    }

    public AttractionModelDbM UpdateFromDTO(AttractionModelCuDto org)
    {
        if (org == null) return null;

        Name = org.Name;
        Category = org.Category;

        return this;
    }

    public AttractionModelDbM() { }
    public AttractionModelDbM(AttractionModelCuDto org)
    {
        AttractionId = Guid.NewGuid();
        UpdateFromDTO(org);
    }

    // public AttractionModelDbM Seed(csSeedGenerator rnd)
    // {
    //     this.Name = rnd.FirstName;
    //     this.Category = rnd.FromEnum<AttractionCategory>();
    //     this.Seeded = true;
    //     return this;
    // }
}