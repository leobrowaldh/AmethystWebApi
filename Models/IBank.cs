namespace Models;

public enum EnBank {xxxxx, Nordea, Swedbank, SBAB, DanskeBank}

public enum EnRiskLevel {xxxxxx, Low, Medium, High}

public interface IBank
{
    public Guid BankId { get; set; }

    public EnBank Banks { get; set; }
    public string BankNumber { get; set; }
    public EnRiskLevel  RiskLevel { get; set; }
   
    public string BankComment {get; set; }
    
    //Navigation properties
    public IAttraction Attraction { get; set; }
}