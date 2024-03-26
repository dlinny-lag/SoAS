namespace SceneModel.ContactAreas
{
    public sealed class Hammer : ContactArea<Single>
    {
        public static readonly Hammer Any = new Hammer();
        private Hammer() : base(Single.Any)
        {
        }
    }
}