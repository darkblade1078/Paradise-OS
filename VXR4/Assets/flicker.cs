using System.Collections;
using UnityEngine;

    public class flicker : MonoBehaviour
    {
        public Light targetLight; // Assign your Point Light here in the Inspector
        public float minIntensity = 0.5f;
        public float maxIntensity = 1.5f;
        public float flickerSpeed = 0.1f; // How often the intensity changes

        private void Start()
        {
            if (targetLight == null)
            {
                targetLight = GetComponent<Light>();
                if (targetLight == null)
                {
                    Debug.LogError("FlickeringLight script requires a Light component or a target Light assigned.");
                    enabled = false; // Disable script if no light found
                    return;
                }
            }
            StartCoroutine(FlickerRoutine());
        }

        IEnumerator FlickerRoutine()
        {
            while (true)
            {
                // Randomly set intensity within the defined range
                targetLight.intensity = Random.Range(minIntensity, maxIntensity);
                yield return new WaitForSeconds(Random.Range(flickerSpeed * 0.5f, flickerSpeed * 1.5f));
            }
        }
    }