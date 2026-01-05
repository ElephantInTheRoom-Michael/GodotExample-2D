using Godot;

namespace DodgeTheCreeps;

public partial class Main : Node
{
    [Export]
    public PackedScene MobScene { get; set; }

    private int _score;
    
    private Hud _hud;
    private AudioStreamPlayer _musicPlayer;
    private AudioStreamPlayer _deathSoundPlayer;

    public override void _Ready()
    {
        base._Ready();

        _hud = GetNode<Hud>("HUD");
        _musicPlayer = GetNode<AudioStreamPlayer>("Music");
        _deathSoundPlayer = GetNode<AudioStreamPlayer>("DeathSound");
    }

    public void GameOver()
    {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
        
        _hud.ShowGameOver();
        
        _musicPlayer.Stop();
        _deathSoundPlayer.Play();
    }

    public void NewGame()
    {
        _score = 0;

        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);

        GetNode<Timer>("StartTimer").Start();
        
        _hud.UpdateScore(_score);
        _hud.ShowMessage("Get Ready!");
        
        GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
        
        _musicPlayer.Play();
    }
    
    private void OnScoreTimerTimeout()
    {
        _score++;
        _hud.UpdateScore(_score);
    }

    private void OnStartTimerTimeout()
    {
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }
    
    private void OnMobTimerTimeout()
    {
        var mob = MobScene.Instantiate<Mob>();

        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.ProgressRatio = GD.Randf();
        mob.Position = mobSpawnLocation.Position;

        // Set the mob's direction perpendicular to the path direction with some randomness.
        var direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
        direction += GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mob.Rotation = direction;

        var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
        mob.LinearVelocity = velocity.Rotated(direction);

        AddChild(mob);
    }
}