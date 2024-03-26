namespace SceneModel.ContactAreas
{
    public sealed class Shoulder : ContactArea<Single>
    {
        public static readonly Shoulder Any = new Shoulder();
        private Shoulder() : base(Single.Any)
        {
        }
    }
}