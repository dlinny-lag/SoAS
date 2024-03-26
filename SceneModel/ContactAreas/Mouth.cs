namespace SceneModel.ContactAreas
{
    public sealed class Mouth : ContactArea<Single>
    {
        public static readonly Mouth Any = new Mouth();
        private Mouth() : base(Single.Any)
        {
        }
    }
}