namespace SceneModel.ContactAreas
{
    public sealed class Throat : ContactArea<Single>
    {
        public static readonly Throat Any = new Throat();
        private Throat() : base(Single.Any)
        {
        }
    }
}