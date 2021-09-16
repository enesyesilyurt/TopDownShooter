﻿using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Spawner : MonoBehaviour
    {
        public Wave[] waves;
        public Enemy enemy;

        LivingEntity PlayerEntity;
        Transform playerTransform;

        Wave currentWave;
        int currentWaveNumber;

        int enemiesRemainingAlive;
        int enemiesRemainingToSpawn;
        float nextSpawnTime;

        MapGenerator map;

        float timeBetweenCampingChecks = 2;
        float campThresholdDistance = 1.5f;
        float nextCampCheckTime;
        Vector3 campPositionOld;
        bool isCamping;

        bool isDisabled;

        public event Action<int> OnNewWave;

        private void Start()
        {
            PlayerEntity = FindObjectOfType<Player>();
            playerTransform = PlayerEntity.transform;

            nextCampCheckTime = timeBetweenCampingChecks + Time.time;
            campPositionOld = playerTransform.position;
            PlayerEntity.OnDeath += OnPlayerDeath;

            map = FindObjectOfType<MapGenerator>();
            NextWave();
        }
        private void Update()
        {
            if (!isDisabled)
            {
                if (Time.time > nextCampCheckTime)
                {
                    nextCampCheckTime = Time.time + timeBetweenCampingChecks;
                    isCamping = (Vector3.Distance(playerTransform.position, campPositionOld) < campThresholdDistance);
                    campPositionOld = playerTransform.position;
                }
                if ((enemiesRemainingToSpawn > 0 || currentWave.infinite)  && Time.time > nextSpawnTime)
                {
                    enemiesRemainingToSpawn--;
                    nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                    StartCoroutine(SpawnEnemy());
                }
            }
        }
        IEnumerator SpawnEnemy()
        {
            float spawnDelay = 1;
            float tileFlashSpeed = 4;

            Transform spawnTile = map.GetRandomOpenTile();
            if (isCamping)
            {
                spawnTile = map.GetTileFromPosition(playerTransform.position);
            }
            Material tileMat = spawnTile.GetComponent<Renderer>().material;
            Color initialColour = tileMat.color;
            Color flashColour = Color.red;
            float spawnTimer = 0;
            while (spawnTimer < spawnDelay)
            {
                tileMat.color = Color.Lerp(initialColour, flashColour, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));
                spawnTimer += Time.deltaTime;
                yield return null;
            }

            Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += OnEnemyDeath;
            spawnedEnemy.SetCharacteristics(currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth, currentWave.skinColour);
        }
        void OnPlayerDeath()
        {
            isDisabled = true;
        }
        void OnEnemyDeath()
        {
            enemiesRemainingAlive--;
            if (enemiesRemainingAlive == 0)
            {
                NextWave();
            }
        }

        void ResetPlayerPosition()
        {
            playerTransform.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up*3;
        }
        void NextWave()
        {
            currentWaveNumber++;
            if (currentWaveNumber - 1 < waves.Length)
            {
                currentWave = waves[currentWaveNumber - 1];
                enemiesRemainingToSpawn = currentWave.enemyCount;
                enemiesRemainingAlive = enemiesRemainingToSpawn;

                if (OnNewWave != null)
                {
                    OnNewWave(currentWaveNumber);
                }
                ResetPlayerPosition();
            }
        }
        [Serializable]
        public class Wave
        {
            public bool infinite;
            public int enemyCount;
            public float timeBetweenSpawns;

            public float moveSpeed;
            public int hitsToKillPlayer;
            public float enemyHealth;
            public Color skinColour;
        }
    }
}