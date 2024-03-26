namespace SceneModel.ContactAreas
{
    public sealed class HumanoidLeg : ContactArea<LeftRight>
    {
        public override string DisplayName => "Leg";

        public static readonly HumanoidLeg Left = new HumanoidLeg(LeftRight.Left);
        public static readonly HumanoidLeg Right = new HumanoidLeg(LeftRight.Right);
        public static readonly HumanoidLeg Any = new HumanoidLeg(LeftRight.Any);
        private HumanoidLeg(LeftRight locationness) : base(locationness)
        {
        }
    }
}