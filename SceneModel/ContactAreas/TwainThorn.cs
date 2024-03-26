namespace SceneModel.ContactAreas
{
    public sealed class TwainThorn : ContactArea<LeftRight>
    {
        public override string DisplayName => "Thorn";
        public static readonly TwainThorn Left = new TwainThorn(LeftRight.Left);
        public static readonly TwainThorn Right = new TwainThorn(LeftRight.Right);
        public static readonly TwainThorn Any = new TwainThorn(LeftRight.Any);
        private TwainThorn(LeftRight locationness) : base(locationness)
        {
        }
    }
}