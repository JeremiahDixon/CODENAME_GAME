using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class TheCamera : MonoBehaviour
{
    public static TheCamera Instance;
    public float xLowEndRange = -1f;
    public float xHighEndRange = 1f;
    public float yLowEndRange = -1f;
    public float yHighEndRange = 1f;

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(xLowEndRange, xHighEndRange) * magnitude;
            float y = Random.Range(yLowEndRange, yHighEndRange) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    public void LockCamera()
    {
        GetComponent<CinemachineCamera>().Follow = null;
        GetComponent<CinemachineCamera>().LookAt = null;
    }
}
