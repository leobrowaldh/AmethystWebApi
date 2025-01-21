namespace Models;

public enum AttractionCategory { Nature, Historical, Party, Museeu,Adventure,Cultural,Entertainment,Thrill}
public enum AttractionName {Skyview,Suncrest,Driftwood,Starfall,Mistvale,Glowcove,Peakpoint,Tidepool}


public interface IAttractionModel
{
    public Guid Id { get; set;}
    public string Name { get; set;}
    public AttractionCategory Category { get; set;}
    public bool Seeded { get; set; }
}