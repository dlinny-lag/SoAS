namespace SceneModel.ContactAreas
{
    public sealed class Vagina : ContactArea<Single>
    {
        public static readonly Vagina Any = new Vagina();
        private Vagina() : base(Single.Any)
        {
        }
    }
}