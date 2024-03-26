namespace SceneModel.ContactAreas
{
    public sealed class Rifle : ContactArea<Single>
    {
        public static readonly Rifle Any = new Rifle();
        private Rifle() : base(Single.Any)
        {
        }
    }
}