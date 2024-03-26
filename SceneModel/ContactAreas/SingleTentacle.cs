namespace SceneModel.ContactAreas
{
    public sealed class SingleTentacle : ContactArea<Single>
    {
        public override string DisplayName => "Tentacle";

        public static readonly SingleTentacle Any = new SingleTentacle();
        private SingleTentacle() : base(Single.Any)
        {
        }
    }
}