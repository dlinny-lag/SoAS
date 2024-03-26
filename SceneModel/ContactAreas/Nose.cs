namespace SceneModel.ContactAreas
{
    public sealed class Nose : ContactArea<Single>
    {
        public static readonly Nose Any = new Nose();
        private Nose() : base(Single.Any)
        {
        }
    }
}