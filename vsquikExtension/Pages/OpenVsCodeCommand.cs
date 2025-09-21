using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Diagnostics;

namespace vsquikExtension
{
    internal sealed class OpenVsCodeCommand : InvokableCommand
    {
        private readonly string _path;

        public OpenVsCodeCommand(string path)
        {
            _path = path;
        }

        public override string Name => "Open in VS Code";

        public override IconInfo Icon => new("\uE8A7"); // Optional: use an appropriate icon

        public override CommandResult Invoke()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c code \"{_path}\"",
                CreateNoWindow = true,
                UseShellExecute = false
            });
            return CommandResult.Hide();
        }

    }
}
