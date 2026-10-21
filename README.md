# VerificationMonitor——插件验证监控

VerificationMonitor 是一款基于 Dalamud 框架的《最终幻想14》插件，用于监控已加载插件的验证到期状态并自动启动 AEAssist。插件提取自 Keita 工具箱，将 AEAssist 启动自动化与插件验证监控两个功能整合为独立插件，当前版本为 0.0.0.1。

## 主要功能

**AEAssist 启动自动化。** 游戏启动后自动检查 AEAssist Loader 的更新状态，依次调用 Loader 内置的检查更新、下载更新与加载入口。Loader 未加载时自动发送启用命令，更新卡滞时自动重试。

**验证码自动提交。** 角色登录后读取 AEAssist 自行保存的验证码，按高级码优先、当前码次优的顺序依次提交验证，无需手动输入。本插件不保存或输出任何验证码。

**多插件验证到期监控。** 统一读取已加载插件的验证到期时间，覆盖 Daily Routines、MissFisher、CharaCradThief、PvPAuto、NyaDraw、Kodakku Assist 与 OmniToolbox 等插件。每 30 秒刷新一次，支持从插件内部反射、缓存回退到认证节点状态检测等多种读取策略。

**服务器信息栏状态点。** 在服务器信息栏（DTR Bar）显示彩色状态点：绿色表示正常，黄色表示 2 天内到期，红色表示已过期，灰色表示状态不完整。点击状态点可打开 Discord 验证入口窗口。

**到期提醒。** 验证到期前 2 天触发提醒，同一到期时间只提醒一次。支持 Dalamud 通知中心、游戏原生 Toast 与聊天栏本地提示三种渠道，可分别开关。

**Discord 验证入口。** 提供窗口集中展示各插件的验证状态与 Discord 认证入口按钮，优先唤起 Discord 客户端，客户端不可用时回退到浏览器打开。

## 命令

- `/vmon`——打开设置窗口

## 项目结构

```
VerificationMonitor/
├── Plugin.cs                         插件生命周期、Feature 实例化与帧循环更新
├── Plugin.Ui.cs                      设置窗口绘制与 UI 辅助方法（换行文本、帮助提示、功能开关）
├── Configuration.cs                  插件配置（AEAssistStartupSettings + VerificationMonitorSettings）
├── AeAssistStartupFeature.cs         AEAssist 启动自动化：Loader 检测、更新、加载与验证码提交
├── VerificationMonitorFeature.cs     验证到期监控：反射读取、DTR 状态点、到期提醒与 Discord 入口
└── VerificationMonitor.json          Dalamud 插件清单
```

## 构建

1. 打开 `VerificationMonitor.slnx`。
2. 构建解决方案（Debug 或 Release）。构建环境需安装 .NET 10 SDK。
3. 将生成的 DLL 添加至 Dalamud 的 Dev Plugin Locations。

## 技术参考

- 功能代码提取自 [KeitaToolbox](https://github.com/zhui-zi/DalamudPlugins)，作者 zhui-zi。
- 项目模板参考 [SamplePlugin](https://github.com/goatcorp/SamplePlugin)。

## 许可证

本插件以 MIT 许可证发布。
