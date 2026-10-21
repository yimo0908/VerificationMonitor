using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;

namespace VerificationMonitor;

public sealed partial class Plugin
{
    private void DrawWindow()
    {
        if (!windowOpen)
            return;

        ImGui.SetNextWindowSize(new Vector2(620, 520), ImGuiCond.FirstUseEver);
        if (!ImGui.Begin("Verification Monitor", ref windowOpen))
        {
            ImGui.End();
            return;
        }

        DrawDisabledWrapped("AEAssist 启动自动化与插件验证监控；修改后立即保存。");
        ImGui.Separator();
        ImGui.Spacing();

        if (aeAssistStartupFeature != null)
        {
            aeAssistStartupFeature.DrawSettings();
            ImGui.Separator();
        }
        else
        {
            DrawUnavailable("AEAssist 启动自动化");
            ImGui.Separator();
        }

        if (verificationMonitorFeature != null)
        {
            if (ImGui.CollapsingHeader("插件验证监控", ImGuiTreeNodeFlags.DefaultOpen))
                verificationMonitorFeature.DrawSettings();
        }
        else
        {
            DrawUnavailable("插件验证监控");
        }

        ImGui.End();
    }

    internal static bool DrawFeatureToggle(string label, bool value, Action<bool> setter)
    {
        var changedValue = value;
        if (!ImGui.Checkbox($"启用{label}", ref changedValue))
            return false;

        setter(changedValue);
        Config.Save();
        return true;
    }

    internal static void DrawHelp(string text)
    {
        ImGui.Indent();
        DrawDisabledWrapped(text);
        ImGui.Unindent();
        ImGui.Spacing();
    }

    internal static void DrawDisabledWrapped(string text)
    {
        var wrapPosition = ImGui.GetCursorPosX() + ImGui.GetContentRegionAvail().X;
        ImGui.PushTextWrapPos(wrapPosition);
        ImGui.TextDisabled(PreservePhraseSpacing(text));
        ImGui.PopTextWrapPos();
    }

    internal static void DrawColoredWrapped(Vector4 color, string text)
    {
        var wrapPosition = ImGui.GetCursorPosX() + ImGui.GetContentRegionAvail().X;
        ImGui.PushTextWrapPos(wrapPosition);
        ImGui.TextColored(color, PreservePhraseSpacing(text));
        ImGui.PopTextWrapPos();
    }

    internal static void DrawWrapped(string text) =>
        ImGui.TextWrapped(PreservePhraseSpacing(text));

    private static string PreservePhraseSpacing(string text) =>
        text.Contains(' ') ? text.Replace(' ', '\u00a0') : text;

    private static void DrawUnavailable(string name) =>
        DrawDisabledWrapped($"{name}当前不可用，请检查 Dalamud 日志。");
}
