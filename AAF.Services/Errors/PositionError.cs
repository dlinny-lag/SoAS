namespace AAF.Services.Errors
{
    public abstract class PositionError : IntegrityError
    {
        protected PositionError(string positionId)
        {
            PositionId = positionId;
        }

        public readonly string PositionId;
    }
}