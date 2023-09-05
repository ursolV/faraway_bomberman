using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Map.Entities;
using Map.Entities.Data;
using UnityEngine;

namespace Map
{
    public class Location : MonoBehaviour
    {
        [SerializeField] private Character[] _characters;
        [SerializeField] private Personage _personage;

        /// <summary>
        /// Turn on the location and apply the save
        /// </summary>
        public void Enable(LocationSave locationSave)
        {
            BombManager.OnExplosion += OnExplosion;
            gameObject.SetActive(true);
            foreach (var character in _characters)
            {
                character.Reset();
                if (locationSave.characters != null && locationSave.characters.Any(c => c.InstanceId == character.GetId))
                {
                    var save = locationSave.characters.First(c => c.InstanceId == character.GetId);
                    character.UnpackSave(save);
                }
            }
            
            BombManager.OnExplosion += OnExplosion;
        }

        private void OnExplosion(Explosion obj)
        {
            if (_characters.Where(c => !c.Enemy).All(c => c.Health <= 0))
            {
                //loose
                GameManager.Instance.FinishGame(false);
            }
            if (_characters.Where(c => c.Enemy).All(c => c.Health <= 0))
            {
                //win
                GameManager.Instance.FinishGame(true);
            }
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            BombManager.OnExplosion -= OnExplosion;
        }

        public ICharacter[] GetCharacters()
        {
            return _characters;
        }

        public Personage GetPersonage()
        {
            return _personage;
        }

        /// <summary>
        /// Get a location save that can be used later
        /// </summary>
        public LocationSave GetProgress()
        {
            var progress = new LocationSave()
            {
                characters = _characters.Select(character => character.GetSave()).ToList()
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