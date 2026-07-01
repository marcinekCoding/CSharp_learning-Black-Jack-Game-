using Avalonia;
using Avalonia.Headless;
using Avalonia.Threading;
using MojeLogowanieGUI;

[assembly: Xunit.CollectionBehavior(DisableTestParallelization = true)]

namespace testy_jednostkowe;

public static class AvaloniaTestSetup
{
    private static readonly object Lock = new();
    private static bool _initialized;

    public static void EnsureInitialized()
    {
        if (_initialized)
        {
            return;
        }

        lock (Lock)
        {
            if (_initialized)
            {
                return;
            }

            AppBuilder.Configure<App>()
                .UseHeadless(new AvaloniaHeadlessPlatformOptions())
                .SetupWithoutStarting();

            _initialized = true;
        }
    }

    public static void RunOnUiThread(Action action)
    {
        EnsureInitialized();
        Dispatcher.UIThread.Invoke(action);
    }

    public static T RunOnUiThread<T>(Func<T> func)
    {
        EnsureInitialized();
        return Dispatcher.UIThread.Invoke(func);
    }
}
