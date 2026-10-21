using System;
using System.Collections.Generic;
using Dalamud.Configuration;
using Dalamud.Plugin;

namespace VerificationMonitor;

[Serializable]
public sealed class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public AeAssistStartupSettings AeAssistStartup { get; set; } = new();
    public VerificationMonitorSettings VerificationMonitor { get; set; } = new();

    [NonSerialized]
    private IDalamudPluginInterface? pluginInterface;

    public void Initialize(IDalamudPluginInterface value) => pluginInterface = value;

    public void Save() => pluginInterface?.SavePluginConfig(this);
}

[Serializable]
public sealed class AeAssistStartupSettings
{
    public bool Enabled { get; set; } = true;
    public bool PrintChatMessage { get; set; } = true;
}

[Serializable]
public sealed class VerificationMonitorSettings
{
    public bool Enabled { get; set; } = true;
    public bool ShowServerInfoBar { get; set; } = true;
    public bool NotifyWithDalamud { get; set; } = true;
    public bool NotifyWithGameToast { get; set; }
    public bool NotifyWithChat { get; set; }
    public Dictionary<string, long> LastNotifiedExpiryUnixSeconds { get; set; } = [];
    public Dictionary<string, long> LastKnownExpiryUnixSeconds { get; set; } = [];
}
