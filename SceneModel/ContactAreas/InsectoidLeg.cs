namespace SceneModel.ContactAreas
{
    public sealed class InsectoidLegsLocationness : Locationness<LeftRightSymmetry, Linear3Asymmetry>
    {
        public static readonly InsectoidLegsLocationness Any = new InsectoidLegsLocationness(LeftRightSymmetry.None, Linear3Asymmetry.None);
        public static readonly InsectoidLegsLocationness Left = new InsectoidLegsLocationness(LeftRightSymmetry.Left, Linear3Asymmetry.None);
        public static readonly InsectoidLegsLocationness Right = new InsectoidLegsLocationness(LeftRightSymmetry.Right, Linear3Asymmetry.None);

        public static readonly InsectoidLegsLocationness Front = new InsectoidLegsLocationness(LeftRightSymmetry.None, Linear3Asymmetry.Front);
        public static readonly InsectoidLegsLocationness Middle = new InsectoidLegsLocationness(LeftRightSymmetry.None, Linear3Asymmetry.Middle);
        public static readonly InsectoidLegsLocationness Back = new InsectoidLegsLocationness(LeftRightSymmetry.None, Linear3Asymmetry.Back);

        public static readonly InsectoidLegsLocationness LeftFront = new InsectoidLegsLocationness(LeftRightSymmetry.Left, Linear3Asymmetry.Front);
        public static readonly InsectoidLegsLocationness LeftMiddle = new InsectoidLegsLocationness(LeftRightSymmetry.Left, Linear3Asymmetry.Middle);
        public static readonly InsectoidLegsLocationness LeftBack = new InsectoidLegsLocationness(LeftRightSymmetry.Left, Linear3Asymmetry.Back);
        public static readonly InsectoidLegsLocationness RightFront = new InsectoidLegsLocationness(LeftRightSymmetry.Right, Linear3Asymmetry.Front);
        public static readonly InsectoidLegsLocationness RightMiddle = new InsectoidLegsLocationness(LeftRightSymmetry.Right, Linear3Asymmetry.Middle);
        public static readonly InsectoidLegsLocationness RightBack = new InsectoidLegsLocationness(LeftRightSymmetry.Right, Linear3Asymmetry.Back);

        private InsectoidLegsLocationness(LeftRightSymmetry type1, Linear3Asymmetry type2)
        {
            Type1 = type1;
            Type2 = type2;
        }
        public override LeftRightSymmetry Type1 { get; }
        public override Linear3Asymmetry Type2 { get; }
    }

    public sealed class InsectoidLeg : ContactArea<InsectoidLegsLocationness>
    {
        public override string DisplayName => "Leg";

        public static readonly InsectoidLeg Any = new InsectoidLeg(InsectoidLegsLocationness.Any);
        public static readonly InsectoidLeg Left = new InsectoidLeg(InsectoidLegsLocationness.Left);
        public static readonly InsectoidLeg Right = new InsectoidLeg(InsectoidLegsLocationness.Right);

        public static readonly InsectoidLeg Front = new InsectoidLeg(InsectoidLegsLocationness.Front);
        public static readonly InsectoidLeg Middle = new InsectoidLeg(InsectoidLegsLocationness.Middle);
        public static readonly InsectoidLeg Back = new InsectoidLeg(InsectoidLegsLocationness.Back);

        public static readonly InsectoidLeg LeftFront = new InsectoidLeg(InsectoidLegsLocationness.LeftFront);
        public static readonly InsectoidLeg LeftMiddle = new InsectoidLeg(InsectoidLegsLocationness.LeftMiddle);
        public static readonly InsectoidLeg LeftBack = new InsectoidLeg(InsectoidLegsLocationness.LeftBack);
        public static readonly InsectoidLeg RightFront = new InsectoidLeg(InsectoidLegsLocationness.RightFront);
        public static readonly InsectoidLeg RightMiddle = new InsectoidLeg(InsectoidLegsLocationness.RightMiddle);
        public static readonly InsectoidLeg RightBack = new InsectoidLeg(InsectoidLegsLocationness.RightBack);

        private InsectoidLeg(InsectoidLegsLocationness locationness) : base(locationness)
        {
        }
    }
}