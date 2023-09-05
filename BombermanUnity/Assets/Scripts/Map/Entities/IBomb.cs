using System;
using System.Threading.Tasks;
using Map.Entities.Data;
using UnityEngine;

namespace Map.Entities
{
    public interface IBomb
    {
        public Action BackToPool { get; set; }
        public string Id { get; }
        public AbstractBomb GetCopy();
        public void BackToPoolHook();
        public void GetFromPoolHook();
        public Task<Explosion> Throw(Vector3 from, Vector3 direction, float throwStrength);
    }
}