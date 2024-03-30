using System.IO;

namespace Shared.Utils
{
    internal class FileSystemFileDescriptor : IFileDescriptor
    {
        public FileSystemFileDescriptor(FileInfo info)
        {
            Name = info.Name;
            FullName = info.FullName;
        }
        public string Name { get; }
        public string FullName { get; }

        public string ReadAllText()
        {
            return File.ReadAllText(FullName);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}