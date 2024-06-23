using System;

namespace SceneModel
{
    /// <summary>
    /// PK = Primary Key
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class PKAttribute : Attribute
    {
        
    }
}