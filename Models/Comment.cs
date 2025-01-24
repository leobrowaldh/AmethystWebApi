using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Identity.Client;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class Comment:IComment, ISeed<Comment>
{
    public virtual Guid CommentId { get; set; } = Guid.NewGuid();
    public CommentType Type { get; set; }
    public CommentRating Rating { get; set; }

    public int CommentAge { get; set; }
    public string CommentName { get; set; }
    public string CommentText { get; set; }

    public virtual IAttractionModel AttractionModel { get; set; }

    public bool Seeded { get; set; } = false;

    public virtual Comment Seed (csSeedGenerator seeder)
    {
        Seeded = true;
        CommentId = Guid.NewGuid();

        Type = seeder.FromEnum<CommentType>();
        Rating = seeder.FromEnum<CommentRating>();

        CommentAge = seeder.Next(0, 30);
        CommentName = seeder.FullName;
        CommentText = seeder.LatinSentence;

        return this;
    }
}