using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFlickeringLight : MonoBehaviour
{
public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 1f;

    private UnityEngine.Rendering.Universal.Light2D light2D;
    private float time;

    void Start()
    {
        light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    void Update()
    {
        // Use a sine wave to smoothly transition between min and max intensity
        time += Time.deltaTime * flickerSpeed;
        float flickerValue = Mathf.Sin(time);

        // Map the sine wave to the intensity range
        float randomIntensity = Mathf.Lerp(minIntensity, maxIntensity, (flickerValue + 1f) / 2f);
        light2D.intensity = randomIntensity;
    }
}
