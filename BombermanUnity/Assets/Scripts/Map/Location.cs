using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Factories;
using Managers;
using UnityEngine;

namespace Map
{
    public class Location : MonoBehaviour
    {
        [SerializeField] private BombFactory bombFactory;
        [SerializeField] private Character[] characters;
        [SerializeField] private Character selectedCharacter;

        /// <summary>
        /// turn on the location and apply the save
        /// </summary>
        /// <param name="locationSave"></param>
        public void Enable(LocationSave locationSave)
        {
            gameObject.SetActive(true);
            foreach (var character in characters)
            {
                character.Reset();
                if (locationSave.characters != null && locationSave.characters.Any(c => c.instanceId == character.gameObject.GetInstanceID()))
                {
                    var save = locationSave.characters.First(c => c.instanceId == character.gameObject.GetInstanceID());
                    character.UnpackSave(save);
                }
            }
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Throw a bomb
        /// </summary>
        /// <param name="strength">Throwing power. Between 0-1</param>
        public async Task Throw(string bombId, float strength)
        {
            var bomb = bombFactory.GetBomb(bombId);
            bomb.SetLocation(this);
            var nearestEnemy = characters.Where(c => c.Enemy && c.Health > 0).OrderBy(c =>
                Vector3.Distance(selectedCharacter.transform.position, c.transform.position)).First();
            var direction = nearestEnemy.transform.position - selectedCharacter.transform.position;

            selectedCharacter.Attack();
            await bomb.Throw(selectedCharacter.transform.position, direction, strength);
        }

        /// <summary>
        /// Explosion at a specific point. Damage to characters will be inflicted depending on the epicenter of the explosion
        /// </summary>
        /// <param name="position">Epicenter of the explosion</param>
        /// <param name="explosionPower">maximum damage that can be caused by this explosion</param>
        public void Explosion(Vector3 position, float explosionRadius, int explosionPower)
        {
            foreach (var character in characters)
            {
                var distance = Vector3.Distance(position, character.transform.position);
                if(distance > explosionRadius)
                    continue;
                //percentage of damage depending on the distance to the explosion
                var k = (explosionRadius - distance) / explosionRadius;
                var damage = (int)(explosionPower * k);
                character.Damage(damage);
            }

            if (characters.Where(c => !c.Enemy).All(c => c.Health <= 0))
            {
                //loose
                GameManager.Instance.FinishGame(false);
            }
            if (characters.Where(c => c.Enemy).All(c => c.Health <= 0))
            {
                //win
                GameManager.Instance.FinishGame(true);
            }
        }

        /// <summary>
        /// Get a location save that can be used later
        /// </summary>
        /// <returns></returns>
        public LocationSave GetProgress()
        {
            var progress = new LocationSave()
            {
                characters = characters.Select(character => character.GetSave()).ToList()
            };
            return progress;
        }
    }
    
    /// <summary>
    /// Data that stores the state of the location
    /// </summary>
    [Serializable]
    public struct LocationSave
    {
        public List<CharacterSave> characters;
    }
}