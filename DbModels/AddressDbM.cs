using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Models;
using Seido.Utilities.SeedGenerator;

namespace DbModels;

[Table("Addresses", Schema = "supusr")]
public class AddressDbM:Address, ISeed<AddressDbM>
{
    [Key]
    public override Guid AddressId { get; set; }
    [Required]
    public override City City { get; set; }
    [Required]
    public override Country Country { get; set; }
    [Required]
    public override string StreetName { get; set; }

    [NotMapped]
    public override List<IAttractionModel> AttractionModels { get; set; }
    [JsonIgnore]
    public List<AttractionModelDbM> AttractionModelDbMs { get; set; }

    public override AddressDbM Seed (csSeedGenerator _seeder)
    {
        base.Seed (_seeder);
        return this;
    }

    // public AddressDbM UpdateFromDTO(AddressCuDto org)
    // {
    //     if (org == null) return null;


    //     return this;
    // }

    public AddressDbM() { }
    // public AddressDbM(AddressCuDto org)
    // {
    //     AddressId = Guid.NewGuid();
    //     UpdateFromDTO(org);
    // }
}