using System.Collections.Generic;
using AAF.Services.Reports;

namespace AAF.Services.Errors
{
    /// <summary>
    /// TODO: warning? should some missing furniture block position processing?
    /// </summary>
    public class MissingFurnitureError : PositionError
    {
        private readonly IList<string> missingFurniture;
        public MissingFurnitureError(string positionId, IList<string> missingFurniture) : base(positionId)
        {
            this.missingFurniture = missingFurniture;
        }

        public override Report Report()
        {
            Report retVal = new Report();
            retVal.AppendPos(PositionId).AppendNotice(" points to missing furniture: ");
            for (int i = 0; i < missingFurniture.Count; i++)
            {
                if (i > 0)
                    retVal.AppendNotice(", ");
                retVal.AppendCritical(missingFurniture[i]);
            }

            return retVal;
        }
    }
}