using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Models;
using Seido.Utilities.SeedGenerator;
using Models.DTO;

namespace DbModels;

[Table("Addresses", Schema = "supusr")]
public class AddressDbM:Address, ISeed<AddressDbM>
{
    [Key]
    public override Guid AddressId { get; set; }
    [Required]
    public virtual string strCity
    { 
        get => City.ToString();
        set { }
    }
    [Required]
    public virtual string strCountry
    { 
        get => Country.ToString(); 
        set { }
    }
    
    [Required]
    public override string StreetName { get; set; }

    [NotMapped]
    public override List<IAttraction> AttractionModels { get; set; }
    [JsonIgnore]
    public List<AttractionDbM> AttractionModelDbM { get; set; }

    public override AddressDbM Seed (csSeedGenerator _seeder)
    {
        base.Seed (_seeder);
        return this;
    }

    public AddressDbM UpdateFromDTO(AddressCuDto org)
    {
        if (org == null) return null;

        City = org.City;
        Country = org.Country;
        StreetName = org.StreetName;
        ZipCode = org.ZipCode;

        return this;
    }

    public AddressDbM() { }
    public AddressDbM(AddressCuDto org)
    {
        AddressId = Guid.NewGuid();
        UpdateFromDTO(org);
    }
}