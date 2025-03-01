﻿using System.ComponentModel.DataAnnotations;

namespace Models.DTO;

//DTO is a DataTransferObject, can be instanstiated by the controller logic
//and represents a, fully instantiable, subset of the Database models
//for a specific purpose.

//These DTO are simplistic and used to Update and Create objects
public class BankCuDto
{
    public Guid? BankId { get; set; }
    [EnumDataType(typeof(EnBank), ErrorMessage = $"Invalid {nameof(EnBank)} type.")]
    public EnBank Banks { get; set; }
     public string BankNumber { get; set; }
    [EnumDataType(typeof(EnRiskLevel), ErrorMessage = $"Invalid {nameof(EnRiskLevel)} type.")]
    public EnRiskLevel  RiskLevel { get; set; }
    public string BankComment {get; set; }
    //Navigation properties
    public Guid AttractionId { get; set; }

    public BankCuDto() { }
    public BankCuDto(IBank org)
    {
        BankId = org.BankId;

        Banks = org.Banks;
       

        BankNumber = org.BankNumber;
        BankComment = org.BankComment;
        RiskLevel = org.RiskLevel;

        AttractionId = org.Attraction.AttractionId;
    }
}