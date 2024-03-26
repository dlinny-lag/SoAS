namespace SceneModel.ContactAreas
{
    public sealed class Nipple : ContactArea<Single>
    {
        public static readonly Nipple Any = new Nipple();

        private Nipple() : base(Single.Any)
        {
        }
    }
}