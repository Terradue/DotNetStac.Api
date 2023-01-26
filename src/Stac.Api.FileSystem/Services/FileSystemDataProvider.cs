using Stac.Api.Interfaces;

namespace Stac.Api.FileSystem.Services
{
    public abstract class FileSystemDataProvider<T> : IDataProvider<T> where T : IStacObject
    {
    }
}