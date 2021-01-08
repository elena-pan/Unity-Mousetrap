using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MouseTrap {
    public class StopSign : MonoBehaviour
    {
        private Rigidbody rb;
        private float increaseFactor = -200f;
        private bool triggerActive = true;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = Vector3.zero;
            StartCoroutine(WaitForKeyPress(KeyCode.Space, () => {
                StartContraption();
            }));
        }

        public void StartContraption()
        {
            StartCoroutine(Wait(2, () => {
                rb.AddRelativeTorque(transform.up * increaseFactor, ForceMode.Impulse);
            }));
        }

        private IEnumerator Wait(int num, System.Action callback)
        {
            yield return new WaitForSeconds(num);
            callback();
        }

        private IEnumerator WaitForKeyPress(KeyCode key, System.Action callback)
        {
            bool done = false;
            while(!done) // essentially a "while true", but with a bool to break out naturally
            {
                if(Input.GetKeyDown(key))
                {
                    done = true; // breaks the loop
                }
                yield return null; // wait until next frame, then continue execution from here (loop continues)
            }
        
            // now this function returns
            callback();
        }
    }
}