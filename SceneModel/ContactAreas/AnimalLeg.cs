namespace SceneModel.ContactAreas
{

    public sealed class AnimalLeg : ContactArea<Bilateral>
    {
        public override string DisplayName => "Leg";

        public static readonly AnimalLeg Any = new AnimalLeg(Bilateral.Any);

        public static readonly AnimalLeg Left = new AnimalLeg(Bilateral.Left);
        public static readonly AnimalLeg Right = new AnimalLeg(Bilateral.Right);

        public static readonly AnimalLeg Front = new AnimalLeg(Bilateral.Front);
        public static readonly AnimalLeg Back = new AnimalLeg(Bilateral.Back);

        public static readonly AnimalLeg LeftFront = new AnimalLeg(Bilateral.LeftFront);
        public static readonly AnimalLeg LeftBack = new AnimalLeg(Bilateral.LeftBack);

        public static readonly AnimalLeg RightFront = new AnimalLeg(Bilateral.RightFront);
        public static readonly AnimalLeg RightBack = new AnimalLeg(Bilateral.RightBack);
        private AnimalLeg(Bilateral locationness) : base(locationness)
        {
        }
    }
}