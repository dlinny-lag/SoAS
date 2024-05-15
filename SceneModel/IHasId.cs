using System;

namespace SceneModel
{
    public interface IHasId
    {
        Guid Id { get; set; }
    }
}