using Managers;
using Map.Entities.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Map.Entities
{
    public class Character : MonoBehaviour, ICharacter
    {
        [SerializeField] private bool _enemy;
        [SerializeField] private int _healthPoints;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Animator _animator;

        private int _health;
        
        private static readonly int DieName = Animator.StringToHash("die");
        private static readonly int AttackName = Animator.StringToHash("attack");

        public float Health => (float)_health / _healthPoints;
        public bool Enemy => _enemy;

        public int GetId => gameObject.GetInstanceID();
        public Vector3 Position => transform.position;

        private void OnEnable()
        {
            BombManager.OnExplosion += OnExplosion;
        }

        private void OnDisable()
        {
            BombManager.OnExplosion -= OnExplosion;
        }

        private void OnExplosion(Explosion explosion)
        {
            var distance = Vector3.Distance(explosion.Epicenter, transform.position);
            if(distance > explosion.Radius)
                return;
            //percentage of damage depending on the distance to the explosion
            var k = (explosion.Radius - distance) / explosion.Radius;
            var damage = (int)(explosion.Power * k);
            Damage(damage);
        }

        public void Reset()
        {
            gameObject.SetActive(true);
            _health = _healthPoints;
            _healthBar.value = Health;
            _healthBar.gameObject.SetActive(true);
        }

        /// <summary>
        /// Damage the current character
        /// </summary>
        /// <param name="damage">amount of damage</param>
        public void Damage(int damage)
        {
            if (_health <= 0)
                return;
            
            _health -= damage;
            if (_health < 0)
                _health = 0;
            _healthBar.value = Health;
            if (Health == 0)
            {
                Kill();
            }
        }

        private void Kill()
        {
            _animator.SetTrigger(DieName);
            _healthBar.gameObject.SetActive(false);
        }
        
        public void Attack(string bombId, float strength)
        {
            _animator.SetTrigger(AttackName);
        }

        /// <summary>
        /// Get character data that can be saved
        /// </summary>
        public CharacterSave GetSave()
        {
            return new CharacterSave
            {
                Health = _health,
                InstanceId = gameObject.GetInstanceID()
            };
        }

        /// <summary>
        /// Unpack data from the save
        /// </summary>
        public void UnpackSave(CharacterSave save)
        {
            _health = save.Health;
            _healthBar.value = Health;
            if (Health <= 0)
            {
                Kill();
            }
        }
    }
}