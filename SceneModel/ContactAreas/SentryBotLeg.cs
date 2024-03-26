namespace SceneModel.ContactAreas
{
    public sealed class SentryBotLeg : ContactArea<Azimuthal3>
    {
        public static readonly SentryBotLeg Any = new SentryBotLeg(Azimuthal3.Any);
        public static readonly SentryBotLeg Back = new SentryBotLeg(Azimuthal3.Back);
        public static readonly SentryBotLeg Left = new SentryBotLeg(Azimuthal3.Left);
        public static readonly SentryBotLeg Right = new SentryBotLeg(Azimuthal3.Right);

        private SentryBotLeg(Azimuthal3 locationness) : base(locationness)
        {
        }
    }
}