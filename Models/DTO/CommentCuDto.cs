using System;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Models.DTO;

//DTO is a DataTransferObject, can be instanstiated by the controller logic
//and represents a, fully instantiable, subset of the Database models
//for a specific purpose.

//These DTO are simplistic and used to Update and Create objects
public class CommentCuDto
{
    public virtual Guid? CommentId { get; set; }
    [EnumDataType(typeof(CommentType), ErrorMessage = $"Invalid {nameof(CommentType)} type.")]
    public CommentType CommentType { get; set; }
    [EnumDataType(typeof(CommentRating), ErrorMessage = $"Invalid {nameof(CommentRating)} type.")]
    public CommentRating CommentRating { get; set; }
    
    public int CommentAge { get; set; }
    public string CommentName { get; set; }
    public string CommentText { get; set; }

    public virtual Guid? AttractionId { get; set; } = null;
    public CommentCuDto() { }
    public CommentCuDto(IComment org)
    {
        CommentId = org.CommentId;

        CommentType = org.Type;
        CommentRating = org.Rating;
        CommentAge = org.CommentAge;
        CommentName = org.CommentName;
        CommentText = org.CommentText;

        AttractionId = org?.Attraction?.AttractionId;
    }
}