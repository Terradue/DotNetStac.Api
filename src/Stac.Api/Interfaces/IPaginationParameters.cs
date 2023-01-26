using System;
using System.Collections.Generic;

namespace Stac.Api.Interfaces
{
    public interface IPaginationParameters
    {
        public const string PaginationPropertiesKey = "PaginationProperties";

        int? Limit { get; }
        int? Page { get; }
        int? Offset { get; }
        string Token { get; }

    }
}