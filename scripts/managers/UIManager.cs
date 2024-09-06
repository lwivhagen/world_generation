using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace world_generation.scripts.managers
{
    public partial class UIManager : CanvasLayer
    {
        [Export]
        private Label fpsCounter;

        public override void _Ready()
        {
            if (fpsCounter == null)
                fpsCounter = GetNode<Label>("FPSCounter");
            Engine.MaxFps = 0;
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            fpsCounter.Text = "FPS: " + Engine.GetFramesPerSecond();
        }
    }
}
