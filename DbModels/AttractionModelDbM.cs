using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Models.DTO;

namespace DbModels;

[Table("AttractionModels", Schema = "supusr")]
public class AttractionModelDbM : AttractionModel
{
    [Key]
    public override Guid Id { get; set; }
    [Required]
    [StringLength(50)]
    public override string Name { get; set; }
}