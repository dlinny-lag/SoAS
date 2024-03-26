namespace SceneModel.ContactAreas
{
    public sealed class Head : ContactArea<Single>
    {
        public static readonly Head Any = new Head();
        private Head() : base(Single.Any)
        {

        }
    }
}