namespace SceneModel.ContactAreas
{
    public sealed class Butt : ContactArea<Single>
    {
        public static readonly Butt Any = new Butt();
        private Butt() : base(Single.Any)
        {
        }
    }
}