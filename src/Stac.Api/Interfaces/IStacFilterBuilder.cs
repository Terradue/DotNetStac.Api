namespace Stac.Api.Interfaces
{
    public interface IStacFilterBuilder
    {
        IStacFilter CreateFilter(double[] bboxArray, string datetime);
    }
}