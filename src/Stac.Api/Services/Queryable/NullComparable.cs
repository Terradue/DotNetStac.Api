using System;

namespace Stac.Api.Services.Queryable
{
    internal class NullComparable : IComparable
    {
        public int CompareTo(object obj)
        {
            if ( obj == null )
            {
                return 0;
            }
            return -1;
        }
    }
}