namespace SceneModel
{
    public class ParticipantSettings
    {
        public bool IsAggressor { get; set; }
        public bool IsVictim { get; set; }
    }
    /// <summary>
    /// Scene participant details
    /// </summary>
    public class Participant : ParticipantSettings
    {
        public string Skeleton { get; set; }
        public Sex Sex { get; set; }

    }
}