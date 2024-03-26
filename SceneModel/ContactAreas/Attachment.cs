using System;
using System.Collections.Generic;

namespace SceneModel.ContactAreas
{
    public abstract class Attachment : ContactArea<Single>
    {
        private readonly List<BodyPart> attachedTo;
        protected Attachment(params BodyPart[] contacts) : base(Single.Any)
        {
            attachedTo = new List<BodyPart>(contacts);
        }

        public BodyPart[] Contacts => attachedTo.ToArray();
    }

    public sealed class AttachmentReference : AreaReference
    {
        public AttachmentReference(Attachment area) : base(area ?? throw new ArgumentNullException(nameof(area)))
        {
        }
    }
}