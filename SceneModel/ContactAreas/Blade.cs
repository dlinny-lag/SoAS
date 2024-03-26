namespace SceneModel.ContactAreas
{
    public sealed class Blade : ContactArea<Single>
    {
        public static readonly Blade Any = new Blade();
        private Blade() : base(Single.Any)
        {
        }
    }
}