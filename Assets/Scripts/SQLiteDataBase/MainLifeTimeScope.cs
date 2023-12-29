using SQLiteDataBase;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainLifeTimeScope : LifetimeScope
{
    [SerializeField]
    private SQLiteDataBaseSettings setting;

    protected override void Configure(IContainerBuilder builder)
    { 
        builder.Register<TestTableRepository>(Lifetime.Singleton);
        builder.Register<MasterMemoryData>(Lifetime.Singleton);
        builder.RegisterInstance(setting);
    }
}
