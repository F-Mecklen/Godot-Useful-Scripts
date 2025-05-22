using Godot;
using System;
using DialogueManagerRuntime;

public partial class DialogueDebug : Area3D
{
    private bool _inRange = false;

    [Export] public string DialoguePath;
    [Export] public string StartNode = "Start";

    private PackedScene _balloonScene;

    public override void _Ready()
    {
        _balloonScene = GD.Load<PackedScene>("res://Dialogues/Balloon.tscn");

        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body.IsInGroup("Player"))
        {
            _inRange = true;
            GD.Print("inrange");
        }
    }

    private void OnBodyExited(Node3D body)
    {
        if (body.IsInGroup("Player"))
        {
            _inRange = false;
            GD.Print("outrange");
        }
    }

    public override void _Process(double delta)
    {
        if (_inRange && Input.IsActionJustPressed("Interact"))
        {
            GD.Print("interact");

            var dialogue = ResourceLoader.Load(DialoguePath);

            if (dialogue != null)
            {
                
                var balloonScene = GD.Load<PackedScene>("res://Dialogues/Balloon.tscn");
                var balloonInstance = balloonScene.Instantiate();

                
                GetTree().CurrentScene.AddChild(balloonInstance);

                
                if (balloonInstance is ExampleBalloon balloon)
                {
                    
                    balloon.Start(dialogue, StartNode);
                }
                else
                {
                    GD.PrintErr("Balloon instance is not of type ExampleBalloon");
                }
            }
            else
            {
                GD.PrintErr($"Failed to load dialogue resource at: {DialoguePath}");
            }
        }

    }
}