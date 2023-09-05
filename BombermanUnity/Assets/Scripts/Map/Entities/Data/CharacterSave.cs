using System;

namespace Map.Entities.Data
{
    /// <summary>
    /// Data that stores the state of the unit
    /// </summary>
    [Serializable]
    public class CharacterSave
    {
        public int InstanceId;
        public int Health;
    }
}