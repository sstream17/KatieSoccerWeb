using System;
using System.Threading.Tasks;

namespace KatieSoccer.Client.Models
{
    public class GameObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public GameObject()
        {
            Velocity = new Vector2(0, 0);

            PhysicsUpdate();
        }

        private async void PhysicsUpdate()
        {
            while (true)
            {
                if (Velocity.Magnitude > 0)
                {
                    var unitVector = new Vector2(
                        Velocity.X / Convert.ToSingle(Velocity.Magnitude),
                        Velocity.Y / Convert.ToSingle(Velocity.Magnitude));

                    var force = -unitVector * 0.1f;
                    AddForce(force);

                    Position += Velocity;
                }
                else
                {
                    Velocity = new Vector2(0, 0);
                }

                await Task.Delay(10);
            }
        }

        public void AddForce(Vector2 force)
        {
            Velocity += force;
        }
    }
}
