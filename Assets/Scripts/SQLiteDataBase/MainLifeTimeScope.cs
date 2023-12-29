using VContainer;
using VContainer.Unity;

public class MainLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    { 
        builder.Register<TestTableRepository>(Lifetime.Singleton);
        builder.Register<MasterMemoryData>(Lifetime.Singleton);
    }
}
