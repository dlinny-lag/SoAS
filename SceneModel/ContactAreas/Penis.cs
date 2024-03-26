namespace SceneModel.ContactAreas
{
    public sealed class Penis : ContactArea<Single>
    {
        public static readonly Penis Any = new Penis();

        public Penis() : base(Single.Any)
        {
        }
    }
}