namespace Models;

public enum VisitorRole {Family, Single, Couple, FriendGroup, WorkVisit}

public interface IVisitor
{
    public Guid VisitorId { get; set; }

    public VisitorRole Role { get; set; }
    public string VisitorFirstName { get; set; }
    public string VisitorLastName { get; set; }
    public string VisitorEmail { get; set; }

    //Navigation properties
    public List<IAttractionModel> AttractionModels { get; set; }
}