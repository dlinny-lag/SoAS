using AAF.Services.Reports;
using AAFModel;

namespace AAF.Services.Errors
{
    public sealed class MissingReferenceError : PositionError
    {
        private readonly ReferenceType type;
        private readonly string referenceId;
        public MissingReferenceError(string positionId, string referenceId, ReferenceType type = ReferenceType.None) : base(positionId)
        {
            this.type = type;
            this.referenceId = referenceId;
        }

        public override Report Report()
        {
            Report retVal = new Report();
            retVal.AppendPos(PositionId);
            retVal.Append(" points to missing ");
            if (type == ReferenceType.None)
                retVal.Append(ReferenceLexem.Miss(referenceId));
            else
                retVal.AppendRef(referenceId, type);

            return retVal;
        }
    }
}