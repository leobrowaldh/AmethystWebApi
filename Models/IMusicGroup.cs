namespace Models;

public enum MusicGenere
{
    Rock,
    Pop,
    Jazz,
    Blues,
    Country,
    HipHop,
    Metal,
    Electronic,
    Classical,
    Reggae,
    Folk,
    Punk,
    RnB,
    Soul,
    Funk,
    Disco,
    Techno,
    House,
    Dubstep,
    Trance,
    DrumAndBass,
    Garage,
    Grime,
    Breakbeat,
    Ambient,
    Downtempo,
    Chillout,
    TripHop,
    Industrial
}

public interface IMusicGroup
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int EstablishedYear { get; set; }
    public MusicGenere Genere { get; set; }
}