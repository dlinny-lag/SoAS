namespace SceneModel.ContactAreas
{
    public sealed class Fang : ContactArea<BilateralVertical>
    {
        public static readonly Fang Any = new Fang(BilateralVertical.Any);

        public static readonly Fang Left = new Fang(BilateralVertical.Left);
        public static readonly Fang Right = new Fang(BilateralVertical.Right);

        public static readonly Fang Top = new Fang(BilateralVertical.Top);
        public static readonly Fang Bottom = new Fang(BilateralVertical.Bottom);

        public static readonly Fang LeftTop = new Fang(BilateralVertical.LeftTop);
        public static readonly Fang LeftBottom = new Fang(BilateralVertical.LeftBottom);

        public static readonly Fang RightTop = new Fang(BilateralVertical.RightTop);
        public static readonly Fang RightBottom = new Fang(BilateralVertical.RightBottom);

        private Fang(BilateralVertical locationness) : base(locationness)
        {
        }
    }
}