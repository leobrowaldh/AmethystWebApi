﻿using System;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Models.DTO;

//DTO is a DataTransferObject, can be instanstiated by the controller logic
//and represents a, fully instantiable, subset of the Database models
//for a specific purpose.

//These DTO are simplistic and used to Update and Create objects
public class AttractionCuDto
{
    public virtual Guid? AttractionId { get; set; }

   
    public AttractionCategory Category { get; set; }
    public string Name { get; set; }

    public AttractionCuDto() { }
    public AttractionCuDto(IAttractionModel org)
    {
        AttractionId = org.AttractionId;
        Name = org.Name;
        Category = org.Category;
        
    }


    
}