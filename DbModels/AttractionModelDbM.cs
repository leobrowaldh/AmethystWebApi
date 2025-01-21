using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Models.DTO;
using Seido.Utilities.SeedGenerator;

namespace DbModels;

[Table("AttractionModels", Schema = "supusr")]
public class AttractionModelDbM : AttractionModel, ISeed<AttractionModelDbM>
{
    [Key]
    public override Guid Id { get; set; }
    [Required]
    [StringLength(50)]
    public override string Name { get; set; }

    public AttractionModelDbM Seed(csSeedGenerator rnd)
    {
        this.Name = rnd.FirstName;
        this.Category = rnd.FromEnum<AttractionCategory>();
        this.Seeded = true;
        return this;
    }
}