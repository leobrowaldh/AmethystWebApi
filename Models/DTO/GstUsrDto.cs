using System;
using Configuration;

namespace Models.DTO;

public class GstUsrInfoDbDto
{
    public int NrSeededAttractions {get;  set;} = 0;
    public int NrUnseededAttractions { get; set; } = 0;

    public int NrSeededComments { get; set; } = 0;
    public int NrUnseededComments { get; set; } = 0;

    public int NrSeededAddresses { get; set; } = 0;
    public int NrUnseededAddresses { get; set; } = 0;
    public int NrSeededBanks { get; set; } = 0;
    public int NrUnseededBanks { get; set; } = 0;
}

public class GstUsrInfoAttractionsDto
{
    public string strCategory { get; set; } = null;
    public int NrAttractions { get; set; } = 0;
}

public class GstUsrInfoCommentsDto
{
    public string AttractionCategory { get; set; } = null;
    public string AttractionName { get; set; } = null;
    public int NrComments { get; set; } = 0;
}

public class GstUsrInfoAddressesDto
{
    public string CityName { get; set; } = null;
    public int NrAttractions { get; set; } = 0;
}

public class GstUsrInfoAllDto
{
    public GstUsrInfoDbDto Db { get; set; } = null;
    public List<GstUsrInfoAttractionsDto> Attractions { get; set; } = null;
    public List<GstUsrInfoCommentsDto> Comments { get; set; } = null;
    public List<GstUsrInfoAddressesDto> Addresses { get; set; } = null;
}


