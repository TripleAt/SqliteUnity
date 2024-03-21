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
#if Connection_MasterMemory || !UNITY_EDITOR 
        builder.Register<ITableRepository, TestTableRepository>(Lifetime.Singleton);
#else
        builder.Register<ITableRepository, TestMasterRepository>(Lifetime.Singleton);
#endif
        builder.Register<MasterMemoryData>(Lifetime.Singleton);
        builder.RegisterInstance(setting);
    }
}
