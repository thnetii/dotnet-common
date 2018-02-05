namespace THNETII.DependencyInjection.Nesting
{
    public enum RootServicesAddBehavior
    {
        None = 0,
        Add = 1,
        TryAdd = 2,
        TryAddEnumerable = 3,
        Replace = 4,
#if NETSTANDARD2_0
        ReplaceAll = 5 
#endif
    }
}
