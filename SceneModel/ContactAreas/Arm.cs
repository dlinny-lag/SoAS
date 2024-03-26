namespace SceneModel.ContactAreas
{
    public sealed class Arm : ContactArea<LeftRight>
    {
        public static readonly Arm Left = new Arm(LeftRight.Left);
        public static readonly Arm Right = new Arm(LeftRight.Right);
        public static readonly Arm Any = new Arm(LeftRight.Any);
        private Arm(LeftRight locationness) : base(locationness)
        {
        }
    }
}