
namespace Hax;

public class GodCommand : ICommand {
    public void Execute(string[] args) {
        Settings.EnableGodMode = !Settings.EnableGodMode;
        Terminal.Print("SYSTEM", $"God mode: {(Settings.EnableGodMode ? "Enabled" : "Disabled")}");
    }
}
