using AAF.Services.Differences;
using AAFModel;

namespace AAF.Services.Errors
{
    public sealed class PositionDuplicationError : DuplicationError<Position, PositionDifference>
    {
        private static readonly CollectionComparer<Position, PositionDifference> positionsComparer =
            new CollectionComparer<Position, PositionDifference>(PositionComparer.Default);

        public PositionDuplicationError(string id, Position reference) : base(id, reference)
        {
        }

        protected override CollectionComparer<Position, PositionDifference> Comparer => positionsComparer;
    }
}