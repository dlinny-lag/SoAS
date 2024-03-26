using System.Collections.Generic;
using SceneModel.ContactAreas;
using Shared.Utils;

namespace SceneModel.Creatures
{
    public abstract class Humanoid : Creature
    {
        protected static BodyPart[] ChestParts => new BodyPart[]
        {
            new BodyPart(Breast.Any, Nipple.Any),
        };

        protected Humanoid(Attachment[] attachments, BodyPart[] chestParts, BodyPart[] genitaliaParts, BodyPart[] mouthParts) : base(attachments, 
            new List<BodyPart>
            {
                new BodyPart(Head.Any, Animal.HeadParts(Mammal.FaceParts(mouthParts)).ToArray()),
                new BodyPart(Arm.Any, Shoulder.Any, Wrist.Any, Hand.Any),
                new BodyPart(Torso.Any, 
                    Mammal.TorsoParts(chestParts)
                        .With(1, BodyPart.Leaf(Armpit.Any))
                        .With(genitaliaParts)
                        .ToArray()),
                
                new BodyPart(HumanoidLeg.Any, Foot.Any)
            }.ToArray())
        {

        }
    }

    public class Human : Humanoid
    {
        public override string Skeleton => "Human";

        public Human() : base(
        new Attachment[]
                {
                    // TODO: 
                }, 
                Humanoid.ChestParts, Animal.SticksAndVagina, Animal.MouthParts(false)
            )
        {
        }
    }
    public sealed class Gorilla : Humanoid
    {
        public override string Skeleton => "Gorilla";

        public Gorilla() : base(null, Humanoid.ChestParts, Animal.Sticks, Animal.MouthParts(true))
        {
        }
    }

    public sealed class SuperMutant : Human
    {
        public override string Skeleton => "SuperMutant";
    }
    
    public sealed class FeralGhoul : Humanoid
    {
        public override string Skeleton => "FeralGhoul";

        public FeralGhoul() : base(null, new []{BodyPart.Leaf(Breast.Any)}, Animal.Sticks, Animal.MouthParts(false))
        {
        }
    }

    public sealed class SuperMutantBehemoth : Humanoid
    {
        public override string Skeleton => "SuperMutantBehemoth";

        public SuperMutantBehemoth() : base(null, new []{BodyPart.Leaf(Breast.Any)}, Animal.Sticks, Animal.MouthParts(false))
        {
        }
    }
    
    public sealed class LibertyPrime : Humanoid 
    {
        public override string Skeleton => "LibertyPrime";
        public LibertyPrime() : base(null, new []{BodyPart.Leaf(Breast.Any)}, Animal.Sticks, Animal.MouthParts(false))
        {
        }
    }
}