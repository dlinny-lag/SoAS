using System.Collections.Generic;
using SceneModel.ContactAreas;

namespace SceneModel.Creatures
{
    public static class Mammal
    {
        public static List<BodyPart> FaceParts(params BodyPart[] mouthParts) => new List<BodyPart>
        {
            BodyPart.Leaf(Eye.Any),
            BodyPart.Leaf(Nose.Any),
            new BodyPart(Mouth.Any, mouthParts),
        };

        public static List<BodyPart> HeadParts(BodyPart[] faceParts) => new List<BodyPart>
        {
            new BodyPart(Face.Any, faceParts),
            BodyPart.Leaf(Nape.Any),
            BodyPart.Leaf(Ear.Any),
            BodyPart.Leaf(Neck.Any),
        };

        public static List<BodyPart> TorsoParts(params BodyPart[] chestParts) => new List<BodyPart>
        {
            new BodyPart(Chest.Any, chestParts),
            BodyPart.Leaf(Back.Any), 
            BodyPart.Leaf(Waist.Any), 
            BodyPart.Leaf(Butt.Any), 
            BodyPart.Leaf(Anus.Any), 
            BodyPart.Leaf(Either.Any),
        };
    }
}