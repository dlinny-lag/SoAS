namespace SceneModel.ContactAreas
{
    public sealed class Waist : ContactArea<Single>
    {
        public static readonly Waist Any = new Waist();
        private Waist() : base(Single.Any)
        {
        }
    }
}