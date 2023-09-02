using System;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private bool enemy;
        [SerializeField] private int healthPoints;
        [SerializeField] private Slider healthBar;
        [SerializeField] private Animator animator;

        private int _health;
        
        private static readonly int DieName = Animator.StringToHash("die");
        private static readonly int AttackName = Animator.StringToHash("attack");

        public float Health => (float)_health / healthPoints;
        public bool Enemy => enemy;
        
        public void Reset()
        {
            gameObject.SetActive(true);
            _health = healthPoints;
            healthBar.value = Health;
            healthBar.gameObject.SetActive(true);
        }

        public void Damage(int damage)
        {
            if (_health <= 0)
                return;
            
            _health -= damage;
            if (_health < 0)
                _health = 0;
            healthBar.value = Health;
            if (Health == 0)
            {
                animator.SetTrigger(DieName);
                healthBar.gameObject.SetActive(false);
            }
        }

        public void Attack()
        {
            animator.SetTrigger(AttackName);
        }

        public CharacterSave GetSave()
        {
            return new CharacterSave
            {
                health = _health,
                instanceId = gameObject.GetInstanceID()
            };
        }

        public void UnpackSave(CharacterSave save)
        {
            _health = save.health;
            healthBar.value = Health;
        }
    }

    [Serializable]
    public struct CharacterSave
    {
        public int instanceId;
        public int health;
    }
}