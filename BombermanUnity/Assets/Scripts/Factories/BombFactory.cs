using System.Collections.Generic;
using System.Linq;
using Map.Entities;
using UnityEngine;

namespace Factories
{
    /// <summary>
    /// Pool-factory that makes <see cref="IBomb"/>
    /// </summary>
    public class BombFactory : MonoBehaviour
    {
        [SerializeField] GameObject[] _bombPrototype;

        /// <summary>
        /// Bombs that can be reused
        /// </summary>
        private readonly Dictionary<string, Queue<IBomb>> _bombInstances = new Dictionary<string, Queue<IBomb>>();
        /// <summary>
        /// Bombs that are currently in use
        /// </summary>
        private readonly Dictionary<string, List<IBomb>> _usedBombInstances = new Dictionary<string, List<IBomb>>();

        /// <summary>
        /// Get a vacant or new bomb
        /// </summary>
        public IBomb GetBomb(string bombId)
        {
            if (_bombInstances.ContainsKey(bombId) && _bombInstances[bombId].Count > 0)
            {
                var bomb = _bombInstances[bombId].Dequeue();
                SetUsed(bombId, bomb);
                return bomb;
            }

            var bombTemplate = _bombPrototype.FirstOrDefault(x => x.name == bombId);
            if (bombTemplate == default)
            {
                Debug.LogError($"Bomb with id {bombId} not found");
                return null;
            }
            
            var newBomb = bombTemplate.GetComponent<IBomb>().GetCopy();
            newBomb.BackToPool = () => ReturnBomb(newBomb);
            SetUsed(bombId, newBomb);
            return newBomb;
        }

        private void ReturnBomb(IBomb bomb)
        {
            if(!_bombInstances.ContainsKey(bomb.Id))
                _bombInstances.Add(bomb.Id, new Queue<IBomb>());
            
            _usedBombInstances[bomb.Id].Remove(bomb);
            _bombInstances[bomb.Id].Enqueue(bomb);
            bomb.BackToPoolHook();
        }
        
        private void SetUsed(string bombId, IBomb bomb)
        {
            if(!_usedBombInstances.ContainsKey(bombId))
                _usedBombInstances.Add(bombId, new List<IBomb>());
            _usedBombInstances[bombId].Add(bomb);
            bomb.GetFromPoolHook();
        }
    }
}