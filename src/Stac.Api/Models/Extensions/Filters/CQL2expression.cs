using System;
using Newtonsoft.Json;
using Stac.Api.Converters;
using Stac.Api.Interfaces;

namespace Stac.Api.Models.Cql2
{
    [JsonConverter(typeof(CQL2FilterConverter))]
    public class CQL2Expression : ISearchExpression, IConvertible
    {
        public CQL2Expression(BooleanExpression expression)
        {
            Expression = expression;
        }

        public BooleanExpression Expression { get; }

        public FilterLang FilterLang { get; set; }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider provider)
        {
            return Expression.ToString();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            if ( FilterLang == FilterLang.Cql2Json && conversionType == typeof(BooleanExpression) )
            {
                return Expression;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }
    }
}