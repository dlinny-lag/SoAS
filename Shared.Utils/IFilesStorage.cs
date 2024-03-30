using System.IO;
using System.Text;

namespace Shared.Utils
{
    public interface IFilesStorage
    {
        Encoding Encoding { get; }
        string Path { get; }
        FileAccess Access { get; }
        IFileDescriptor[] GetFiles();
        IFileDescriptor AddFile(string relativeFilePath, Stream content);
        IFileDescriptor FindFile(string relativePath);
        void DeleteFile(IFileDescriptor toDelete);
        void UpdateFile(IFileDescriptor existing, Stream newContent);
    }
}