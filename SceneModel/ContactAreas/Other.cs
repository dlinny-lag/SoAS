namespace SceneModel.ContactAreas
{
    public sealed class Other : ContactArea<Single>
    {
        public static readonly Other Any = new Other();
        private Other() : base(Single.Any)
        {
        }
    }
}