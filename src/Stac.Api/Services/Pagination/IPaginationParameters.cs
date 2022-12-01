namespace Stac.Api.Services.Pagination
{
    public interface IPaginationParameters
    {
        int Limit { get; }
        int Page { get; }
        int StartIndex { get; }
    }
}