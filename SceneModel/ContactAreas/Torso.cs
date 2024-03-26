namespace SceneModel.ContactAreas
{
    public sealed class Torso : ContactArea<Single>
    {
        public static readonly Torso Any = new Torso();
        private Torso() : base(Single.Any)
        {
        }
    }
}