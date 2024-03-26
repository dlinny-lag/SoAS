using System.Collections.Generic;
using System.Linq;

namespace Shared.Controls
{
    public class FileStringFailureList : FailedFilesList<IList<string>>
    {
        protected override IList<FailInfo> GetData(IDictionary<string, IList<string>> errors)
        {
            return errors.Select(p => new FailInfo
            {
                File = p.Key,
                Error =  string.Join("\n", p.Value),
            }).ToArray();
        }
    }
}