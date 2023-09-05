using System.Linq;
using System.Threading.Tasks;
using Managers;
using UnityEngine;

namespace Map.Entities
{
    public class Personage : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private BombManager _bombManager;
        [SerializeField] private Location _location;

        public async Task ThrowBomb(string bombId, float strength)
        {
            var nearestEnemy = _location.GetCharacters().Where(c => c.Enemy && c.Health > 0).OrderBy(c =>
                Vector3.Distance(_character.Position, c.Position)).First();
            var direction = nearestEnemy.Position - _character.Position;
            
            _character.Attack(bombId, strength);
            await _bombManager.Throw(bombId, _character.Position, direction, strength);
        }
    }
}