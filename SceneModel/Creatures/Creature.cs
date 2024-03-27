using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using SceneModel.ContactAreas;
using Shared.Utils;
using Single = SceneModel.ContactAreas.Single;

namespace SceneModel.Creatures
{
    public abstract class Creature
    {
        sealed class FakeRoot : ContactArea<Single>
        {
            public override string DisplayName => "";
            public static readonly FakeRoot Any = new FakeRoot();
            private FakeRoot() : base(Single.Any)
            {
            }
        }

        private static readonly ConcurrentDictionary<Type, ConstructorInfo> constructorsCache = new ConcurrentDictionary<Type, ConstructorInfo>();

        private static ConstructorInfo GetConstructor(Type t)
        {
            return constructorsCache.GetOrAdd(t, (key) => 
                key.GetConstructor(Type.EmptyTypes) ??
                throw new NoNullAllowedException($"Can't find default constructor for {key.FullName}"));
        }

        private Creature Create()
        {
            return (Creature)GetConstructor(GetType()).Invoke(Array.Empty<object>());
        }

        public abstract string Skeleton { get; }

        private readonly List<AttachmentReference> attachments;
        private readonly BodyPart fakeRoot;

        protected Creature(Attachment[] attachments, params BodyPart[] body)
        {
            this.attachments = new List<AttachmentReference>(attachments?.Select(a => new AttachmentReference(a)) ?? Array.Empty<AttachmentReference>());
            fakeRoot = BodyPart.TopRoot(FakeRoot.Any, body);
        }

        public BodyPart[] Body => fakeRoot.Children;
        public AttachmentReference[] Attachments => attachments.ToArray();


        public sealed override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Body");
            foreach (BodyPart part in Body)
            {
                sb.Append(part.ToString(BodyPart.PrintIndent));
            }

            if (attachments.Count == 0)
                return sb.ToString();

            sb.AppendLine("Attachments");
            foreach (AttachmentReference attachment in attachments)
            {
                sb.Space(BodyPart.PrintIndent).Append(attachment.Area.Id);
            }

            return sb.ToString();
        }

        private void ResolveMultiAreas(Sex sex)
        {
            BodyPart stick = fakeRoot.Find(Stick.Any);
            if (stick != null)
            {
                ReplaceWithoutDuplications(stick, sex.GetStick());
            }

            BodyPart penis = fakeRoot.Find(Penis.Any);
            if (penis != null)
            {
                ReplaceWithoutDuplications(penis, sex.GetPenis());
            }

            BodyPart either = fakeRoot.Find(Either.Any);
            if (either != null)
            {
                ReplaceWithoutDuplications(either, sex.GetEither());
            }

            BodyPart vagina = fakeRoot.Find(Vagina.Any);
            if (vagina != null)
            {
                ReplaceWithoutDuplications(vagina, sex.GetVagina());
            }
        }

        private static void ReplaceWithoutDuplications(BodyPart toReplace, params ContactArea[] resolved)
        {
            List<ContactArea> replaceBy = new List<ContactArea>();
            foreach (ContactArea area in resolved)
            {
                if (area == null)
                    continue;
                var found = toReplace.Parent.Find(area);
                if (found != null && found != toReplace)
                    continue; // do not skip replacing part
                replaceBy.Add(area);
            }
            toReplace.ReplaceBy(replaceBy.Select(BodyPart.Leaf).ToArray());
        }

        private static VisitResult InjectVariants(BodyPart current, BodyPart fakeRoot)
        {
            if (current == fakeRoot)
                return VisitResult.None;

            var variants = current.Area.IsAny ? current.Area.GetVariants() : Array.Empty<ContactArea>();
            if (variants.Length <= 1)
                return VisitResult.None;
            List<BodyPart> variations = new List<BodyPart>(variants.Length - 1);
            foreach (ContactArea area in variants)
            {
                variations.Add(new BodyPart(area, current.Children.Select(c => c.Clone()).ToArray()));
            }
            current.ReplaceBy(variations.ToArray());
            return VisitResult.Replaced;
        }
        private void EnrichBySymmetry()
        {
            fakeRoot.VisitAll(InjectVariants, fakeRoot);
        }

        private bool canHasCache = true;
        private Creature[] cacheBySex = null;
        public Creature this[Sex sex]
        {
            get
            {
                if (!canHasCache)
                    throw new InvalidOperationException("Must be called on template object only");
                
                if (cacheBySex == null)
                    cacheBySex = new Creature[4];

                int sexIndex = (int)sex;
                if (cacheBySex[sexIndex] != null)
                    return cacheBySex[sexIndex];

                Creature retVal = Create();
                retVal.canHasCache = false;
                retVal.ResolveMultiAreas(sex);
                retVal.EnrichBySymmetry();
                cacheBySex[sexIndex] = retVal;
                

                return cacheBySex[sexIndex];
            }
        }

        public AreaReference[] GetContactAreas()
        {
            List<AreaReference> retVal = new List<AreaReference>(Body);
            retVal.AddRange(attachments);
            return retVal.ToArray();
        }

        public BodyPart Find(params ContactArea[] reverse)
        {
            if (reverse.Length == 0)
                return null;
            string reversePath = null;
            if (reverse.Length > 1)
            {
                // TODO: refactor along with BodyPart.Find
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < reverse.Length; i++)
                {
                    if (i > 0)
                        sb.Append(BodyPart.Delimiter);
                    sb.Append(reverse[i].Id);
                }

                reversePath = sb.ToString();
            }
            return fakeRoot.Find(reverse[0], reversePath);
        }
    }
}