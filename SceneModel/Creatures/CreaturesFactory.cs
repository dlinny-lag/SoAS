using System;
using System.Collections.Generic;

namespace SceneModel.Creatures
{
    public static class CreaturesFactory
    {
        private static readonly Dictionary<string, Creature> mapping;

        static CreaturesFactory()
        {
            var references = new Func<Creature>[14]
            {
                () => new Cat(),
                () => new Dog(),
                () => new Brahmin(),
                () => new Gorilla(),
                () => new FEVHound(),
                () => new MoleRat(),
                () => new YaoGuai(),
                () => new RadStag(),
                () => new RadRabbit(),

                () => new Human(),
                () => new SuperMutant(),
                () => new FeralGhoul(),
                () => new SuperMutantBehemoth(),
                () => new LibertyPrime(),
            };
            mapping = new Dictionary<string, Creature>(references.Length);
            foreach (Func<Creature> ctr in references)
            {
                var creature = ctr();
                mapping.Add(creature.Skeleton.ToUpperInvariant(), creature);
            }
        }

        public static bool IsSupportedSkeleton(this string skeleton)
        {
            return skeleton != null && mapping.ContainsKey(skeleton.ToUpperInvariant());
        }

        public static Creature GetCreatureTemplate(this Participant p)
        {
            if (!mapping.TryGetValue(p.Skeleton.ToUpperInvariant(), out var creature))
                return null;
            return creature[p.Sex];
        }
    }
}