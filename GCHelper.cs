using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MauiActivityMemoryleak;

public class GCHelper(IServiceProvider serviceProvider)
{
    private readonly Dictionary<Type, List<WeakReference>> _monitoredInstances = [];

    private readonly ILogger _logger = serviceProvider.GetRequiredService<ILogger<GCHelper>>();

    public void MonitorInstance(object instance)
    {
        lock (_monitoredInstances)
        {
            var weakRef = new WeakReference(instance);
            var type = instance.GetType();
            if (_monitoredInstances.TryGetValue(type, out var list))
                list.Add(weakRef);
            else
                _monitoredInstances[type] = [weakRef];

            LogInstanceCount(type);
        }
    }

    private void LogInstanceCount(Type type)
    {
        lock (_monitoredInstances)
        {
            _logger.LogInformation("Instance count ({Type}): {InstanceCount}", type.FullName, _monitoredInstances[type].Count);
        }
    }

    public async void Run()
    {
        while (true)
        {
            _logger.LogInformation("GC.Collect");
            Java.Lang.Runtime.GetRuntime().Gc();
            GC.Collect(int.MaxValue, GCCollectionMode.Forced, true, true);
            GC.WaitForPendingFinalizers();

            await Task.Delay(5000);

            lock (_monitoredInstances)
            {
                foreach (var typeEntry in _monitoredInstances)
                {
                    var removedCount = typeEntry.Value.RemoveAll(weakRef => !weakRef.IsAlive);
                    if (removedCount > 0)
                        LogInstanceCount(typeEntry.Key);
                }
            }
        }
    }
}
