namespace AAFModel
{
    public class Actor
    {
        public string Gender { get; set; }
        public string Skeleton { get; set; }
        public int IdleFormId { get; set; }

        /// <summary>
        /// optional. may be declared, but used only for Skeleton resolving
        /// </summary>
        public string Race { get; set; }
    }
}