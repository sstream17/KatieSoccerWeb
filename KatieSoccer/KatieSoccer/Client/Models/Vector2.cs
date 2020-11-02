using System;

namespace KatieSoccer.Client.Models
{
    public class Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public double Direction
        {
            get
            {
                return Math.Tanh(X / Y);
            }
        }

        public double Magnitude
        {
            get
            {
                return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            }
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator -(Vector2 a) => new Vector2(-a.X, -a.Y);

        public static Vector2 operator +(Vector2 a, Vector2 b)
            => new Vector2(a.X + b.X, a.Y + b.Y);

        public static Vector2 operator *(Vector2 a, float b)
            => new Vector2(a.X * b, a.Y * b);
    }
}
