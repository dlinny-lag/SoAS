using System.Collections.Generic;
using AAF.Services.AAFImport;
using SceneModel;

namespace SceneServices
{
    public class BuildResult
    {
        public readonly IList<Scene> Scenes;
        public readonly AAFImportErrors Errors;
        public BuildResult(IList<Scene> scenes, AAFImportErrors errors)
        {
            Scenes = scenes;
            Errors = errors;
        }
    }
}