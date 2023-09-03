using System.Collections.Generic;
using System.Linq;
using Map;
using UnityEngine;

namespace Factories
{
    /// <summary>
    /// pool-factory that makes <see cref="BaseBomb"/>
    /// </summary>
    public class BombFactory : MonoBehaviour
    {
        [SerializeField] BaseBomb[] bombPrototype;
        
        /// <summary>
        /// bombs that can be reused
        /// </summary>
        private readonly Dictionary<string, Queue<BaseBomb>> _bombInstances = new Dictionary<string, Queue<BaseBomb>>();
        /// <summary>
        /// bombs that are currently in use
        /// </summary>
        private readonly Dictionary<string, List<BaseBomb>> _usedBombInstances = new Dictionary<string, List<BaseBomb>>();

        /// <summary>
        /// get a vacant or new bomb
        /// </summary>
        /// <param name="bombId"></param>
        /// <returns></returns>
        public BaseBomb GetBomb(string bombId)
        {
            if (_bombInstances.ContainsKey(bombId) && _bombInstances[bombId].Count > 0)
            {
                var bomb = _bombInstances[bombId].Dequeue();
                SetUsed(bombId, bomb);
                return bomb;
            }

            var bombTemplate = bombPrototype.FirstOrDefault(x => x.Id() == bombId);
            if (bombTemplate == default)
            {
                Debug.LogError($"Bomb with id {bombId} not found");
                return null;
            }
            
            var newBomb = bombTemplate.GetCopy();
            newBomb.BackToPool = () => ReturnBomb(newBomb);
            SetUsed(bombId, newBomb);
            return newBomb;
        }

        private void ReturnBomb(BaseBomb bomb)
        {
            if(!_bombInstances.ContainsKey(bomb.Id()))
                _bombInstances.Add(bomb.Id(), new Queue<BaseBomb>());
            
            _usedBombInstances[bomb.Id()].Remove(bomb);
            _bombInstances[bomb.Id()].Enqueue(bomb);
            bomb.BackToPoolHook();
        }
        
        private void SetUsed(string bombId, BaseBomb bomb)
        {
            if(!_usedBombInstances.ContainsKey(bombId))
                _usedBombInstances.Add(bombId, new List<BaseBomb>());
            _usedBombInstances[bombId].Add(bomb);
            bomb.GetFromPoolHook();
        }
    }
}