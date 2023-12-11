namespace Application;
public static class AssemblyReference
{
    public static readonly Lazy<Assembly> LazyAssembly =
        new(Assembly.GetExecutingAssembly());
    public static Assembly Assembly => LazyAssembly.Value;
}
