using System.Threading.Tasks;
using UnityEngine;

namespace Map
{
    public class Bomb : BaseBomb
    {
        [SerializeField] private float maxThrowDistance;
        [SerializeField] private float explosionRadius;
        [SerializeField] private int explosionPower;
        [SerializeField] private float speed;
        [SerializeField] private SpriteRenderer bombRenderer;
        [SerializeField] private GameObject aim;
        [SerializeField] private GameObject explosionFX;
        [SerializeField] private AnimationCurve curve;
        
        private Location _location;

        public override void SetLocation(Location location)
        {
            _location = location;
        }
        
        /// <summary>
        /// Throw this bomb
        /// </summary>
        /// <param name="from">start point</param>
        /// <param name="direction">throw direction</param>
        /// <param name="throwStrength">between 0-1</param>
        public override async Task Throw(Vector3 from, Vector3 direction, float throwStrength)
        {
            var arrivePoint = from + direction.normalized * maxThrowDistance * throwStrength;
            transform.position = from;
            await Fly(arrivePoint);
        }

        private async Task Fly(Vector3 destination)
        {
            var start = transform.position;
            var distance = Vector3.Distance(start, destination);
            float i = 0;
            while (i < 1)
            {
                var pos = Vector3.Lerp(start, destination, i);
                //todo convert to a formula
                pos.y += curve.Evaluate(i) * distance;
                transform.position = pos;
                aim.transform.position = destination;
                i += Time.deltaTime * (speed / distance);
                await Task.Yield();
            }

            _location.Explosion(destination, explosionRadius, explosionPower);

            ShowExplosionFX();
        }

        private async void ShowExplosionFX()
        {
            bombRenderer.enabled = false;
            aim.SetActive(false);
            explosionFX.SetActive(true);
            //better to get the duration of the fx
            await Task.Delay(2000);
            BackToPool();
        }

        public override string Id()
        {
            return gameObject.name;
        }

        public override BaseBomb GetCopy()
        {
            var bomb = Instantiate(gameObject).GetComponent<Bomb>();
            bomb.gameObject.name = Id();
            return bomb;
        }

        public override void BackToPoolHook()
        {
            gameObject.SetActive(false);
        }

        public override void GetFromPoolHook()
        {
            bombRenderer.enabled = true;
            aim.SetActive(true);
            gameObject.SetActive(true);
            explosionFX.SetActive(false);
        }
    }
}