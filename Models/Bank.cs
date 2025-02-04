using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Identity.Client;
using Seido.Utilities.SeedGenerator;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Models;

public class Bank : IBanks, ISeed<Bank>
{
    public virtual Guid BankId { get; set; }

    public enBank Banks { get; set; }
       public string BankNumber { get; set; }
  
    public enRiskLevel RiskLevel { get; set; }
    public string BankComment {get; set; }

    [JsonIgnore]
    public string EnryptedToken {get; set; }
    

    //Navigation properties
    public virtual IAttractionModel AttractionModel { get; set; }

    #region Seeder
    public bool Seeded { get; set; } = false;

    public virtual Bank Seed (csSeedGenerator seeder)
    {
        Seeded = true;
        BankId = Guid.NewGuid();
    
        Banks = seeder.FromEnum<enBank>();
        RiskLevel = seeder.FromEnum<enRiskLevel>();

        BankNumber = $"{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}";
       BankComment=$"This is a comment for {Banks} with a risk level of {RiskLevel}";
        return this;
    }
    #endregion

    public Bank EnryptAndObfuscate (Func<Bank, string> encryptor)
    {
        this.EnryptedToken = encryptor(this);

       
        this.BankComment=Regex.Replace(BankComment, "(?<=.{1}).", "*");
        this.RiskLevel = enRiskLevel.xxxxxx;
        this.Banks = enBank.xxxxx;
      
        string pattern = @"\b(\d{4}[-\s]?)(\d{4}[-\s]?)(\d{4}[-\s]?)(\d{4})\b";
        string replacement = "$1**** **** **** $4"; 
        this.BankNumber =  Regex.Replace(BankNumber, pattern, replacement);

        
        return this;
    }

    public Bank Decrypt (Func<string, Bank> decryptor)
    {
        return decryptor(this.EnryptedToken);
    }
}

