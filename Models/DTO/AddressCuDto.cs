using System;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Models.DTO;

//DTO is a DataTransferObject, can be instanstiated by the controller logic
//and represents a, fully instantiable, subset of the Database models
//for a specific purpose.

//These DTO are simplistic and used to Update and Create objects
public class AddressCuDto
{
    public virtual Guid? AddressId { get; set; }

    public EnCity City { get; set; }
    public EnCountry Country { get; set; }
    
    public string StreetName { get; set; }
    public int ZipCode { get; set; }

    public List<Guid> AttractionIds { get; set; } = null;
    public AddressCuDto() { }
    public AddressCuDto(IAddress org)
    {
        AddressId = org.AddressId;

        City = org.City;
        Country = org.Country;
        StreetName = org.StreetName;
        ZipCode = org.ZipCode;

        AttractionIds = org?.Attractions?.Select(a => a.AttractionId).ToList();
    }
}