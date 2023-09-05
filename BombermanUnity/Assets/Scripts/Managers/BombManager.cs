using System;
using System.Threading.Tasks;
using Factories;
using Map.Entities.Data;
using UnityEngine;

namespace Managers
{
    public class BombManager : MonoBehaviour
    {
        [SerializeField] private BombFactory _bombFactory;
        
        public static event Action<Explosion> OnExplosion;

        public async Task Throw(string bombId, Vector3 from, Vector3 direction, float strength)
        {
            var bomb = _bombFactory.GetBomb(bombId);

            var explosion = await bomb.Throw(from, direction, strength);
            OnExplosion?.Invoke(explosion);
        }
    }
}