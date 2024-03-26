using System.IO;

namespace Shared.Utils
{
    public interface IFilesStorage
    {
        FileAccess Access { get; }
        IFileDescriptor[] GetFiles();
        IFileDescriptor AddFile(string relativeFilePath, Stream content);
        IFileDescriptor FindFile(string relativePath);
        void DeleteFile(IFileDescriptor toDelete);
        void UpdateFile(IFileDescriptor existing, Stream newContent);
    }
}