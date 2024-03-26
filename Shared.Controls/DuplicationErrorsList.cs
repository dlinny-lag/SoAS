using System;
using AAF.Services.AAFImport;
using AAF.Services.Errors;
using AAFModel;

namespace Shared.Controls
{
    public abstract class DuplicationErrorsList<T, TDiff, TE> : IntegrityErrorsList<TE>
        where T: Declared
        where TDiff: Enum
        where TE: DuplicationError<T, TDiff>
    {

    }
}