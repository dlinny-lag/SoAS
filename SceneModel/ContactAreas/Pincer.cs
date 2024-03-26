namespace SceneModel.ContactAreas
{
    public sealed class Pincer : ContactArea<LeftRight>
    {
        public static readonly Pincer Left = new Pincer(LeftRight.Left);
        public static readonly Pincer Right = new Pincer(LeftRight.Right);
        public static readonly Pincer Any = new Pincer(LeftRight.Any);
        private Pincer(LeftRight locationness) : base(locationness)
        {
        }
    }
}