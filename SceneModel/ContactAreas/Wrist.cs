namespace SceneModel.ContactAreas
{
    public sealed class Wrist : ContactArea<Single>
    {
        public static readonly Wrist Any = new Wrist();
        private Wrist() : base(Single.Any)
        {
        }
    }
}