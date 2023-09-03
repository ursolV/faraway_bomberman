using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Map
{
    public abstract class BaseBomb : MonoBehaviour
    {
        /// <summary>
        /// Сall to return an object to the pool
        /// </summary>
        public Action BackToPool;
        public abstract string Id();
        /// <summary>
        /// Get a copy of the object. prototype pattern
        /// </summary>
        /// <returns></returns>
        public abstract BaseBomb GetCopy();
        public abstract void BackToPoolHook();
        public abstract void GetFromPoolHook();

        /// <summary>
        /// Throw a bomb. 
        /// </summary>
        /// <param name="from">The starting point of the flight</param>
        /// <param name="throwStrength">Throwing power. Between 0-1</param>
        /// <returns></returns>
        public abstract Task Throw(Vector3 from, Vector3 direction, float throwStrength);

        /// <summary>
        /// Set the location. Characters that are at this location will receive damage
        /// </summary>
        public abstract void SetLocation(Location location);
    }
}