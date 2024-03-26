namespace SceneModel.ContactAreas
{
    public sealed class Tail : ContactArea<Single>
    {
        public static readonly Tail Any = new Tail();
        private Tail() : base(Single.Any)
        {
        }
    }
}