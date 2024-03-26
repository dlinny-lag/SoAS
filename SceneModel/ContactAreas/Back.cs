namespace SceneModel.ContactAreas
{
    public sealed class Back : ContactArea<Single>
    {
        public static readonly Back Any = new Back();
        private Back() : base(Single.Any)
        {
        }
    }
}