using System;
using System.Linq;
using AAF.Services.Reports;
using AAFModel;

namespace AAF.Services.Errors
{
    public sealed class AmbiguousReferenceError : PositionError
    {
        private readonly Referenceable[] ambiguity;
        public AmbiguousReferenceError(string positionId, params Referenceable[] refs) : base(positionId)
        {
            ambiguity = refs.Where(t => t.Type != ReferenceType.None).ToArray();
            if (ambiguity.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(refs), "No any type defined");
            if (ambiguity.Length == 1)
                throw new ArgumentOutOfRangeException(nameof(refs), "Not an erroneous combination");
        }
        public override Report Report()
        {
            Report retVal = new Report();
            retVal.AppendPos(PositionId).AppendNotice(" points to multiple entities: ");
            for (int i = 0; i < ambiguity.Length; i++)
            {
                if (i > 0)
                    retVal.AppendNotice(", ");
                var reference = ambiguity[i];
                retVal.AppendRef(reference.Id,reference.Type, reference.File);
            }

            return retVal;
        }
    }
}