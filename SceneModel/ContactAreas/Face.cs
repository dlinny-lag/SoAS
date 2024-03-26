namespace SceneModel.ContactAreas
{
    public sealed class Face : ContactArea<Single>
    {
        public static readonly Face Any = new Face();
        private Face() : base(Single.Any)
        {
        }
    }
}