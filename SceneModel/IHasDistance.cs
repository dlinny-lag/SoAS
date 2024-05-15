namespace SceneModel
{
    public interface IHasDistance<in T>
        where T : IHasDistance<T>
    {
        ulong Distance(T other);
    }
}