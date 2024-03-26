using SceneModel;

namespace ScenesEditor
{
    public static class SceneExtension
    {
        public static string ParticipantTitle(this Scene scene, int participantIndex)
        {
            var p = scene.Participants[participantIndex];
            return $"#{participantIndex+1}. {p.Skeleton} [{p.Sex.Abbr()}]";
        }
    }
}