using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Seido.Utilities.SeedGenerator;
using Models.DTO;


namespace DbModels;
[Table("Comments", Schema = "supusr")]
public class CommentDbM : Comment, ISeed<CommentDbM>
{
    [Key]
    public override Guid CommentId { get; set; }

    #region adding more readability to an enum type in the database
    public virtual string strType
    {
        get => Type.ToString();
        set { }  //set is needed by EFC to include in the database, so I make it to do nothing
    }
    public virtual string strRating
    {
        get => Rating.ToString();
        set { } //set is needed by EFC to include in the database, so I make it to do nothing
    }
    #endregion
    
    [NotMapped]
    public override IAttraction Attraction { get => AttractionDbM; set => throw new NotImplementedException(); }

    [JsonIgnore]
    [Required]
    public  AttractionDbM AttractionDbM { get; set; }

    public override CommentDbM Seed (csSeedGenerator _seeder)
    {
        base.Seed (_seeder);
        return this;
    }

    public CommentDbM UpdateFromDTO(CommentCuDto org)
    {
        if (org == null) return null;

        Type = org.CommentType;
        Rating = org.CommentRating;
        CommentAge = org.CommentAge;
        CommentName = org.CommentName;
        CommentText = org.CommentText;

        return this;
    }

    public CommentDbM() { }
    public CommentDbM(CommentCuDto org)
    {
        CommentId = Guid.NewGuid();
        UpdateFromDTO(org);
    }
}
