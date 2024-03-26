namespace SceneModel.ContactAreas
{
    public sealed class Dildo : ContactArea<Single>
    {
        public static readonly Dildo Any = new Dildo();
        private Dildo() : base(Single.Any)
        {
        }
    }
}