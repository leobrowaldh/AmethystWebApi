namespace Models;

public enum enBank {xxxxx, Nordea, Swedbank, SBAB, DanskeBank}

public enum enRiskLevel {xxxxxx, Low, Medium, High}

public interface IBanks
{
    public Guid BankId { get; set; }

    public enBank Banks { get; set; }
    public string BankNumber { get; set; }
    public enRiskLevel  RiskLevel { get; set; }
   
    public string BankComment {get; set; }
    
    //Navigation properties
    public IAttractionModel AttractionModel { get; set; }
}