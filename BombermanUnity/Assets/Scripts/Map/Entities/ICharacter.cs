using Map.Entities.Data;
using UnityEngine;

namespace Map.Entities
{
    public interface ICharacter
    {
        public bool Enemy { get; }
        public float Health { get; }
        public int GetId { get; }
        public Vector3 Position { get; }

        public void Reset();
        public CharacterSave GetSave();
        public void UnpackSave(CharacterSave save);
    }
}