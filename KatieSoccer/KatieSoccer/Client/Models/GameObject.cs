using System;
using System.Linq;
using System.Threading.Tasks;

namespace KatieSoccer.Client.Models
{
    public class GameObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public bool IsMoving { get; private set; } = false;
        private bool WasMoving { get; set; } = false;

        private readonly float threshold = 0.1f;
        private const int noMovementFrames = 3;
        private readonly Vector2[] previousPositions = new Vector2[noMovementFrames];

        public GameObject()
        {
            Velocity = new Vector2(0, 0);

            InitializePreviousPositions();

            PhysicsUpdate();
        }

        private async void InitializePreviousPositions()
        {
            await Task.Run(() => Position != null);
            for (int i = 0; i < previousPositions.Length; i++)
            {
                previousPositions[i] = Position;
            }
        }

        private async void PhysicsUpdate()
        {
            while (true)
            {
                if (IsMoving)
                {
                    var unitVector = new Vector2(
                        Velocity.X / Convert.ToSingle(Velocity.Magnitude),
                        Velocity.Y / Convert.ToSingle(Velocity.Magnitude));

                    var force = -unitVector * 0.1f;
                    AddForce(force);

                    Position += Velocity;
                }
                else if (WasMoving && !IsMoving)
                {
                    Velocity = new Vector2(0, 0);
                }

                WasMoving = IsMoving;

                CheckIsMoving();

                await Task.Delay(10);
            }
        }

        public void AddForce(Vector2 force, bool isExternal = false)
        {
            if (isExternal)
            {
                IsMoving = isExternal;
            }

            Velocity += force;
        }

        private void CheckIsMoving()
        {
            if (previousPositions.Any(p => p == null))
            {
                IsMoving = false;
                return;
            }

            for (int i = 0; i < previousPositions.Length - 1; i++)
            {
                previousPositions[i] = previousPositions[i + 1];
            }
            previousPositions[^1] = Position;

            for (int i = 0; i < previousPositions.Length - 1; i++)
            {
                var distance = Vector2.Distance(previousPositions[i], previousPositions[i + 1]);
                if (distance >= threshold)
                {
                    //The minimum movement has been detected between frames
                    IsMoving = true;
                    break;
                }
                else
                {
                    IsMoving = false;
                }
            }
        }
    }
}
