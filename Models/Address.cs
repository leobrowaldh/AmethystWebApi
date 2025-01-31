using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Identity.Client;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class Address:IAddress, ISeed<Address>
{
    public virtual Guid AddressId { get; set; } = Guid.NewGuid();
    public virtual City City { get; set; }
    public virtual Country Country { get; set; }

    public virtual string StreetName { get; set; }
    public int ZipCode { get; set; }

    public virtual List<IAttractionModel> AttractionModels { get; set; }

    public bool Seeded { get; set; } = false;

    public virtual Address Seed (csSeedGenerator seeder)
    {
        Seeded = true;
        AddressId = Guid.NewGuid();

        City = seeder.FromEnum<City>();
        Country = seeder.FromEnum<Country>();

        StreetName = seeder.FullName;
        ZipCode = seeder.Next(00001, 9999);

        return this;
    }
}