using System.ComponentModel.DataAnnotations;
namespace Models.DTO;

//DTO is a DataTransferObject, can be instanstiated by the controller logic
//and represents a, fully instantiable, subset of the Database models
//for a specific purpose.

//These DTO are simplistic and used to Update and Create objects
public class AttractionCuDto
{
    public virtual Guid? AttractionId { get; set; }

    [EnumDataType(typeof(AttractionCategory), ErrorMessage = $"Invalid {nameof(AttractionCategory)} type.")]
    public AttractionCategory Category { get; set; }
    public string Name { get; set; }

    public virtual List<Guid> CommentIds { get; set; } = [];
    public virtual Guid AddressId { get; set; }
    


    public AttractionCuDto() { }
    public AttractionCuDto(IAttraction org)
    {
        AttractionId = org.AttractionId;
        Name = org.Name;
        Category = org.Category;
        
        CommentIds = org.Comments?.Select(c => c.CommentId).ToList();
        AddressId = org.Address.AddressId;
    }


    
}