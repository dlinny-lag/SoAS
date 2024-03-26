namespace SceneModel.ContactAreas
{
    public sealed class Either : ContactArea<Single>
    {
        public static readonly Either Any = new Either();
        public Either() : base(Single.Any)
        {
        }
    }
}