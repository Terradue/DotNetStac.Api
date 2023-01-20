namespace Stac.Api.Interfaces
{
    public interface IPaginationParameters
    {
        int Limit { get; }
        int Page { get; }
        int StartIndex { get; }
    }
}