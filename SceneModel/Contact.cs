using System.Collections.Generic;
using SceneModel.Creatures;

namespace SceneModel
{
    public enum EnvironmentDirections
    {
        FromEnvironment = 0,
        FromActor = 1
    }

    public sealed class EnvironmentContact
    {
        public ParticipantContactDetails Details { get; set; } = new ParticipantContactDetails();
        public EnvironmentDirections Direction { get; set; }
        public void MakeAlive(IList<Participant> participants)
        {
            Details.Participant = participants[Details.ParticipantIndex];
            Details.MakeAlive();
            Details.ResolveBodyPart(Details.Participant.GetCreatureTemplate());
        }
    }

    public abstract class GenericActorsContact<TDetails>
        where TDetails:ContactDetails, new()
    {
        public TDetails From { get; set; } = new TDetails();
        public TDetails To { get; set; } = new TDetails();
        public override string ToString()
        {
            return $"From {From} to {To}";
        }
    }

    public sealed class ActorsContact : GenericActorsContact<ParticipantContactDetails>
    {
        public void MakeAlive(IList<Participant> participants)
        {
            From.Participant = participants[From.ParticipantIndex];
            From.MakeAlive();
            var template = From.Participant.GetCreatureTemplate();
            if (template != null)
                From.ResolveBodyPart(template);
            To.Participant = participants[To.ParticipantIndex];
            To.MakeAlive();
            template = To.Participant.GetCreatureTemplate();
            if (template != null)
                To.ResolveBodyPart(template);
        }
    }
}