namespace SceneModel.ContactAreas
{
    public sealed class SingleThorn : ContactArea<Single>
    {
        public override string DisplayName => "Thorn";
        public static readonly SingleThorn Any = new SingleThorn();
        private SingleThorn() : base(Single.Any)
        {
        }
    }
}