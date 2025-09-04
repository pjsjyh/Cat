// CameraShake.cs - 카메라 흔들림
using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private Vector3 originalPosition;
    private Camera cam;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            cam = Camera.main;
            originalPosition = cam.transform.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cam.transform.position = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.transform.position = originalPosition;
    }
}