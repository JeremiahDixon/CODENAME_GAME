using System.Collections;
using UnityEngine;

public class SoldierClass : MonoBehaviour
{
    public GameObject iceBow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // StartCoroutine(SpawnIceBow());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private IEnumerator SpawnIceBow()
    // {
    //     yield return new WaitForSeconds(5);
    //     Instantiate(iceBow, transform.position, Quaternion.identity);
    // }
}
