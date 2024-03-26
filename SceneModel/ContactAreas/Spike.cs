namespace SceneModel.ContactAreas
{
    public sealed class Spike : ContactArea<Single>
    {
        public static readonly Spike Any = new Spike();
        private Spike() : base(Single.Any)
        {
        }
    }
}