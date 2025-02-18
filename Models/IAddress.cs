namespace Models;

public enum EnCity {
    Stockholm, Copenhagen, Helsinki, Oslo, NYC, London, Beijing, Tokyo, Mumbai, Bangkok, Paris, Kiev,
    Berlin, Madrid, Rome, Lisbon, Amsterdam, Brussels, Vienna, Zurich, Sydney, Melbourne, Toronto, Vancouver,
    Dubai, Singapore, Seoul, Hong_Kong, Istanbul, Cairo, Johannesburg, Nairobi, BuenosAires, Santiago, Lima,
    MexicoCity, Caracas, Manila, Jakarta, Karachi, Dhaka, Colombo, Kathmandu, Tbilisi, Yerevan, Baku,
    Tashkent, Ashgabat, Bishkek, Dushanbe, Almaty, Samarkand, Ankara, Nicosia, Sofia, Bucharest, Belgrade,
    Zagreb, Ljubljana, Bratislava, Prague, Warsaw, Budapest, Tallinn, Riga, Vilnius, SanFrancisco, LosAngeles,
    Chicago, Dallas, Houston, Miami, Atlanta, Boston, Seattle, Phoenix, Philadelphia, Detroit, Baltimore,
    NewOrleans, Orlando, Minneapolis, StLouis, SanDiego, Tampa, Charlotte, Indianapolis, Nashville, KansasCity,
    Columbus, SaltLakeCity, Cincinnati, Pittsburgh, Milwaukee, Portland, Sacramento, Raleigh, VirginiaBeach,
    Omaha, Tulsa, Albuquerque, Tucson, Fresno, LongBeach, Mesa, Birmingham, Mobile, Knoxville, Chattanooga,
    Augusta, Albany, Syracuse, Bridgeport, NewHaven, Stamford, Springfield, Worcester, CapeCoral, FortWayne,
    Evansville, SouthBend, CedarRapids, Tallahassee, Gainesville, Lakeland, CoralSprings, PembrokePines,
    FortLauderdale, Hialeah, Richmond, WashingtonDC, NewYork
}
public enum EnCountry {
    Sweden, Denmark, Finland, Norway, USA, England, China, Japan, India, Thailand, France, Ukraine,
    Germany, Spain, Italy, Portugal, Netherlands, Belgium, Austria, Switzerland, Australia, Canada,
    UAE, Singapore, SouthKorea, HongKong, Turkey, Egypt, SouthAfrica, Kenya, Argentina, Chile,
    Peru, Mexico, Venezuela, Philippines, Indonesia, Pakistan, Bangladesh, SriLanka, Nepal, Georgia,
    Armenia, Azerbaijan, Uzbekistan, Turkmenistan, Kyrgyzstan, Tajikistan, Kazakhstan, Belarus, Moldova,
    Romania, Bulgaria, Serbia, Croatia, Slovenia, Slovakia, CzechRepublic, Poland, Hungary, Estonia,
    Latvia, Lithuania, Brazil, Russia, Greece, Ireland, Iceland, Malta, Cyprus, Luxembourg,
    Monaco, Andorra, SanMarino, VaticanCity, Kosovo, Montenegro, NorthMacedonia, Albania, BosniaAndHerzegovina
}

public interface IAddress
{
    public Guid AddressId { get; set; }
    public EnCity City { get; set; }
    public EnCountry Country { get; set; }

    public string StreetName { get; set; }
    public int ZipCode { get; set; }
        
    public List<IAttraction> Attractions { get; set; }
}