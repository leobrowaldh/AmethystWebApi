using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Identity.Client;
using Seido.Utilities.SeedGenerator;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Models;

public class Bank : IBank, ISeed<Bank>
{
    public virtual Guid BankId { get; set; }

    public EnBank Banks { get; set; }
       public string BankNumber { get; set; }
  
    public EnRiskLevel RiskLevel { get; set; }
    public string BankComment {get; set; }

    [JsonIgnore]
    public string EncryptedToken {get; set; }
    

    //Navigation properties
    public virtual IAttraction Attraction { get; set; }

    #region Seeder
    public bool Seeded { get; set; } = false;

    public virtual Bank Seed (csSeedGenerator seeder)
    {
        Seeded = true;
        BankId = Guid.NewGuid();
    
        Banks = seeder.FromEnum<EnBank>();
        RiskLevel = seeder.FromEnum<EnRiskLevel>();

        BankNumber = $"{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}";
       BankComment=$"This is a comment for {Banks} with a risk level of {RiskLevel}";
        return this;
    }
    #endregion

    public Bank EnryptAndObfuscate (Func<Bank, string> encryptor)
    {
        this.EncryptedToken = encryptor(this);

       
        this.BankComment=Regex.Replace(BankComment, "(?<=.{1}).", "*");
        this.RiskLevel = EnRiskLevel.xxxxxx;
        this.Banks = EnBank.xxxxx;
      
        string pattern = @"\b(\d{4}[-\s]?)(\d{4}[-\s]?)(\d{4}[-\s]?)(\d{4})\b";
        string replacement = "$1**** **** **** $4"; 
        this.BankNumber =  Regex.Replace(BankNumber, pattern, replacement);

        
        return this;
    }

    public Bank Decrypt (Func<string, Bank> decryptor)
    {
        return decryptor(this.EncryptedToken);
    }
}

