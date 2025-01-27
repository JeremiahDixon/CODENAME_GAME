using UnityEngine;

[CreateAssetMenu(fileName = "MobSpawnerSO", menuName = "Scriptable Objects/MobSpawnerSO")]
public class MobSpawnerSO : ScriptableObject
{
    [SerializeField] GameObject[] basicMobs;
    [SerializeField] GameObject[] intermediateMobs;
    [SerializeField] GameObject[] advancedMobs;
    [SerializeField] GameObject[] legendaryMobs;
    [SerializeField] GameObject[] shooterMobs;
    [SerializeField] float basicTimeBtwSpawn;
    [SerializeField] float startBasicTimeBtwSpawn;
    [SerializeField] float intermediateTimeBtwSpawn;
    [SerializeField] float startIntermediateTimeBtwSpawn;
    [SerializeField] float advancedTimeBtwSpawn;
    [SerializeField] float startAdvancedTimeBtwSpawn;
    [SerializeField] float legendaryTimeBtwSpawn;
    [SerializeField] float startLegendaryTimeBtwSpawn;
    [SerializeField] float shooterTimeBtwSpawn;
    [SerializeField] float startShooterTimeBtwSpawn;

    public void CreateInfo(GameObject gameObject){
        MobSpawner spawner = gameObject.GetComponent<MobSpawner>();
        spawner.BasicMobs = basicMobs;
        spawner.IntermediateMobs = intermediateMobs;
        spawner.AdvancedMobs = advancedMobs;
        spawner.LegendaryMobs = legendaryMobs;
        spawner.ShooterMobs = shooterMobs;

        spawner.BasicTimeBtwSpawn = basicTimeBtwSpawn;
        spawner.StartBasicTimeBtwSpawn = startBasicTimeBtwSpawn;
        spawner.IntermediateTimeBtwSpawn = intermediateTimeBtwSpawn;
        spawner.StartIntermediateTimeBtwSpawn = startIntermediateTimeBtwSpawn;
        spawner.AdvancedTimeBtwSpawn = advancedTimeBtwSpawn;
        spawner.StartAdvancedTimeBtwSpawn = startAdvancedTimeBtwSpawn;
        spawner.LegendaryTimeBtwSpawn = legendaryTimeBtwSpawn;
        spawner.StartLegendaryTimeBtwSpawn = startLegendaryTimeBtwSpawn;
        spawner.ShooterTimeBtwSpawn = shooterTimeBtwSpawn;
        spawner.StartShooterTimeBtwSpawn = startShooterTimeBtwSpawn;
    }
}
