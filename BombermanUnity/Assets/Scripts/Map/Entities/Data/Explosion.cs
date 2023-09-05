using UnityEngine;

namespace Map.Entities.Data
{
    public class Explosion
    {
        public Vector3 Epicenter;
        public float Radius;
        public float Power;

        public Explosion(Vector3 epicenter, float radius, float power)
        {
            Epicenter = epicenter;
            Radius = radius;
            Power = power;
        }
    }
}