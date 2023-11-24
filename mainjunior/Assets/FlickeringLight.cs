using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;

    private UnityEngine.Rendering.Universal.Light2D light2D;

    void Start()
    {
        light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0, flickerSpeed));

            float randomIntensity = Random.Range(minIntensity, maxIntensity);
            light2D.intensity = randomIntensity;
        }
    }
}
