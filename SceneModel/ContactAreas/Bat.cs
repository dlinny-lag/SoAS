namespace SceneModel.ContactAreas
{
    public sealed class Bat : ContactArea<Single>
    {
        public static readonly Bat Any = new Bat();
        private Bat() : base(Single.Any)
        {
        }
    }
}