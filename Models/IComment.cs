namespace Models;

public enum CommentType { Constructive, Angry, Spam }

public enum CommentRating { One, Two, Three, Four, Five }

public interface IComment
{
    public Guid CommentId { get; set; }
    public CommentType Type { get; set; }
    public CommentRating Rating { get; set; }

    public int CommentAge { get; set; }
    public string CommentName { get; set; }
    public string CommentText { get; set; }

    public IAttraction Attraction { get; set; }
}