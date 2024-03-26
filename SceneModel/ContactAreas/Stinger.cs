namespace SceneModel.ContactAreas
{
    public sealed class Stinger : ContactArea<Single>
    {
        public static readonly Stinger Any = new Stinger();
        private Stinger() : base(Single.Any)
        {
        }
    }
}