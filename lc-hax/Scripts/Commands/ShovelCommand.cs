
namespace Hax;

public class ShovelCommand : ICommand {
    public void Execute(string[] args) {
        if (args.Length < 1) {
            Helper.PrintSystem("Usage: /shovel <force=1>");
            return;
        }

        Settings.ShovelHitForce = int.TryParse(args[0], out int shovelHitForce) ? shovelHitForce : Settings.ShovelHitForce;
    }
}
