namespace SceneModel.ContactAreas
{
    public sealed class Teeth : ContactArea<TopBottom>
    {
        public static readonly Teeth Any = new Teeth(TopBottom.Any);
        public static readonly Teeth Top = new Teeth(TopBottom.Top);
        public static readonly Teeth Bottom = new Teeth(TopBottom.Bottom);

        private Teeth(TopBottom locationness) : base(locationness)
        {
        }
    }
}