using System.Threading.Tasks;
using Map.Entities.Data;
using UnityEngine;

namespace Map.Entities
{
    public class Bomb : AbstractBomb
    {
        [SerializeField] private float _maxThrowDistance;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private int _explosionPower;
        [SerializeField] private float _speed;
        [SerializeField] private SpriteRenderer _bombRenderer;
        [SerializeField] private GameObject _aim;
        [SerializeField] private GameObject _explosionFX;
        [SerializeField] private AnimationCurve _curve;

        /// <summary>
        /// Throw this bomb
        /// </summary>
        /// <param name="from">start point</param>
        /// <param name="direction">throw direction</param>
        /// <param name="throwStrength">between 0-1</param>
        public override async Task<Explosion> Throw(Vector3 from, Vector3 direction, float throwStrength)
        {
            var arrivePoint = from + direction.normalized * _maxThrowDistance * throwStrength;
            transform.position = from;
            return await Fly(arrivePoint);
        }

        /// <summary>
        /// Visual fly from current position to destination
        /// </summary>
        /// <param name="destination">arrive point</param>
        private async Task<Explosion> Fly(Vector3 destination)
        {
            var start = transform.position;
            var distance = Vector3.Distance(start, destination);
            float i = 0;
            while (i < 1)
            {
                var pos = Vector3.Lerp(start, destination, i);
                //todo convert to a formula
                pos.y += _curve.Evaluate(i) * distance;
                transform.position = pos;
                _aim.transform.position = destination;
                i += Time.deltaTime * (_speed / distance);
                await Task.Yield();
            }
            
            ShowExplosionFX();
            
            return new Explosion(destination, _explosionRadius, _explosionPower);
        }

        private async void ShowExplosionFX()
        {
            _bombRenderer.enabled = false;
            _aim.SetActive(false);
            _explosionFX.SetActive(true);
            //better to get the duration of the fx
            await Task.Delay(2000);
            BackToPool();
        }

        public override AbstractBomb GetCopy()
        {
            var bomb = Instantiate(gameObject).GetComponent<Bomb>();
            bomb.gameObject.name = Id;
            return bomb;
        }

        public override void BackToPoolHook()
        {
            gameObject.SetActive(false);
        }

        public override void GetFromPoolHook()
        {
            _bombRenderer.enabled = true;
            _aim.SetActive(true);
            gameObject.SetActive(true);
            _explosionFX.SetActive(false);
        }
    }
}