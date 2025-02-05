using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Seido.Utilities.SeedGenerator;
using Models.DTO;

namespace DbModels;
[Table("Banks", Schema = "supusr")]
public class BankDbM : Bank, ISeed<Bank>
{
    [Key]
    public override Guid  BankId { get; set; }

    #region adding more readability to an enum type in the database
    public virtual string strIssuer
    {
        get => Banks.ToString();
        set { }  //set is needed by EFC to include in the database, so I make it to do nothing
    }
    #endregion
    
    //In a one-to-one relationship, you needed to establish that the Foreign Key should in this table, CreditCards
    //Connecting below FK above with EmployeeDbM.EmployeeId
    [JsonIgnore]
    public virtual Guid AttractionId { get; set; }

    [NotMapped]
    public override IAttraction Attraction { get => AttractionDbM; set => throw new NotImplementedException(); }

    [JsonIgnore]
    [Required]
    [ForeignKey ("AttractionId")] //Connecting FK above with EmployeeDbM.EmployeeId
    public AttractionDbM   AttractionDbM { get; set; } = null;


    public override BankDbM Seed (csSeedGenerator _seeder)
    {
        base.Seed (_seeder);
        return this;
    }

    public BankDbM UpdateFromDTO(BankCuDto org)
    {
        if (org == null) return null;

        Banks = org.Banks;
        BankNumber = org.BankNumber;
     
        RiskLevel = org.RiskLevel;
        BankComment = org.BankComment;


       

        return this;
    }

    public BankDbM() { }
    public BankDbM(BankCuDto org)
    {
        BankId = Guid.NewGuid();
        UpdateFromDTO(org);
    }
}
