﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;

namespace Models.DTO;

public class ResponsePageDto<T>
{
    public List<T> PageItems { get; init; }
    public int DbItemsCount { get; init; }

    public int PageNr { get; init; }
    public int PageSize { get; init; }
    public int PageCount => (int)Math.Ceiling((double)DbItemsCount / PageSize);
    public string DbConnectionKeyUsed {get; init;}
}

public class ResponseItemDto<T>
{
    public T Item { get; init; }
    public string DbConnectionKeyUsed {get; init;}
}

public class RobustResponseItemDto
{
    public UserDto UserResponse { get; init; }
    public ResponseItemDto<GstUsrInfoAllDto> DbSeedingResponse { get; init; }
}