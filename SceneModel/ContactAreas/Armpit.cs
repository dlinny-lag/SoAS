namespace SceneModel.ContactAreas
{
    public sealed class Armpit : ContactArea<Single>
    {
        public static readonly Armpit Any = new Armpit();
        private Armpit() : base(Single.Any)
        {
        }
    }
}