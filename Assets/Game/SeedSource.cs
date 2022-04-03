using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SeedSource : MonoBehaviour
    {
        public GameObject seedTemplate;
        private bool _used;

        public int minAmount = 2;
        public int maxAmount = 3;

        void Start()
        {
            if (!seedTemplate)
            {
                seedTemplate = AssetLibrary.Instance.seedItemTemplate;
            }
        }

        public void Pick()
        {
            if (_used) return;

            _used = true;
            GameManager.Instance.SpawnCoroutine(SpawnSeeds());
            Destroy(gameObject);
        }

        private IEnumerator SpawnSeeds()
        {
            var routineSeedTemplate = seedTemplate;
            var originPosition = transform.position;

            var rand = Random.Range(minAmount, maxAmount + 1);
            for (int i = 0; i < rand; i++)
            {
                var seeds = Instantiate(routineSeedTemplate);
                seeds.transform.position = originPosition + Vector3.up * .5f;
                var body = seeds.GetComponent<Rigidbody>();
                body.AddForce(Vector3.up + Random.insideUnitSphere * 1.5f, ForceMode.Impulse);

                yield return new WaitForSeconds(.9f);
            }
        }
    }
}