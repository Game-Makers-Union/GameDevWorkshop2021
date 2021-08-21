using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private const float DefaultIntensity = 0.1f;
    private const float DefaultDuration = 0.1f;
    private float shakeIntensity;
    private float shakeDuration;

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f) return;
        
        if (shakeDuration > 0f)
        {
            transform.position = new Vector3(Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity), -10f);
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            transform.position = new Vector3(0f, 0f, -10f);
        }
    }

    public void Shake(float intensity = DefaultIntensity, float duration = DefaultDuration)
    {
        shakeIntensity = intensity;
        shakeDuration = duration;
    }
}
