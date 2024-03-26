namespace SceneModel.ContactAreas
{
    public sealed class Stick : ContactArea<Single>
    {
        public static readonly Stick Any = new Stick();

        public Stick() : base(Single.Any)
        {
        }
    }
}