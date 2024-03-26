namespace SceneModel.ContactAreas
{
    public sealed class Ovipositor : ContactArea<Single>
    {
        public static readonly Ovipositor Any = new Ovipositor();
        private Ovipositor() : base(Single.Any)
        {
        }
    }
}