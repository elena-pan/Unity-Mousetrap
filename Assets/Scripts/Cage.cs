using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MouseTrap {
    public class Cage : MonoBehaviour
    {
        private Rigidbody rb;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            ResetStart();
        }

        public void ResetStart()
        {
            StartCoroutine(LetFall());
        }

        private IEnumerator LetFall()
        {
            rb.constraints = RigidbodyConstraints.None;
            yield return new WaitForSeconds(1);
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }

        void OnCollisionStay(Collision info) {
            if (GameManager.instance.isTurningCrank && info.collider.name == "Board") {
                GameManager.instance.cageFell = true;
            }
        }
            
    }
}
