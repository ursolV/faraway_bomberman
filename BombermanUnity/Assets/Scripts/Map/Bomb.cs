using System.Collections;
using UnityEngine;

namespace Map
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private float maxThrowDistance;
        [SerializeField] private float explosionRadius;
        [SerializeField] private int explosionPower;
        [SerializeField] private float speed;
        [SerializeField] private GameObject explosionFX;
        [SerializeField] private AnimationCurve curve;
        
        private Location _location;

        public void Initialize(Location location)
        {
            _location = location;
            explosionFX.SetActive(false);
        }
        
        /// <summary>
        /// Throw this bomb
        /// </summary>
        /// <param name="from">start point</param>
        /// <param name="direction">throw direction</param>
        /// <param name="throwStrength">between 0-1</param>
        public void Throw(Vector3 from, Vector3 direction, float throwStrength)
        {
            var arrivePoint = from + direction.normalized * maxThrowDistance * throwStrength;
            //todo show aim
            //todo show boom fx

            transform.position = from;
            StartCoroutine(Fly(arrivePoint));
        }

        private IEnumerator Fly(Vector3 destination)
        {
            var start = transform.position;
            var distance = Vector3.Distance(start, destination);
            float i = 0;
            while (i < 1)
            {
                var pos = Vector3.Lerp(start, destination, i);
                pos.y += curve.Evaluate(i) * distance;
                transform.position = pos;
                i += Time.deltaTime * (speed / distance);
                yield return null;
            }

            explosionFX.SetActive(true);
            _location.Explosion(destination, explosionRadius, explosionPower);

            yield return new WaitForSeconds(2);
            explosionFX.SetActive(false);
        }
    }
}