namespace SceneModel.ContactAreas
{
    public sealed class Foot : ContactArea<Single>
    {
        public static readonly Foot Any = new Foot();
        private Foot() : base(Single.Any)
        {
        }
    }
}