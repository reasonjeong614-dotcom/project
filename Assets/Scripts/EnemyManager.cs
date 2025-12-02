using UnityEngine;

/// <summary>
/// 1. 적 비행기들을 관리하는 매니저 클래스
/// - 적 비행기 스폰
/// </summary>
public class EnemyManager : MonoBehaviour
{
    public GameObject enemyFactory;     //적 비행기 공장 프리팹
    public GameObject[] spawnPoints;    //적 비행기 스폰 위치들
    float spawnTime = 1f;               //적 비행기 스폰 주기 (랜덤하게 변경)
    float timer = 0f;                   //스폰 타이머


    // Update is called once per frame
    void Update()
    {
        // 적 비행기 생성
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        timer += Time.deltaTime;
        if (timer >= spawnTime)
        {
            ////스폰 위치 랜덤 선택
            //int index = Random.Range(0, spawnPoints.Length);
            //Vector3 spawnPos = spawnPoints[index].transform.position;
            ////적 비행기 생성
            //Instantiate(enemyFactory, spawnPos, Quaternion.identity);
            ////타이머 초기화
            //timer = 0f;
            ////다음 스폰 주기 랜덤 설정 (0.5 ~ 2초)
            //spawnTime = Random.Range(0.5f, 2f);

            timer = 0f;
            // 다음 스폰 주기 랜덤 설정 (0.5 ~ 2초)
            spawnTime = Random.Range(0.2f, 0.5f);
            // 적 비행기 생성
            GameObject enemy = Instantiate(enemyFactory);
            // 스폰 위치 랜덤 선택
            int index = Random.Range(0, spawnPoints.Length);
            enemy.transform.position = spawnPoints[index].transform.position;
        }
    }
}
