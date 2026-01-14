using Godot;

namespace DisplayManagerPlugin.addons.displaymanager;

public partial class DisplayManager : Node
{
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (!@event.IsActionPressed("fullscreen_toggle")) return;
        
        ToggleFullscreen();
        
        GetViewport().SetInputAsHandled();
    }

    private static void ToggleFullscreen()
    {
        GD.Print("Fullscreen toggle");
        if (IsFullscreen())
        {
            GD.Print("Switch to windowed display mode");
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }
        else
        {
            GD.Print("Switch to fullscreen display mode");
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
    }
    
    private static bool IsFullscreen()
    {
        return DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen;
    }
}