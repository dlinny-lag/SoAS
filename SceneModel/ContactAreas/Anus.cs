namespace SceneModel.ContactAreas
{
    public sealed class Anus : ContactArea<Single>
    {
        public static readonly Anus Any = new Anus();
        private Anus() : base(Single.Any)
        {
        }
    }
}