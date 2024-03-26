using AAF.Services.AAFImport;
using AAFModel;
using SceneModel;

namespace SceneServices.Scenes
{
    public static class AAFHelper
    {
        internal static Sex ToSex(this string gender)
        {
            return gender.ToSex<Sex>();
        }

        internal static SceneType ToSceneType(this ReferenceType type)
        {
            return (SceneType)(type);
        }
    }
}