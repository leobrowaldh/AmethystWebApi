namespace Models;

public enum EnCity { Stockholm, Copenhagen, Helsinki, Oslo, NYC, London, Beijing, Tokyo, Mumbai, Bangkok, Paris, Kiev }
public enum EnCountry { Sweden, Denmark, Finland, Norway, USA, England, China, Japan, India, Thailand, France, Ukraine }

public interface IAddress
{
    public Guid AddressId { get; set; }
    public EnCity City { get; set; }
    public EnCountry Country { get; set; }

    public string StreetName { get; set; }
    public int ZipCode { get; set; }
        
    public List<IAttraction> Attractions { get; set; }
}