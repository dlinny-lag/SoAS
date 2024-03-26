using System.Collections.Generic;
using System.Linq;
using AAFModel;

namespace Shared.Controls
{
    public sealed class FileLoadFailureList : FailedFilesList<LoadException>
    {
        protected override IList<FailInfo> GetData(IDictionary<string, LoadException> errors)
        {
            return errors.Select(p => new FailInfo
            {
                File = p.Key,
                Error = p.Value.InnerException.Message // same as in FileExport.ExportText. TODO: avoid duplication
            }).ToArray();
        }
    }
}