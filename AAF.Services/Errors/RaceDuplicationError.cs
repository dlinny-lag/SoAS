using AAF.Services.Differences;
using AAFModel;

namespace AAF.Services.Errors
{
    public sealed class RaceDuplicationError : DuplicationError<Race, RaceDifference>
    {
        private static readonly CollectionComparer<Race, RaceDifference> comparer = new CollectionComparer<Race, RaceDifference>(new RaceComparer());
        public RaceDuplicationError(string id, Race reference) : base(id, reference)
        {

        }

        protected override CollectionComparer<Race, RaceDifference> Comparer => comparer;
    }
}