namespace SceneModel.ContactAreas
{
    public sealed class Gun : ContactArea<Single>
    {
        public static readonly Gun Any = new Gun();
        private Gun() : base(Single.Any)
        {
        }
    }
}