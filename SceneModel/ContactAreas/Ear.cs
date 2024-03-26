namespace SceneModel.ContactAreas
{
    public sealed class Ear : ContactArea<LeftRight>
    {
        public static readonly Ear Left = new Ear(LeftRight.Left);
        public static readonly Ear Right = new Ear(LeftRight.Right);
        public static readonly Ear Any = new Ear(LeftRight.Any);
        private Ear(LeftRight locationness) : base(locationness)
        {
        }
    }
}