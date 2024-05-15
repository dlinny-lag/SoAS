using System;
using System.Collections.Generic;
using SceneModel.Creatures;

namespace SceneModel
{
    public enum EnvironmentDirections
    {
        FromEnvironment = 0,
        FromActor = 1
    }

    public sealed class EnvironmentContact : IHasId, IHasDistance<EnvironmentContact>
    {
        public ParticipantContactDetails Details { get; set; } = new ParticipantContactDetails();
        public EnvironmentDirections Direction { get; set; }
        public void MakeAlive(IList<Participant> participants)
        {
            Details.Participant = participants[Details.ParticipantIndex];
            Details.MakeAlive();
            Details.ResolveBodyPart(Details.Participant.GetCreatureTemplate());
        }

        public Guid Id { get; set; }
        public ulong Distance(EnvironmentContact other)
        {
            throw new NotImplementedException();
        }
    }
}