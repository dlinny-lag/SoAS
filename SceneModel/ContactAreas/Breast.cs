namespace SceneModel.ContactAreas
{
    public sealed class Breast:ContactArea<LeftRight>
    {
        public static readonly Breast Left = new Breast(LeftRight.Left);
        public static readonly Breast Right = new Breast(LeftRight.Right);
        public static readonly Breast Any = new Breast(LeftRight.Any);
        private Breast(LeftRight locationness) : base(locationness)
        {
        }
    }
}