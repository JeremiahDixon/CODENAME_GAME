using UnityEngine;

[CreateAssetMenu(fileName = "MobSpawnerSO", menuName = "Scriptable Objects/MobSpawnerSO")]
public class MobSpawnerSO : ScriptableObject
{
    public string mob1;
    public string mob2;
    public string mob3;
    public string mob4;
    public GameObject[] basicMobs;
    public GameObject[] intermediateMobs;
    public GameObject[] advancedMobs;

    public void CreateInfo(GameObject gameObject){
        MobSpawner spawner = gameObject.GetComponent<MobSpawner>();
        //spawner
    }
}
