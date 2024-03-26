namespace SceneModel.ContactAreas
{
    public sealed class Tongue : ContactArea<Single>
    {
        public static readonly Tongue Any = new Tongue();
        private Tongue() : base(Single.Any)
        {
        }
    }
}