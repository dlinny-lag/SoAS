namespace Shared.Utils
{
    public interface IFileDescriptor
    {
        string Name { get; }
        string FullName { get; }
        string ReadAllText();
    }
}