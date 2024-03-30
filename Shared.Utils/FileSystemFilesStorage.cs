using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;

namespace Shared.Utils
{
    public class FileSystemFilesStorage : IFilesStorage
    {
        public Encoding Encoding => Encoding.UTF8;

        private readonly DirectoryInfo folderInfo;
        public FileSystemFilesStorage(string folderPath, bool readOnly = true)
        {
            folderInfo = new DirectoryInfo(folderPath);
            Access = readOnly ? FileAccess.Read : FileAccess.ReadWrite;
        }

        public string Path => folderInfo.FullName;

        public FileAccess Access { get; }

        public IFileDescriptor[] GetFiles()
        {
            return folderInfo.GetFiles().Select(fi => new FileSystemFileDescriptor(fi)).ToArray<IFileDescriptor>();
        }

        public IFileDescriptor AddFile(string relativeFilePath, Stream content)
        {
            if (!Access.HasFlag(FileAccess.Write))
                throw new SecurityException("Storage is in readonly mode");

            string fullPath = System.IO.Path.Combine(folderInfo.FullName, relativeFilePath);
            SaveFile(content, fullPath);
            return new FileSystemFileDescriptor(new FileInfo(fullPath));
        }

        private static void SaveFile(Stream content, string fullPath)
        {
            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
                content.CopyTo(fs);
            }
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
            File.Delete(toDelete.FullName);
        }

        public void UpdateFile(IFileDescriptor existing, Stream newContent)
        {
            if (!Access.HasFlag(FileAccess.Write))
                throw new SecurityException("Storage is in readonly mode");
            // TODO: potential inconsistency: existing may come from another storage
            SaveFile(newContent, existing.FullName);
        }
    }
}