namespace SceneModel.ContactAreas
{
    public sealed class AnimalDualHorn : ContactArea<LeftRight>
    {
        public override string DisplayName => "Horn";
        public static readonly AnimalDualHorn Any = new AnimalDualHorn(LeftRight.Any);
        public static readonly AnimalDualHorn Left = new AnimalDualHorn(LeftRight.Left);
        public static readonly AnimalDualHorn Right = new AnimalDualHorn(LeftRight.Right);
        private AnimalDualHorn(LeftRight locationness) : base(locationness)
        {
        }
    }
}