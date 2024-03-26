using AAF.Services.AAFImport;
using AAF.Services.Errors;

namespace Shared.Controls
{
    public sealed class PositionValidationErrorsDialog : IntegrityErrorsDialog<PositionError, ValidationErrorsList, PositionValidationErrorsDialog>
    {
        public PositionValidationErrorsDialog()
        {
            Text = "Position validation errors";
        }
    }
}