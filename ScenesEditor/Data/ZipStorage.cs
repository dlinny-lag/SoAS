using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security;
using System.Text;
using Shared.Utils;

namespace ScenesEditor.Data
{
    [DebuggerDisplay("{zipFilePath}")]
    public sealed class ZipStorage : IFilesStorage, IDisposable
    {
        /// <summary>
        /// For the text files
        /// </summary>
        public Encoding Encoding => Encoding.UTF8;

        private readonly string zipFilePath;
        private bool disposed = false;
        private FileStream zipFileStream;
        private ZipArchive archive;
        public ZipStorage(string path, bool readOnly)
        {
            Access = readOnly ? FileAccess.Read : FileAccess.ReadWrite;
            if (readOnly && !File.Exists(path))
                throw new ArgumentOutOfRangeException(nameof(path), $@"File {path} does not exist");
            zipFilePath = path;
        }

        public string Path => zipFilePath;
        public FileAccess Access { get; }

        void EnsureInitialized()
        {
            if (zipFileStream != null)
                return;
            if (disposed)
                throw new ObjectDisposedException($@"{nameof(ZipStorage)} for {zipFilePath} is disposed");

            bool canWrite = Access.HasFlag(FileAccess.Write);

            bool fileExist = File.Exists(zipFilePath);

            zipFileStream = new FileStream(zipFilePath, 
                !fileExist? FileMode.Create : FileMode.Open, 
                canWrite  ? FileAccess.ReadWrite : FileAccess.Read, 
                canWrite  ? FileShare.None : FileShare.Read);

            if (!fileExist)
            {
                (new ZipArchive(zipFileStream, ZipArchiveMode.Create, true)).Dispose();
            }

            archive = new ZipArchive(zipFileStream, canWrite?ZipArchiveMode.Update:ZipArchiveMode.Read, true);
        }

        internal Stream GetReadStream(string filePath)
        {
            EnsureInitialized();
            var entry = archive.GetEntry(filePath);
            if (entry == null)
                throw new FileNotFoundException($@"File {filePath} not found in {zipFilePath}");
            return entry.Open();
        }

        public IFileDescriptor[] GetFiles()
        {
            if (Access.HasFlag(FileAccess.Write) && !File.Exists(zipFilePath))
                return Array.Empty<IFileDescriptor>();
            
            EnsureInitialized();
            return archive.Entries.Select(e => new ZippedFileDescriptor(this, e.FullName)).ToArray<IFileDescriptor>();
        }
        public IFileDescriptor AddFile(string relativeFilePath, Stream content)
        {
            if (!Access.HasFlag(FileAccess.Write))
                throw new SecurityException("Storage is in readonly mode");

            EnsureInitialized();
            if (archive.GetEntry(relativeFilePath) != null)
                throw new DuplicateNameException($@"{relativeFilePath} already exist");

            var entry = archive.CreateEntry(relativeFilePath);
            using (var stream = entry.Open())
            {
                content.CopyTo(stream);
            }

            return new ZippedFileDescriptor(this, entry.FullName);
        }

        public IFileDescriptor FindFile(string relativePath)
        {
            EnsureInitialized();
            var entry = archive.GetEntry(relativePath);
            if (entry == null)
                return null;
            return new ZippedFileDescriptor(this, relativePath);
        }

        public void DeleteFile(IFileDescriptor toDelete)
        {
            if (!Access.HasFlag(FileAccess.Write))
                throw new SecurityException("Storage is in readonly mode");

            EnsureInitialized();
            var entry = archive.GetEntry(toDelete.FullName);
            entry?.Delete();
        }

        public void UpdateFile(IFileDescriptor existing, Stream newContent)
        {
            if (!Access.HasFlag(FileAccess.Write))
                throw new SecurityException("Storage is in readonly mode");

            EnsureInitialized();
            var entry = archive.GetEntry(existing.FullName);
            if (entry == null)
                throw new FileNotFoundException($@"File {existing.FullName} not found in {zipFilePath}");
            using (var stream = entry.Open())
            {
                newContent.CopyTo(stream);
            }
            entry.LastWriteTime = DateTimeOffset.Now;
        }

        public void Dispose()
        {
            if (disposed)
                return;
            disposed = true;

            archive?.Dispose();
            archive = null;
            zipFileStream?.Flush(true);
            zipFileStream?.Dispose();
            zipFileStream = null;
        }
    }
}