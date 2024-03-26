namespace SceneModel.ContactAreas
{
    public sealed class Strapon : ContactArea<Single>
    {
        public override string DisplayName => "Strap-on";

        public static readonly Strapon Any = new Strapon();

        public Strapon() : base(Single.Any)
        {
        }
    }
}