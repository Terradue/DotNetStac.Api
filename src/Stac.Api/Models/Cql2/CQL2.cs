#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"

namespace Stac.Api.Models.Cql2
{
    using System;
    using System.Collections.Generic;
    using GeoJSON.Net;
    using GeoJSON.Net.Geometry;
    using Itenso.TimePeriod;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Stac;
    using Stac.Api.Converters;
    using Stac.Api.Interfaces;
    using System = global::System;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FilterLang
    {

        [System.Runtime.Serialization.EnumMember(Value = @"cql2-text")]
        Cql2Text = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"cql2-json")]
        Cql2Json = 1,

    }

    [JsonConverter(typeof(BooleanExpressionConverter))]
    public abstract partial class BooleanExpression : IIsNullOperand, IScalarExpression, IFilterExpression
    {

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

        public abstract AndOrExpression AsAndOrExpression();
        public abstract NotExpression AsNotExpression();
        public abstract ComparisonPredicate AsComparison();
        public abstract SpatialPredicate AsSpatialPredicate();
        public abstract TemporalPredicate AsTemporalPredicate();
        public abstract ArrayPredicate AsArrayPredicate();

        public CharExpression AsCharExpression()
        {
            return null;
        }

        public Number AsNumeric()
        {
            return null;
        }

        ITemporalExpression IIsNullOperand.AsTemporalExpression()
        {
            return null;
        }

        public BooleanExpression AsBooleanExpression()
        {
            return this;
        }

        public IGeomExpression AsGeomExpression()
        {
            return null;
        }

        public ITemporalInstantExpression AsTemporalInstant()
        {
            return null;
        }

        INumericExpression IScalarExpression.AsNumericExpression()
        {
            return null;
        }

        public IScalarExpression AsScalarExpression()
        {
            return this;
        }

        public ScalarExpressionCollection AsScalarExpressionCollection()
        {
            return null;
        }
    }

    [JsonConverter(typeof(AndOrExpressionConverter))]
    public class AndOrExpression : BooleanExpression
    {
        [Newtonsoft.Json.JsonProperty("op", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public AndOrExpressionOp Op { get; set; }

        [Newtonsoft.Json.JsonProperty("args", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(2)]
        public System.Collections.Generic.List<BooleanExpression> Args { get; set; } = new System.Collections.Generic.List<BooleanExpression>();

        public override AndOrExpression AsAndOrExpression()
        {
            return this;
        }

        public override ArrayPredicate AsArrayPredicate()
        {
            return null;
        }

        public override ComparisonPredicate AsComparison()
        {
            return null;
        }

        public override NotExpression AsNotExpression()
        {
            return null;
        }

        public override SpatialPredicate AsSpatialPredicate()
        {
            return null;
        }

        public override TemporalPredicate AsTemporalPredicate()
        {
            return null;
        }
    }

    public class NotExpression : BooleanExpression
    {
        [Newtonsoft.Json.JsonProperty("op", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public NotExpressionOp Op { get; set; }

        [Newtonsoft.Json.JsonProperty("args", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(1)]
        [System.ComponentModel.DataAnnotations.MaxLength(1)]
        public System.Collections.Generic.List<BooleanExpression> Args { get; set; } = new System.Collections.Generic.List<BooleanExpression>();

        public override AndOrExpression AsAndOrExpression()
        {
            return null;
        }

        public override ArrayPredicate AsArrayPredicate()
        {
            return null;
        }

        public override ComparisonPredicate AsComparison()
        {
            return null;
        }

        public override NotExpression AsNotExpression()
        {
            return this;
        }

        public override SpatialPredicate AsSpatialPredicate()
        {
            return null;
        }

        public override TemporalPredicate AsTemporalPredicate()
        {
            return null;
        }
    }

    [JsonConverter(typeof(ComparisonPredicateConverter))]
    public abstract class ComparisonPredicate : BooleanExpression
    {
        public abstract BinaryComparisonPredicate AsBinaryComparison();
        public abstract IsLikePredicate AsIsLikePredicate();
        public abstract IsBetweenPredicate AsIsBetweenPredicate();
        public abstract IsInListPredicate AsIsInListPredicate();
        public abstract IsNullPredicate AsIsNullPredicate();
    }

    [JsonConverter(typeof(BinaryComparisonPredicateConverter))]
    public class BinaryComparisonPredicate : ComparisonPredicate
    {
        [Newtonsoft.Json.JsonProperty("op", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ComparisonPredicateOp Op { get; set; }

        [Newtonsoft.Json.JsonProperty("args", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(2)]
        [System.ComponentModel.DataAnnotations.MaxLength(2)]
        public ScalarOperands Args { get; set; } = new ScalarOperands(new List<IScalarExpression>());

        public override AndOrExpression AsAndOrExpression()
        {
            return null;
        }

        public override ArrayPredicate AsArrayPredicate()
        {
            return null;
        }

        public override BinaryComparisonPredicate AsBinaryComparison()
        {
            return this;
        }

        public override ComparisonPredicate AsComparison()
        {
            return this;
        }

        public override IsBetweenPredicate AsIsBetweenPredicate()
        {
            return null;
        }

        public override IsInListPredicate AsIsInListPredicate()
        {
            return null;
        }

        public override IsLikePredicate AsIsLikePredicate()
        {
            return null;
        }

        public override IsNullPredicate AsIsNullPredicate()
        {
            return null;
        }

        public override NotExpression AsNotExpression()
        {
            return null;
        }

        public override SpatialPredicate AsSpatialPredicate()
        {
            return null;
        }

        public override TemporalPredicate AsTemporalPredicate()
        {
            return null;
        }

    }

    [JsonConverter(typeof(ScalarOperandsConverter))]
    public class ScalarOperands : System.Collections.ObjectModel.Collection<IScalarExpression>
    {
        public ScalarOperands(IList<IScalarExpression> exprs) : base(exprs)
        {
        }
    }

    [JsonConverter(typeof(CharExpressionConverter))]
    public abstract class CharExpression : IScalarExpression, IIsNullOperand
    {
        public abstract CaseiExpression AsCasei();
        public abstract AccentiExpression AsAccenti();
        public abstract String AsString();
        public abstract PropertyRef AsPropertyRef();
        public abstract FunctionRef AsFunctionRef();

        public CharExpression AsCharExpression()
        {
            return this;
        }

        public ITemporalInstantExpression AsTemporalInstant()
        {
            return null;
        }

        public INumericExpression AsNumericExpression()
        {
            return null;
        }

        public BooleanExpression AsBooleanExpression()
        {
            return null;
        }

        Number IIsNullOperand.AsNumeric()
        {
            return null;
        }

        public ITemporalExpression AsTemporalExpression()
        {
            return null;
        }

        public IGeomExpression AsGeomExpression()
        {
            return null;
        }

        public IScalarExpression AsScalarExpression()
        {
            return this;
        }

        public ScalarExpressionCollection AsScalarExpressionCollection()
        {
            return null;
        }
    }

    public class CaseiExpression : CharExpression, IPatternExpression
    {
        [Newtonsoft.Json.JsonProperty("casei", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public CharExpression Casei { get; set; }

        public override AccentiExpression AsAccenti()
        {
            return null;
        }

        public override CaseiExpression AsCasei()
        {
            return this;
        }

        public override FunctionRef AsFunctionRef()
        {
            return null;
        }

        public override PropertyRef AsPropertyRef()
        {
            return null;
        }

        public override String AsString()
        {
            return null;
        }
    }

    [JsonConverter(typeof(StringExpressionConverter))]
    public class String : CharExpression, IPatternExpression, IScalarLiteral
    {
        public String(string value)
        {
            Str = value;
        }

        public string Str { get; set; }

        public IComparable Value => Str;

        public override AccentiExpression AsAccenti()
        {
            return null;
        }

        public override CaseiExpression AsCasei()
        {
            return null;
        }

        public override FunctionRef AsFunctionRef()
        {
            return null;
        }

        public override PropertyRef AsPropertyRef()
        {
            return null;
        }

        public override String AsString()
        {
            return this;
        }

        public override string ToString()
        {
            return Str;
        }
    }

    public abstract class InstantLiteral : ITemporalInstantExpression, IScalarLiteral, ITemporalLiteral
    {
        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        protected InstantLiteral()
        {
        }

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

        public abstract DateTimeOffset DateTime { get; set; }

        [JsonIgnore]
        public IComparable Value => DateTime;

        public BooleanExpression AsBooleanExpression()
        {
            return null;
        }

        public CharExpression AsCharExpression()
        {
            return null;
        }

        public FunctionRef AsFunctionRef()
        {
            return null;
        }

        public IGeomExpression AsGeomExpression()
        {
            return null;
        }

        public IntervalLiteral AsIntervalLiteral()
        {
            return null;
        }

        public INumericExpression AsNumericExpression()
        {
            return null;
        }

        public PropertyRef AsPropertyRef()
        {
            return null;
        }

        public IScalarExpression AsScalarExpression()
        {
            return this;
        }

        public ScalarExpressionCollection AsScalarExpressionCollection()
        {
            return null;
        }

        public ITemporalExpression AsTemporalExpression()
        {
            return this;
        }

        public ITemporalInstantExpression AsTemporalInstant()
        {
            return this;
        }

        public ITemporalLiteral AsTemporalLiteral()
        {
            return this;
        }

        InstantLiteral ITemporalLiteral.AsInstantLiteral()
        {
            return this;
        }

        Number IIsNullOperand.AsNumeric()
        {
            return null;
        }
    }

    [JsonConverter(typeof(ITemporalLiteralConverter))]
    public interface ITemporalLiteral : ITemporalExpression
    {
        InstantLiteral AsInstantLiteral();
        IntervalLiteral AsIntervalLiteral();
    }

    [JsonConverter(typeof(IsLikePredicateConverter))]
    public class IsLikePredicate : ComparisonPredicate
    {
        [Newtonsoft.Json.JsonProperty("op", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public IsLikePredicateOp Op { get; set; }

        [Newtonsoft.Json.JsonProperty("args", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(2)]
        [System.ComponentModel.DataAnnotations.MaxLength(2)]
        public IsLikeOperands Args { get; set; } = new IsLikeOperands();

        public override AndOrExpression AsAndOrExpression()
        {
            return null;
        }

        public override ArrayPredicate AsArrayPredicate()
        {
            return null;
        }

        public override BinaryComparisonPredicate AsBinaryComparison()
        {
            return null;
        }

        public override ComparisonPredicate AsComparison()
        {
            return this;
        }

        public override IsBetweenPredicate AsIsBetweenPredicate()
        {
            return null;
        }

        public override IsInListPredicate AsIsInListPredicate()
        {
            return null;
        }

        public override IsLikePredicate AsIsLikePredicate()
        {
            return this;
        }

        public override IsNullPredicate AsIsNullPredicate()
        {
            return null;
        }

        public override NotExpression AsNotExpression()
        {
            return null;
        }

        public override SpatialPredicate AsSpatialPredicate()
        {
            return null;
        }

        public override TemporalPredicate AsTemporalPredicate()
        {
            return null;
        }

    }

    public class IsLikeOperands : System.Collections.ObjectModel.Collection<CharExpression>
    {

    }

    public interface IPatternExpression
    {
    }

    [JsonConverter(typeof(IsBetweenPredicateConverter))]
    public class IsBetweenPredicate : ComparisonPredicate
    {
        [Newtonsoft.Json.JsonProperty("op", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public IsBetweenPredicateOp Op { get; set; }

        [Newtonsoft.Json.JsonProperty("args", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(3)]
        [System.ComponentModel.DataAnnotations.MaxLength(3)]
        public IsBetweenOperands Args { get; set; } = new IsBetweenOperands();

        public override AndOrExpression AsAndOrExpression()
        {
            return null;
        }

        public override ArrayPredicate AsArrayPredicate()
        {
            return null;
        }

        public override BinaryComparisonPredicate AsBinaryComparison()
        {
            return null;
        }

        public override ComparisonPredicate AsComparison()
        {
            return this;
        }

        public override IsBetweenPredicate AsIsBetweenPredicate()
        {
            return this;
        }

        public override IsInListPredicate AsIsInListPredicate()
        {
            return null;
        }

        public override IsLikePredicate AsIsLikePredicate()
        {
            return null;
        }

        public override IsNullPredicate AsIsNullPredicate()
        {
            return null;
        }

        public override NotExpression AsNotExpression()
        {
            return null;
        }

        public override SpatialPredicate AsSpatialPredicate()
        {
            return null;
        }

        public override TemporalPredicate AsTemporalPredicate()
        {
            return null;
        }
    }

    public class IsBetweenOperands : System.Collections.ObjectModel.Collection<INumericExpression>
    {

    }

    [JsonConverter(typeof(INumericExpressionConverter))]
    public interface INumericExpression : IIsNullOperand
    {
    }

    public static class INumericExpressionExtensions
    {
        public static Number AsNumber(this INumericExpression expr) => expr as Number;
        public static PropertyRef AsPropertyRef(this INumericExpression expr) => expr as PropertyRef;
        public static FunctionRef AsFunctionRef(this INumericExpression expr) => expr as FunctionRef;
    }

    [JsonConverter(typeof(IsInListPredicateConverter))]
    public class IsInListPredicate : ComparisonPredicate
    {
        [Newtonsoft.Json.JsonProperty("op", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public IsInListPredicateOp Op { get; set; }

        [Newtonsoft.Json.JsonProperty("args", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public InListOperands Args { get; set; } = new InListOperands();

        public override AndOrExpression AsAndOrExpression()
        {
            return null;
        }

        public override ArrayPredicate AsArrayPredicate()
        {
            return null;
        }

        public override BinaryComparisonPredicate AsBinaryComparison()
        {
            return null;
        }

        public override ComparisonPredicate AsComparison()
        {
            return this;
        }

        public override IsBetweenPredicate AsIsBetweenPredicate()
        {
            return null;
        }

        public override IsInListPredicate AsIsInListPredicate()
        {
            return this;
        }

        public override IsLikePredicate AsIsLikePredicate()
        {
            return null;
        }

        public override IsNullPredicate AsIsNullPredicate()
        {
            return null;
        }

        public override NotExpression AsNotExpression()
        {
            return null;
        }

        public override SpatialPredicate AsSpatialPredicate()
        {
            return null;
        }

        public override TemporalPredicate AsTemporalPredicate()
        {
            return null;
        }
    }

    public class InListOperands : System.Collections.ObjectModel.Collection<IInListOperand>
    {
        public IInListOperand Left => this[0];

        public IInListOperand Right => this[1];
    }

    [JsonConverter(typeof(IIsInListOperandConverter))]
    public interface IInListOperand
    {
        IScalarExpression AsScalarExpression();

        ScalarExpressionCollection AsScalarExpressionCollection();
    }

    [JsonConverter(typeof(IsNullPredicateConverter))]
    public class IsNullPredicate : ComparisonPredicate
    {
        [Newtonsoft.Json.JsonProperty("op", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public IsNullPredicateOp Op { get; set; }

        [Newtonsoft.Json.JsonProperty("args", Required = Newtonsoft.Json.Required.Always)]
        public IIsNullOperand Args { get; set; }

        public override AndOrExpression AsAndOrExpression()
        {
            return null;
        }

        public override ArrayPredicate AsArrayPredicate()
        {
            return null;
        }

        public override BinaryComparisonPredicate AsBinaryComparison()
        {
            return null;
        }

        public override ComparisonPredicate AsComparison()
        {
            return this;
        }

        public override IsBetweenPredicate AsIsBetweenPredicate()
        {
            return null;
        }

        public override IsInListPredicate AsIsInListPredicate()
        {
            return null;
        }

        public override IsLikePredicate AsIsLikePredicate()
        {
            return null;
        }

        public override IsNullPredicate AsIsNullPredicate()
        {
            return this;
        }

        public override NotExpression AsNotExpression()
        {
            return null;
        }

        public override SpatialPredicate AsSpatialPredicate()
        {
            return null;
        }

        public override TemporalPredicate AsTemporalPredicate()
        {
            return null;
        }
    }

    [JsonConverter(typeof(IIsNullOperandConverter))]
    public interface IIsNullOperand
    {
        public CharExpression AsCharExpression();
        public Number AsNumeric();
        public ITemporalExpression AsTemporalExpression();
        public BooleanExpression AsBooleanExpression();
        public IGeomExpression AsGeomExpression();
    }

    [JsonConverter(typeof(SpatialPredicateConverter))]
    public class SpatialPredicate : ComparisonPredicate
    {
        [Newtonsoft.Json.JsonProperty("op", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public SpatialPredicateOp Op { get; set; }

        [Newtonsoft.Json.JsonProperty("args", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(2)]
        [System.ComponentModel.DataAnnotations.MaxLength(2)]
        public SpatialOperands Args { get; set; } = new SpatialOperands();

        public override AndOrExpression AsAndOrExpression()
        {
            return null;
        }

        public override ArrayPredicate AsArrayPredicate()
        {
            return null;
        }

        public override BinaryComparisonPredicate AsBinaryComparison()
        {
            return null;
        }

        public override ComparisonPredicate AsComparison()
        {
            return this;
        }

        public override IsBetweenPredicate AsIsBetweenPredicate()
        {
            return null;
        }

        public override IsInListPredicate AsIsInListPredicate()
        {
            return null;
        }

        public override IsLikePredicate AsIsLikePredicate()
        {
            return null;
        }

        public override IsNullPredicate AsIsNullPredicate()
        {
            return null;
        }

        public override NotExpression AsNotExpression()
        {
            return null;
        }

        public override SpatialPredicate AsSpatialPredicate()
        {
            return this;
        }

        public override TemporalPredicate AsTemporalPredicate()
        {
            return null;
        }
    }

    public class SpatialOperands : System.Collections.ObjectModel.Collection<IGeomExpression>
    {

    }

    [JsonConverter(typeof(SpatialLiteralConverter))]
    public interface ISpatialLiteral : IGeomExpression
    {
        public GeometryLiteral AsGeometryLiteral();
        public EnvelopeLiteral AsEnvelopeLiteral();
    }

    [JsonConverter(typeof(TemporalPredicateConverter))]
    public class TemporalPredicate : ComparisonPredicate
    {
        [Newtonsoft.Json.JsonProperty("op", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public TemporalPredicateOp Op { get; set; }

        [Newtonsoft.Json.JsonProperty("args", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(2)]
        [System.ComponentModel.DataAnnotations.MaxLength(2)]
        public TemporalOperands Args { get; set; } = new TemporalOperands();

        public override AndOrExpression AsAndOrExpression()
        {
            return null;
        }

        public override ArrayPredicate AsArrayPredicate()
        {
            return null;
        }

        public override BinaryComparisonPredicate AsBinaryComparison()
        {
            return null;
        }

        public override ComparisonPredicate AsComparison()
        {
            return this;
        }

        public override IsBetweenPredicate AsIsBetweenPredicate()
        {
            return null;
        }

        public override IsInListPredicate AsIsInListPredicate()
        {
            return null;
        }

        public override IsLikePredicate AsIsLikePredicate()
        {
            return null;
        }

        public override IsNullPredicate AsIsNullPredicate()
        {
            return null;
        }

        public override NotExpression AsNotExpression()
        {
            return null;
        }

        public override SpatialPredicate AsSpatialPredicate()
        {
            return null;
        }

        public override TemporalPredicate AsTemporalPredicate()
        {
            return this;
        }
    }

    public partial class TemporalOperands : System.Collections.ObjectModel.Collection<ITemporalExpression>
    {

    }

    [JsonConverter(typeof(TemporalInstantExpressionConverter))]
    public interface ITemporalInstantExpression : IScalarExpression
    {
        string ToString();

        DateTimeOffset DateTime { get; }
    }

    public class ArrayPredicate : BooleanExpression
    {
        [Newtonsoft.Json.JsonProperty("op", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ArrayPredicateOp Op { get; set; }

        [Newtonsoft.Json.JsonProperty("args", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(2)]
        [System.ComponentModel.DataAnnotations.MaxLength(2)]
        public ArrayExpression Args { get; set; } = new ArrayExpression();

        public override AndOrExpression AsAndOrExpression()
        {
            return null;
        }

        public override ArrayPredicate AsArrayPredicate()
        {
            return this;
        }

        public override ComparisonPredicate AsComparison()
        {
            return null;
        }

        public override NotExpression AsNotExpression()
        {
            return null;
        }

        public override SpatialPredicate AsSpatialPredicate()
        {
            return null;
        }

        public override TemporalPredicate AsTemporalPredicate()
        {
            return null;
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ArrayExpression : System.Collections.ObjectModel.Collection<PropertyRef>
    {

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ArrayLiteral : System.Collections.ObjectModel.Collection<IScalarExpression>
    {

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ArithmeticOperands : System.Collections.ObjectModel.Collection<INumericExpression>
    {

    }

    [JsonConverter(typeof(BoolExpressionConverter))]
    public class BoolExpression : IScalarExpression, IScalarLiteral
    {
        public BoolExpression(bool value)
        {
            Bool = value;
        }

        public bool Bool { get; set; }

        public IComparable Value => Bool;

        public BooleanExpression AsBooleanExpression()
        {
            return null;
        }

        public CharExpression AsCharExpression()
        {
            return new String(Bool.ToString());
        }

        public INumericExpression AsNumericExpression()
        {
            return new Number(Bool ? 1 : 0);
        }

        public IScalarExpression AsScalarExpression()
        {
            return this;
        }

        public ScalarExpressionCollection AsScalarExpressionCollection()
        {
            return null;
        }

        public ITemporalInstantExpression AsTemporalInstant()
        {
            return null;
        }
    }

    [JsonConverter(typeof(NumberConverter))]
    public class Number : IScalarExpression, INumericExpression, IScalarLiteral
    {
        public Number(double value)
        {
            Num = value;
        }

        public double Num { get; set; }

        public IComparable Value => Num;

        public string ToString()
        {
            return Num.ToString();
        }

        public BooleanExpression AsBooleanExpression()
        {
            return null;
        }

        public CharExpression AsCharExpression()
        {
            return null;
        }

        public IGeomExpression AsGeomExpression()
        {
            return null;
        }

        public INumericExpression AsNumericExpression()
        {
            return this;
        }

        public IScalarExpression AsScalarExpression()
        {
            return this;
        }

        public ScalarExpressionCollection AsScalarExpressionCollection()
        {
            return null;
        }

        public ITemporalExpression AsTemporalExpression()
        {
            return null;
        }

        public ITemporalInstantExpression AsTemporalInstant()
        {
            return null;
        }

        Number IIsNullOperand.AsNumeric()
        {
            return this;
        }
    }

    [JsonConverter(typeof(PropertyRefConverter))]
    public class PropertyRef : CharExpression, INumericExpression, IIsNullOperand, IGeomExpression, ITemporalExpression
    {
        public PropertyRef()
        {
        }

        public PropertyRef(string property)
        {
            Property = property;
        }

        [Newtonsoft.Json.JsonProperty("property", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Property { get; set; }

        public override AccentiExpression AsAccenti()
        {
            return null;
        }

        public override CaseiExpression AsCasei()
        {
            return null;
        }

        public override FunctionRef AsFunctionRef()
        {
            return null;
        }

        PropertyRef IGeomExpression.AsPropertyRef()
        {
            return this;
        }

        PropertyRef ITemporalExpression.AsPropertyRef()
        {
            return this;
        }

        public ISpatialLiteral AsSpatialLiteral()
        {
            return null;
        }

        public override String AsString()
        {
            return null;
        }

        public ITemporalLiteral AsTemporalLiteral()
        {
            return null;
        }

        public override PropertyRef AsPropertyRef()
        {
            return this;
        }
    }

    public class AccentiExpression : CharExpression, IPatternExpression
    {
        [Newtonsoft.Json.JsonProperty("accenti", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public CharExpression Accenti { get; set; }

        public override AccentiExpression AsAccenti()
        {
            return this;
        }

        public override CaseiExpression AsCasei()
        {
            return null;
        }

        public override FunctionRef AsFunctionRef()
        {
            return null;
        }

        public override PropertyRef AsPropertyRef()
        {
            return null;
        }

        public override String AsString()
        {
            return null;
        }
    }

    public partial class FunctionRef : CharExpression, INumericExpression, IGeomExpression, ITemporalExpression
    {
        [Newtonsoft.Json.JsonProperty("function", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Function Function { get; set; } = new Function();

        [JsonIgnore]
        public double? Number => null;

        public override AccentiExpression AsAccenti()
        {
            return null;
        }

        public override CaseiExpression AsCasei()
        {
            return null;
        }

        public override FunctionRef AsFunctionRef()
        {
            return this;
        }

        public override PropertyRef AsPropertyRef()
        {
            return null;
        }

        public ISpatialLiteral AsSpatialLiteral()
        {
            return null;
        }

        public override String AsString()
        {
            return null;
        }

        public ITemporalLiteral AsTemporalLiteral()
        {
            return null;
        }
    }

    public class Function
    {
        [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("args", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.List<IScalarExpression> Args { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

    }

    public interface IScalarLiteral : IScalarExpression
    {
        IComparable Value { get; }
    }

    [JsonConverter(typeof(GeometryStringConverter))]
    public class GeometryLiteral : IGeometryObject, ISpatialLiteral
    {
        public GeometryLiteral(IGeometryObject value)
        {
            GeometryObject = value;
        }

        public GeoJSONObjectType Type => GeometryObject.Type;

        public IGeometryObject GeometryObject { get; private set; }

        public BooleanExpression AsBooleanExpression()
        {
            return null;
        }

        public CharExpression AsCharExpression()
        {
            return null;
        }

        public EnvelopeLiteral AsEnvelopeLiteral()
        {
            return null;
        }

        public FunctionRef AsFunctionRef()
        {
            return null;
        }

        public IGeomExpression AsGeomExpression()
        {
            return null;
        }

        public Number AsNumeric()
        {
            return null;
        }

        public PropertyRef AsPropertyRef()
        {
            return null;
        }

        public ISpatialLiteral AsSpatialLiteral()
        {
            return this;
        }

        public ITemporalExpression AsTemporalExpression()
        {
            return null;
        }

        GeometryLiteral ISpatialLiteral.AsGeometryLiteral()
        {
            return this;
        }
    }

    public partial class EnvelopeLiteral : ISpatialLiteral
    {
        [Newtonsoft.Json.JsonProperty("bbox", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Bbox Bbox { get; set; }

        public BooleanExpression AsBooleanExpression()
        {
            return null;
        }

        public CharExpression AsCharExpression()
        {
            return null;
        }

        public FunctionRef AsFunctionRef()
        {
            return null;
        }

        public IGeomExpression AsGeomExpression()
        {
            return null;
        }

        public GeometryLiteral AsGeometryLiteral()
        {
            return null;
        }

        public Number AsNumeric()
        {
            return null;
        }

        public PropertyRef AsPropertyRef()
        {
            return null;
        }

        public ISpatialLiteral AsSpatialLiteral()
        {
            return this;
        }

        public ITemporalExpression AsTemporalExpression()
        {
            return null;
        }

        EnvelopeLiteral ISpatialLiteral.AsEnvelopeLiteral()
        {
            return this;
        }

        public string ToString()
        {
            return $"{Bbox[0]},{Bbox[1]},{Bbox[2]},{Bbox[3]}";
        }
    }

    [JsonConverter(typeof(BboxConverter))]
    public partial class Bbox : System.Collections.Generic.List<double>
    {
        [JsonConstructor]
        public Bbox(double minX, double minY, double maxX, double maxY)
        {
            this.Add(minX);
            this.Add(minY);
            this.Add(maxX);
            this.Add(maxY);
        }

        public Bbox(double minX, double minY, double minZ, double maxX, double maxY, double maxZ)
        {
            this.Add(minX);
            this.Add(minY);
            this.Add(minZ);
            this.Add(maxX);
            this.Add(maxY);
            this.Add(maxZ);
        }
    }

    public class DateLiteral : InstantLiteral
    {
        [Newtonsoft.Json.JsonProperty("date", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public override DateTimeOffset DateTime { get; set; }
    }

    [JsonConverter(typeof(DateStringConverter))]
    public class DateString : IInstantString
    {
        public DateString(DateTimeOffset date)
        {
            Date = date;
        }

        public System.DateTimeOffset Date { get; }

        public DateTime DateTime => Date.DateTime;

        public static DateString Parse(string v)
        {
            return new DateString(DateTimeOffset.ParseExact(v, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.AssumeUniversal));
        }

        public override string ToString()
        {
            return Date.ToUniversalTime().ToString("yyyy-MM-dd");
        }
    }

    [JsonConverter(typeof(TimestampLiteralConverter))]
    public class TimestampLiteral : InstantLiteral
    {
        public TimestampLiteral(DateTime value)
        {
            DateTime = value;
        }

        [Newtonsoft.Json.JsonProperty("timestamp", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public override DateTimeOffset DateTime { get; set; }
    }

    [JsonConverter(typeof(TimestampStringConverter))]
    public class TimestampString : IInstantString
    {
        public TimestampString(DateTimeOffset timestamp)
        {
            Timestamp = timestamp;
        }

        public System.DateTimeOffset Timestamp { get; }

        public DateTime DateTime => Timestamp.DateTime;

        public static TimestampString Parse(string v)
        {
            return new TimestampString(DateTimeOffset.Parse(v, null, System.Globalization.DateTimeStyles.AssumeUniversal));
        }

        public override string ToString()
        {
            return Timestamp.ToUniversalTime().ToString("O");
        }

    }

    public interface IInstantString : IIntervalItem
    {
        string ToString();
    }

    [JsonConverter(typeof(IntervalLiteralConverter))]
    public class IntervalLiteral : ITemporalLiteral
    {
        [Newtonsoft.Json.JsonProperty("interval", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(2)]
        [System.ComponentModel.DataAnnotations.MaxLength(2)]
        public IntervalArray Interval { get; set; } = new IntervalArray();

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        public IntervalLiteral(IntervalArray intervalArray)
        {
            Interval = intervalArray;
        }

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

        [JsonIgnore]
        public Itenso.TimePeriod.ITimeInterval TimeInterval
        {
            get
            {
                return new Itenso.TimePeriod.TimeInterval(Interval[0].DateTime, Interval[1].DateTime);
            }
        }

        public InstantLiteral AsInstantLiteral()
        {
            return null;
        }

        IntervalLiteral ITemporalLiteral.AsIntervalLiteral()
        {
            return this;
        }

        public ITemporalLiteral AsTemporalLiteral()
        {
            return this;
        }

        public PropertyRef AsPropertyRef()
        {
            return null;
        }

        public FunctionRef AsFunctionRef()
        {
            return null;
        }

        public CharExpression AsCharExpression()
        {
            return null;
        }

        public Number AsNumeric()
        {
            return null;
        }

        public ITemporalExpression AsTemporalExpression()
        {
            return null;
        }

        public BooleanExpression AsBooleanExpression()
        {
            return null;
        }

        public IGeomExpression AsGeomExpression()
        {
            return null;
        }
    }

    public partial class IntervalArray : System.Collections.ObjectModel.Collection<IIntervalItem>
    {

    }


    [JsonConverter(typeof(IIntervalItemConverter))]
    public interface IIntervalItem
    {
        DateTime DateTime { get; }
    }

    public enum StringIntervalItemEnum
    {
        [System.Runtime.Serialization.EnumMember(Value = @"..")]
        DotDot = 0
    }

    public class StringIntervalItem
    {
        public StringIntervalItemEnum value { get; set; }

        public StringIntervalItem(StringIntervalItemEnum v)
        {
            this.value = v;
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AndOrExpressionOp
    {

        [System.Runtime.Serialization.EnumMember(Value = @"and")]
        And = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"or")]
        Or = 1,

    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum NotExpressionOp
    {

        [System.Runtime.Serialization.EnumMember(Value = @"not")]
        Not = 0,

    }

    [JsonConverter(typeof(StringEnumConverter))]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum ComparisonPredicateOp
    {

        [System.Runtime.Serialization.EnumMember(Value = @"=")]
        Eq = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"<")]
        Lt = 1,

        [System.Runtime.Serialization.EnumMember(Value = @">")]
        Gt = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"<=")]
        Le = 3,

        [System.Runtime.Serialization.EnumMember(Value = @">=")]
        Ge = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"<>")]
        Diff = 5,

    }

    public class ScalarExpressionCollection : System.Collections.ObjectModel.Collection<IScalarExpression>, IInListOperand
    {
        public ScalarExpressionCollection(IList<IScalarExpression> list) : base(list)
        {
        }

        public IScalarExpression AsScalarExpression()
        {
            return null;
        }

        public ScalarExpressionCollection AsScalarExpressionCollection()
        {
            return this;
        }
    }

    [JsonConverter(typeof(ScalarExpressionConverter))]
    public interface IScalarExpression : IInListOperand
    {
        public CharExpression AsCharExpression();
        public ITemporalInstantExpression AsTemporalInstant();
        public INumericExpression AsNumericExpression();
        public BooleanExpression AsBooleanExpression();
        public string ToString();
    }

    [JsonConverter(typeof(StringEnumConverter))]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum IsLikePredicateOp
    {

        [System.Runtime.Serialization.EnumMember(Value = @"like")]
        Like = 0,

    }

    [JsonConverter(typeof(StringEnumConverter))]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum IsBetweenPredicateOp
    {

        [System.Runtime.Serialization.EnumMember(Value = @"between")]
        Between = 0,

    }

    [JsonConverter(typeof(StringEnumConverter))]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum NumericExpressionOp
    {

        [System.Runtime.Serialization.EnumMember(Value = @"+")]
        Plus = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"-")]
        Minus = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"*")]
        Times = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"/")]
        Divides = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"^")]
        Pow = 4,

    }

    [JsonConverter(typeof(StringEnumConverter))]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum IsInListPredicateOp
    {

        [System.Runtime.Serialization.EnumMember(Value = @"in")]
        In = 0,

    }

    [JsonConverter(typeof(StringEnumConverter))]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum IsNullPredicateOp
    {

        [System.Runtime.Serialization.EnumMember(Value = @"isNull")]
        IsNull = 0,

    }

    [JsonConverter(typeof(StringEnumConverter))]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum SpatialPredicateOp
    {

        [System.Runtime.Serialization.EnumMember(Value = @"s_contains")]
        S_contains = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"s_crosses")]
        S_crosses = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"s_disjoint")]
        S_disjoint = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"s_equals")]
        S_equals = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"s_intersects")]
        S_intersects = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"s_overlaps")]
        S_overlaps = 5,

        [System.Runtime.Serialization.EnumMember(Value = @"s_touches")]
        S_touches = 6,

        [System.Runtime.Serialization.EnumMember(Value = @"s_within")]
        S_within = 7,

    }

    [JsonConverter(typeof(IGeomExpressionConverter))]
    public interface IGeomExpression : IIsNullOperand
    {
        public ISpatialLiteral AsSpatialLiteral();
        public PropertyRef AsPropertyRef();
        public FunctionRef AsFunctionRef();
    }

    [JsonConverter(typeof(StringEnumConverter))]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum TemporalPredicateOp
    {

        [System.Runtime.Serialization.EnumMember(Value = @"t_after")]
        T_after = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"t_before")]
        T_before = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"t_contains")]
        T_contains = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"t_disjoint")]
        T_disjoint = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"t_during")]
        T_during = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"t_equals")]
        T_equals = 5,

        [System.Runtime.Serialization.EnumMember(Value = @"t_finishedBy")]
        T_finishedBy = 6,

        [System.Runtime.Serialization.EnumMember(Value = @"t_finishes")]
        T_finishes = 7,

        [System.Runtime.Serialization.EnumMember(Value = @"t_intersects")]
        T_intersects = 8,

        [System.Runtime.Serialization.EnumMember(Value = @"t_meets")]
        T_meets = 9,

        [System.Runtime.Serialization.EnumMember(Value = @"t_metBy")]
        T_metBy = 10,

        [System.Runtime.Serialization.EnumMember(Value = @"t_overlappedBy")]
        T_overlappedBy = 11,

        [System.Runtime.Serialization.EnumMember(Value = @"t_overlaps")]
        T_overlaps = 12,

        [System.Runtime.Serialization.EnumMember(Value = @"t_startedBy")]
        T_startedBy = 13,

        [System.Runtime.Serialization.EnumMember(Value = @"t_starts")]
        T_starts = 14,

    }

    [JsonConverter(typeof(ITemporalExpressionConverter))]
    public interface ITemporalExpression : IIsNullOperand
    {
        public ITemporalLiteral AsTemporalLiteral();
        public PropertyRef AsPropertyRef();
        public FunctionRef AsFunctionRef();
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum ArrayPredicateOp
    {

        [System.Runtime.Serialization.EnumMember(Value = @"a_containedBy")]
        A_containedBy = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"a_contains")]
        A_contains = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"a_equals")]
        A_equals = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"a_overlaps")]
        A_overlaps = 3,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum GeometryLiteralType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Point")]
        Point = 0,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum LinestringType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"LineString")]
        LineString = 0,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum PolygonType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Polygon")]
        Polygon = 0,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum MultipointType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"MultiPoint")]
        MultiPoint = 0,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum MultilinestringType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"MultiLineString")]
        MultiLineString = 0,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum MultipolygonType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"MultiPolygon")]
        MultiPolygon = 0,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal class DateFormatConverter : Newtonsoft.Json.Converters.IsoDateTimeConverter
    {
        public DateFormatConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }


}

#pragma warning restore 1591
#pragma warning restore 1573
#pragma warning restore 472
#pragma warning restore 114
#pragma warning restore 108
#pragma warning restore 3016
#pragma warning restore 8603