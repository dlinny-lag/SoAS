using System;
using System.Collections.Generic;
using SceneModel.Creatures;

namespace SceneModel
{
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

    public sealed class ActorsContact : GenericActorsContact<ParticipantContactDetails>, IHasId, IHasDistance<ActorsContact>
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

        public Guid Id { get; set; }
        public ulong Distance(ActorsContact other)
        {
            throw new NotImplementedException();
        }
    }
}