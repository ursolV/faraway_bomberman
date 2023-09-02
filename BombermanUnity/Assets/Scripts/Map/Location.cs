using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Managers;
using UnityEngine;

namespace Map
{
    public class Location : MonoBehaviour
    {
        [SerializeField] private Bomb[] bombs;
        [SerializeField] private Character[] characters;
        [SerializeField] private Character selectedCharacter;

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

            foreach (var bomb in bombs)
            {
                bomb.Initialize(this);
            }
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public async Task Throw(string bombId, float strength)
        {
            var bomb = bombs.First(b => b.name == bombId);
            var nearestEnemy = characters.Where(c => c.Enemy && c.Health > 0).OrderBy(c =>
                Vector3.Distance(selectedCharacter.transform.position, c.transform.position)).First();
            var direction = nearestEnemy.transform.position - selectedCharacter.transform.position;
            bomb.Throw(selectedCharacter.transform.position, direction, strength);

            selectedCharacter.Attack();
            //todo
            await Task.Delay(2000);
        }

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

        public LocationSave GetProgress()
        {
            var progress = new LocationSave()
            {
                characters = characters.Select(character => character.GetSave()).ToList()
            };
            return progress;
        }
    }
    
    [Serializable]
    public struct LocationSave
    {
        public List<CharacterSave> characters;
    }
}