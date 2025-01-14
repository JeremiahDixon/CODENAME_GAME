using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class TheCamera : MonoBehaviour
{
    public static TheCamera Instance;

    private void Awake()
    {

    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

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
