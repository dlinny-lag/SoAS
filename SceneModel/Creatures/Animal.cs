using System.Collections.Generic;
using SceneModel.ContactAreas;
using Shared.Utils;

namespace SceneModel.Creatures
{
    public static class Animal
    {
        public static BodyPart[] Sticks => new BodyPart[]
        {
            BodyPart.Leaf(Stick.Any),
            BodyPart.Leaf(Penis.Any), 
        };
        public static BodyPart[] SticksAndVagina => new BodyPart[]
        {
            BodyPart.Leaf(Stick.Any),
            BodyPart.Leaf(Penis.Any), 
            BodyPart.Leaf(Vagina.Any),
        };

        public static BodyPart[] MouthParts(bool withFang)
        {
            if (withFang)
                return new [] { BodyPart.Leaf(Tongue.Any), BodyPart.Leaf(Teeth.Any), BodyPart.Leaf(Fang.Any) };
            return new[] { BodyPart.Leaf(Tongue.Any), BodyPart.Leaf(Teeth.Any) };
        }

        public static List<BodyPart> HeadParts(List<BodyPart> faceParts, params BodyPart[] additionalHeadParts)
        {
            List<BodyPart> retVal = Mammal.HeadParts(faceParts.ToArray());
            retVal.AddRange(additionalHeadParts);
            return retVal;
        }
    }

    public abstract class CommonAnimal : Creature
    {
        protected CommonAnimal(bool haveFang, bool withVagina, BodyPart horns = null) : base(null, 
            new BodyPart(Head.Any, Animal.HeadParts(Mammal.FaceParts(Animal.MouthParts(haveFang))).With(horns).ToArray()),
            new BodyPart(Torso.Any, 
                Mammal.TorsoParts()
                    .With(BodyPart.Leaf(Tail.Any))
                    .With(withVagina?Animal.SticksAndVagina:Animal.Sticks)
                    .ToArray()),
            BodyPart.Leaf(AnimalLeg.Any)
            )
        {
        }
    }

    public sealed class Cat : CommonAnimal
    {
        public override string Skeleton => "Cat";

        public Cat() : base(false, true)
        {
        }
    }
    
    public sealed class Dog : CommonAnimal
    {
        public override string Skeleton => "Dog";
        public Dog() : base(true, true)
        {
        }
    }
    
    public sealed class FEVHound : CommonAnimal
    {
        public override string Skeleton => "FEVHound";
        public FEVHound() : base(true, true)
        {
        }
    }

    public sealed class MoleRat : CommonAnimal
    {
        public override string Skeleton => "MoleRat";
        public MoleRat() : base(true, true)
        {
        }
    }
    
    public sealed class YaoGuai : CommonAnimal
    {
        public override string Skeleton => "YaoGuai";
        public YaoGuai() : base(true, true)
        {
        }
    }

    public sealed class RadRabbit : CommonAnimal
    {
        public override string Skeleton => "RadRabbit";
        public RadRabbit() : base(true, true)
        {
        }
    }
    
    public sealed class RadStag : CommonAnimal
    {
        public override string Skeleton => "RadStag";

        public RadStag() : base(false, true, BodyPart.Leaf(AnimalDualHorn.Any))
        {
        }
    }

    public sealed class Brahmin : Creature
    {
        public override string Skeleton => "Brahmin";

        public Brahmin() : base(null, 
            new BodyPart(DualHead.Any, 
                Animal.HeadParts(
                    Mammal.FaceParts(Animal.MouthParts(false))
                        .With(BodyPart.Leaf(AnimalDualHorn.Any)))
                    .ToArray()),
            new BodyPart(Torso.Any, 
                Mammal.TorsoParts()
                    .With(BodyPart.Leaf(Tail.Any))
                    .With(Animal.SticksAndVagina)
                    .ToArray()),
            BodyPart.Leaf(AnimalLeg.Any)
        )
        {
        }
    }
}