namespace SceneModel.ContactAreas
{
    public sealed class DualHead : ContactArea<LeftRight>
    {
        public override string DisplayName => "Head";

        public static readonly DualHead Any = new DualHead(LeftRight.Any);
        public static readonly DualHead Left = new DualHead(LeftRight.Left);
        public static readonly DualHead Right = new DualHead(LeftRight.Right);
        private DualHead(LeftRight locationness) : base(locationness)
        {
        }
    }
}