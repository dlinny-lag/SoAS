namespace SceneModel.ContactAreas
{
    public sealed class Hand : ContactArea<Single>
    {
        public static readonly Hand Any = new Hand();
        private Hand() : base(Single.Any)
        {
        }
    }
}