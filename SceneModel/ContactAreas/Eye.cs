namespace SceneModel.ContactAreas
{
    public sealed class Eye : ContactArea<LeftRight>
    {
        public static readonly Eye Left = new Eye(LeftRight.Left);
        public static readonly Eye Right = new Eye(LeftRight.Right);
        public static readonly Eye Any = new Eye(LeftRight.Any);
        private Eye(LeftRight locationness) : base(locationness)
        {
        }
    }
}