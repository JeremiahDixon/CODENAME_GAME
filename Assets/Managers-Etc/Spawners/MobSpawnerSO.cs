using UnityEngine;

[CreateAssetMenu(fileName = "MobSpawnerSO", menuName = "Scriptable Objects/MobSpawnerSO")]
public class MobSpawnerSO : ScriptableObject
{
    public string mob1;
    public string mob2;
    public string mob3;
    public string mob4;
    public int mob1Int = 0;
    public int mob2Int = 0;
    public int mob3Int = 0;
    public int mob4Int = 0;
    public GameObject[] basicMobs;
    public GameObject[] intermediateMobs;
    public GameObject[] advancedMobs;

    public void CreateInfo(GameObject gameObject){
        MobSpawner spawner = gameObject.GetComponent<MobSpawner>();
        //spawner
    }
}
