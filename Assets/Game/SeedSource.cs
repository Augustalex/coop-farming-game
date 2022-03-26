using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SeedSource : MonoBehaviour
    {
        public GameObject seedTemplate;
        private bool _used;

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
            StartCoroutine(SpawnSeeds());
        }

        private IEnumerator SpawnSeeds()
        {
            var rand = Random.Range(2, 3);
            for (int i = 0; i < rand; i++)
            {
                var seeds = Instantiate(seedTemplate);
                seeds.transform.position = transform.position + Vector3.up * .5f;
                var body = seeds.GetComponent<Rigidbody>();
                body.AddForce(Vector3.up * 5f + Random.insideUnitSphere * .5f, ForceMode.Impulse);
            
                yield return new WaitForSeconds(.5f);
            }
            
            
            Destroy(gameObject, .1f);
        }
    }
}