namespace SceneModel.ContactAreas
{
    public sealed class Chest : ContactArea<Single>
    {
        public static readonly Chest Any = new Chest();
        private Chest() : base(Single.Any)
        {
        }
    }
}