namespace Models;

public enum City { Stockholm, Copenhagen, Helsinki, Oslo, NYC, London, Beijing, Tokyo, Mumbai, Bangkok, Paris, Kiev }
public enum Country { Sweden, Denmark, Finland, Norway, USA, England, China, Japan, India, Thailand, France, Ukraine }

public interface IAddress
{
    public Guid AddressId { get; set; }
    public City City { get; set; }
    public Country Country { get; set; }

    public string StreetName { get; set; }
    public int ZipCode { get; set; }

    public List<IAttractionModel> AttractionModels { get; set; }
}