using System;
using System.IO;
using System.Linq;
using System.Security;

namespace Shared.Utils
{
    public class FileSystemFilesStorage : IFilesStorage
    {
        private readonly DirectoryInfo folderInfo;
        public FileSystemFilesStorage(string folderPath, bool readOnly = true)
        {
            folderInfo = new DirectoryInfo(folderPath);
            Access = readOnly ? FileAccess.Read : FileAccess.ReadWrite;
        }

        public FileAccess Access { get; }

        public IFileDescriptor[] GetFiles()
        {
            return folderInfo.GetFiles().Select(fi => new FileSystemFileDescriptor(fi)).ToArray<IFileDescriptor>();
        }

        public IFileDescriptor AddFile(string relativeFilePath, Stream content)
        {
            if (!Access.HasFlag(FileAccess.Write))
                throw new SecurityException("Storage is in readonly mode");
            throw new System.NotImplementedException();
        }

        public IFileDescriptor FindFile(string relativePath)
        {
            var files = folderInfo.GetFiles(relativePath);
            if (files.Length == 0)
                return null;
            if (files.Length == 1)
                return new FileSystemFileDescriptor(files[0]);
            throw new InvalidOperationException("More than one matches found");
        }

        public void DeleteFile(IFileDescriptor toDelete)
        {
            if (!Access.HasFlag(FileAccess.Write))
                throw new SecurityException("Storage is in readonly mode");
            throw new System.NotImplementedException();
        }

        public void UpdateFile(IFileDescriptor existing, Stream newContent)
        {
            if (!Access.HasFlag(FileAccess.Write))
                throw new SecurityException("Storage is in readonly mode");
            throw new System.NotImplementedException();
        }
    }
}