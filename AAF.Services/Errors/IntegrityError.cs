using AAF.Services.Reports;

namespace AAF.Services.Errors
{
    public abstract class IntegrityError
    {
        public abstract Report Report();
    }
}