using System;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace VerificationMonitor;

public sealed partial class Plugin : IDalamudPlugin
{
    private const string CommandName = "/vmon";

    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static IFramework Framework { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService] internal static IObjectTable ObjectTable { get; private set; } = null!;
    [PluginService] internal static IChatGui Chat { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IDtrBar DtrBar { get; private set; } = null!;
    [PluginService] internal static IToastGui Toasts { get; private set; } = null!;
    [PluginService] internal static INotificationManager Notifications { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;

    internal static Configuration Config { get; private set; } = null!;

    private AeAssistStartupFeature? aeAssistStartupFeature;
    private VerificationMonitorFeature? verificationMonitorFeature;
    private bool windowOpen;

    public Plugin()
    {
        Config = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Config.Initialize(PluginInterface);

        PluginInterface.UiBuilder.Draw += DrawWindow;
        PluginInterface.UiBuilder.OpenConfigUi += OpenWindow;
        PluginInterface.UiBuilder.OpenMainUi += OpenWindow;

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "打开 Verification Monitor 设置。",
        });

        aeAssistStartupFeature = CreateFeature("AEAssist startup automation", () => new AeAssistStartupFeature());
        verificationMonitorFeature = CreateFeature("plugin verification monitor", () => new VerificationMonitorFeature());

        Framework.Update += OnFrameworkUpdate;

        Log.Information("Verification Monitor loaded.");
    }

    public void Dispose()
    {
        Framework.Update -= OnFrameworkUpdate;
        CommandManager.RemoveHandler(CommandName);
        PluginInterface.UiBuilder.OpenMainUi -= OpenWindow;
        PluginInterface.UiBuilder.OpenConfigUi -= OpenWindow;
        PluginInterface.UiBuilder.Draw -= DrawWindow;

        verificationMonitorFeature?.Dispose();
        aeAssistStartupFeature?.Dispose();

        Log.Information("Verification Monitor disabled.");
    }

    private void OnFrameworkUpdate(IFramework _)
    {
        try
        {
            verificationMonitorFeature?.Update();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "A framework update failed.");
        }
    }

    private void OnCommand(string _, string __) => windowOpen = true;

    private void OpenWindow() => windowOpen = true;

    private static T? CreateFeature<T>(string name, Func<T> factory)
        where T : class, IDisposable
    {
        try
        {
            return factory();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to initialize feature: {Feature}.", name);
            return null;
        }
    }
}
