namespace SceneModel.ContactAreas
{
    public sealed class Neck : ContactArea<Single>
    {
        public static readonly Neck Any = new Neck();
        private Neck() : base(Single.Any)
        {
        }
    }
}