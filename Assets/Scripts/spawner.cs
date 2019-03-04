using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    private int wave = 0;
    public int numOfTotalWaves = 10;
    public GameObject endGoal;
    public TilemapNavigation AStar;
    float halfWidth;
    float halfHeight;

    [System.Serializable]
    public class WaveDescription
    {
        public float delay;
        public EnemyDescription[] enemyDescriptions;
    }

    [System.Serializable]
    public class EnemyDescription
    {
        public GameObject enemyType;
        public float unitDelay;
        public float waveDelay;
        public int waveSpawnCount;
    }

    public WaveDescription[] waveDescriptions;
    private int aliveEnemies = 0; // Will count how many enemies are currently alive, MAKE SURE TO HAVE THIS DECREASE WHEN ONE DIES

    void Start()
    {
        StartCoroutine("SpawnWave");
        halfWidth = GetComponent<BoxCollider2D>().bounds.extents.x;
        halfHeight = GetComponent<BoxCollider2D>().bounds.extents.y;
    }

    void Update()
    {

    }

    IEnumerator SpawnWave()
    {
        while (wave < waveDescriptions.Length)
        {
            WaveDescription currentWave = waveDescriptions[wave];
            for (int i = 0; i < currentWave.enemyDescriptions.Length; i++) // For every enemy Type
            {
                EnemyDescription currentEnemy = currentWave.enemyDescriptions[i];
                StartCoroutine(SpawnEnemy(currentEnemy)); // spawn that enemy type
                yield return new WaitForSeconds(currentEnemy.waveDelay);
            }
            ++wave;
            yield return new WaitForSeconds(currentWave.delay);
        }
    }

    IEnumerator SpawnEnemy(EnemyDescription description)
    {
        GameObject enemyType = description.enemyType;
        float delay = description.unitDelay;
        int enemiesToSpawn = description.waveSpawnCount;
        for(int i = 0;i<enemiesToSpawn;++i)
        {
            GameObject newEnemy = Instantiate(enemyType,(Vector2)transform.position + new Vector2(Random.Range(-halfWidth,halfWidth),Random.Range(-halfHeight,halfHeight)),Quaternion.identity);
            newEnemy.GetComponent<AgentScript>().goal = endGoal;
            newEnemy.GetComponent<AgentScript>().aStar = AStar;
            aliveEnemies++; // Add one to our alive enemies counter
            yield return new WaitForSeconds(delay);
        }
    }
}
