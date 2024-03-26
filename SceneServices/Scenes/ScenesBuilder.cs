using System;
using System.Collections.Generic;
using AAF.Services.AAFImport;
using AAFModel;
using SceneModel;
using SceneServices.TagCategories;
using SceneServices.TagsHandlers;

namespace SceneServices.Scenes
{
    public static class ScenesBuilder
    {
        /// <summary>
        /// Returns Animation object corresponding to the Position
        /// </summary>
        /// <returns></returns>
        private static Animation GetAnimation(Position position,
            IDictionary<string, Position> positions,
            IDictionary<string, Animation> animations,
            IDictionary<string, AnimationGroup> animGroups,
            IDictionary<string, PositionTree> posTrees)
        {
            switch (position.ReferenceType)
            {
                case ReferenceType.Animation:
                    return animations[position.Reference];
                case ReferenceType.AnimationGroup:
                    return animations[animGroups[position.Reference].Animations[0]];
                case ReferenceType.PositionTree:
                    return ResolveFromTree(posTrees[position.Reference], positions, animations, animGroups);
            }

            throw new ArgumentOutOfRangeException(nameof(position), "Position reference type is undefined");
        }

        private static Animation ResolveFromTree(PositionTree tree, IDictionary<string, Position> positions, IDictionary<string, Animation> animations, IDictionary<string, AnimationGroup> groups)
        {
            if (!positions.TryGetValue(tree.Root.PositionId, out var position))
                throw new InvalidOperationException($"There is no position {tree.Root.PositionId} mentioned in tree {tree.Id}");
            return GetAnimation(position, positions, animations, groups, null); // trees can not be referenced by positions in positions tree
        }

        public static BuildResult GenerateScenes(this AAFData aafModel)
        {
            List<Scene> scenes = new List<Scene>(aafModel.Positions.Count);

            var result = ImportResult.Import(aafModel);

            foreach (var pair in result.Positions)
            {
                if (result.Errors.ValidationErrors.ContainsKey(pair.Key))
                    continue; // do not use erroneous positions

                Position p = pair.Value;
                Animation anim = GetAnimation(p, result.Positions, result.Animations, result.AnimationGroups, result.PositionsTrees);
                if (aafModel.Files.TryGetValue(anim.File, out var defaults))
                    defaults = Defaults.Default;

                Scene scene = new Scene() { Id = p.Id, Type = p.ReferenceType.ToSceneType() };
                scene.RawTags = result.AllTags.TryGetValue(p.Id, out var tags) ? tags : new List<string[]>();

                scene.Furniture = result.MergedLocations.TryGetValue(p.Id, out var locations) ? locations : new HashSet<string>();

                scene.Participants = new List<Participant>(anim.Actors.Count);
                for (int i = 0; i < anim.Actors.Count; i++)
                {
                    scene.Participants.Add(new Participant
                    {
                        Skeleton = anim.Actors[i].GetSkeleton(defaults, result.Races),
                        Sex = anim.Actors[i].Gender.ToSex(),
                    });
                }

                scene.FillCategories();

                scene.MakeAlive();

                scenes.Add(scene);
            }

            return new BuildResult(scenes, result.Errors);
        }

        public static void GuessContacts(this Scene scene)
        {
            var actors = scene.GuessActorContacts();
            scene.ActorsContacts = actors.Contacts;
            foreach (int victimIndex in actors.VictimIndices)
            {
                scene.Participants[victimIndex].IsVictim = true;
            }

            foreach (int aggressorIndex in actors.AggressorIndices)
            {
                scene.Participants[aggressorIndex].IsAggressor = true;
            }

            EnvironmentGuess env = scene.GuessEnvironmentContacts();
            scene.EnvironmentContacts = env.Contacts;
            foreach (int victimIndex in env.VictimIndices)
            {
                scene.Participants[victimIndex].IsVictim = true;
            }

            scene.MakeAlive();
        }
    }
}