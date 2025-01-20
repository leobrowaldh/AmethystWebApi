using Configuration;

namespace Models;



public class MusicGroup: IMusicGroup
{
    public virtual Guid Id { get; set;}
    public virtual string Name { get; set; }
    public virtual int EstablishedYear { get; set; }
    public virtual MusicGenere Genere { get; set; }
}