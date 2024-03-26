namespace SceneModel.ContactAreas
{
    public sealed class Nape : ContactArea<Single>
    {
        public static readonly Nape Any = new Nape();
        private Nape() : base(Single.Any)
        {
        }
    }
}