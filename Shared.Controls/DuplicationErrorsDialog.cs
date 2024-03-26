using System;
using AAF.Services.AAFImport;
using AAF.Services.Errors;
using AAFModel;

namespace Shared.Controls
{
    public abstract class DuplicationErrorsDialog<T, TDiff, TE, TL, TD> : IntegrityErrorsDialog<TE, TL, TD>
        where T: Declared
        where TDiff : Enum
        where TE : DuplicationError<T, TDiff>
        where TL : DuplicationErrorsList<T, TDiff, TE>, new()
        where TD : DuplicationErrorsDialog<T, TDiff, TE, TL, TD>, new()
    {
        protected DuplicationErrorsDialog()
        {
            if (typeof(TD) != GetType())
                throw new TypeInitializationException(GetType().FullName, 
                    new InvalidOperationException($"Invalid declaration of type {GetType().FullName}. {nameof(TD)} must be {GetType().FullName}, but {typeof(TD).FullName} defined"));
        }
    }
}