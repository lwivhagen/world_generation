using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace world_generation.scripts.other
{
    public partial class CameraController : Camera2D
    {
        private const float ZoomSpeed = 0.1f;

        [Export]
        private float MoveSpeed = 20f;

        private Vector2 _targetPosition;

        [Export]
        private float _targetZoom = 1;

        public override void _Process(double delta)
        {
            // Move the camera towards the target position
            Position = Position.Lerp(_targetPosition, (float)(MoveSpeed * delta));

            // Zoom the camera towards the target zoom
            float zoomFac = Mathf.Lerp(Zoom.X, _targetZoom, (float)(ZoomSpeed * delta));
            zoomFac = Math.Clamp(zoomFac, 0.1f, 10f);
            if ((Zoom.X - zoomFac) < 0.1f)
            {
                zoomFac = Zoom.X;
            }
            Zoom = new Vector2(zoomFac, zoomFac);

            // Handle input controls
            Vector2 input = new Vector2();
            if (Input.IsActionPressed("ui_right"))
            {
                input.X += 1;
            }
            if (Input.IsActionPressed("ui_left"))
            {
                input.X -= 1;
            }
            if (Input.IsActionPressed("ui_down"))
            {
                input.Y += 1;
            }
            if (Input.IsActionPressed("ui_up"))
            {
                input.Y -= 1;
            }
            input = input.Normalized();

            // Move the target position based on input
            _targetPosition += input * MoveSpeed * (float)delta;
        }

        public void SetTargetPosition(Vector2 targetPosition)
        {
            _targetPosition = targetPosition;
        }

        public void SetTargetZoom(float targetZoom)
        {
            _targetZoom = targetZoom;
        }
    }
}
