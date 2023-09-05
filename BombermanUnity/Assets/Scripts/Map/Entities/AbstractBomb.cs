using System;
using System.Threading.Tasks;
using Map.Entities.Data;
using UnityEngine;

namespace Map.Entities
{
    public abstract class AbstractBomb : MonoBehaviour, IBomb
    {
        /// <summary>
        /// Сall to return an object to the pool
        /// </summary>
        public Action BackToPool { get; set; }

        public string Id => gameObject.name;
        /// <summary>
        /// Get a copy of the object. prototype pattern
        /// </summary>
        public abstract AbstractBomb GetCopy();
        public abstract void BackToPoolHook();
        public abstract void GetFromPoolHook();

        /// <summary>
        /// Throw a bomb. 
        /// </summary>
        /// <param name="from">The starting point of the flight</param>
        /// <param name="throwStrength">Throwing power. Between 0-1</param>
        public abstract Task<Explosion> Throw(Vector3 from, Vector3 direction, float throwStrength);
    }
}