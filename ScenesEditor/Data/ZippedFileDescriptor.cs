using System;
using System.IO;
using Shared.Utils;

namespace ScenesEditor.Data
{
    internal class ZippedFileDescriptor : IFileDescriptor
    {
        private readonly ZipStorage parent;
        private readonly string filePath;
        public ZippedFileDescriptor(ZipStorage storage, string path)
        {
            parent = storage ?? throw new ArgumentNullException(nameof(storage));
            filePath = path ?? throw new ArgumentNullException(nameof(path));
        }

        public string Name => Path.GetFileName(filePath);
        public string FullName => filePath;
        public string ReadAllText()
        {
            using (var reader = new StreamReader(parent.GetReadStream(filePath), parent.Encoding))
            {
                return reader.ReadToEnd();
            }
        }
    }
}