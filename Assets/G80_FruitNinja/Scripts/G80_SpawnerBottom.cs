using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;


[Serializable]
public class G80_SpawnerBottom : G80_Spawner
{
    public int numberOfBombs = 0;

    public void Update()
    {
        
    }

    public override void Start()
    {
        base.Start();
        StartCoroutine(UpdateBombTime());
    }

    public override void OnModeChange(SpawnMode mode)
    {
        base.OnModeChange(mode);
        StartCoroutine(UpdateBombTime());
    }

    public override IEnumerator Spawn()
    {
        yield return new WaitForSeconds(startDelay);
        while (enabled)
        {
            GameObject fruitPrefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
            var spawnPosX = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            var spawnPosY = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            var spawnPosZ = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);
            var intervalTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            var shootForce = Random.Range(minForce, maxForce);
            var randomAngleOffset = Random.Range(minInitAngle, maxInitAngle);
            var initAngle = Quaternion.Euler(fruitPrefab.transform.rotation.eulerAngles.x,
                fruitPrefab.transform.rotation.eulerAngles.y, CalculateSpawnAngles(spawnPosX, randomAngleOffset));
            // var initAngle = CalculateSpawnAngles(spawnPosX);
            Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, spawnPosZ);
            
            //burstMode
            if (maxBurstFruits > 0)
            {
                if (currentBusrtFruits < maxBurstFruits)
                {
                    GameObject newFruit = Instantiate(fruitPrefab, spawnPos, initAngle);
                    G80_SoundManager.Instance.PlaySoundOnce(G80_SoundManager.Instance.sfx.launchClip);
                    G80_GameManager.Instance.AddObjectToActiveList(newFruit);
                    newFruit.GetComponent<Rigidbody>().AddForce(newFruit.transform.up * shootForce, ForceMode.VelocityChange);
                    newFruit.GetComponent<Rigidbody>().AddTorque(newFruit.transform.forward * initAngle.z*  shootForce * 1, ForceMode.VelocityChange);
                    Destroy(newFruit, fruitLifetime);
                    if (bombAvailable)
                    {
                        StartCoroutine(UpdateBombTime());
                        int numberOfBombs = Random.Range(1, 3);
                        while (numberOfBombs-- > 0)
                        {
                            Instantiate(bombPrefabs, spawnPos, initAngle);
                            GameObject newBomb = Instantiate(bombPrefabs, spawnPos, initAngle);
                            G80_GameManager.Instance.AddObjectToActiveList(newBomb);
                            newBomb.GetComponent<Rigidbody>()
                                .AddForce(newBomb.transform.up * shootForce, ForceMode.VelocityChange);
                            newBomb.GetComponent<Rigidbody>()
                                .AddTorque(newBomb.transform.forward * initAngle.z * shootForce,
                                    ForceMode.VelocityChange);
                            Destroy(newBomb, 4.0f);
                            
                        }
                        
                      
                    }
                    yield return new WaitForSeconds(burstInterval);
                    currentBusrtFruits++;
                }
                else
                {
                    yield return new WaitForSeconds(intervalTime);
                    currentBusrtFruits = 0;
                }
                
                
            }
            //normal mode
            else
            {
                GameObject newFruit = Instantiate(fruitPrefab, spawnPos, initAngle);
                G80_GameManager.Instance.AddObjectToActiveList(newFruit);
                G80_SoundManager.Instance.PlaySoundOnce(G80_SoundManager.Instance.sfx.launchClip);
                newFruit.GetComponent<Rigidbody>().AddForce(newFruit.transform.up * shootForce, ForceMode.VelocityChange);
                // newFruit.GetComponent<Rigidbody>()
                //     .AddTorque(newFruit.transform.forward * initAngle.z * shootForce * 2, ForceMode.Impulse);
                Destroy(newFruit, fruitLifetime);
                if (bombAvailable)
                {
                 
                    StartCoroutine(UpdateBombTime());
                    {
                        StartCoroutine(SpawnBomb());
                    }

                }
                yield return new WaitForSeconds(intervalTime);
            }
         
          
           
        }
        yield  break;
    }
    
    

    public override IEnumerator SpawnBomb()
    {
        numberOfBombs = Random.Range(1, 2);
        while (numberOfBombs-- > 0)
        {
            var spawnPosX = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            var spawnPosY = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            var spawnPosZ = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);
            var intervalTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            var shootForce = Random.Range(minForce, maxForce);
            var randomAngleOffset = Random.Range(minInitAngle, maxInitAngle);
            var initAngle = Quaternion.Euler(bombPrefabs.transform.rotation.eulerAngles.x,
                bombPrefabs.transform.rotation.eulerAngles.y, CalculateSpawnAngles(spawnPosX, randomAngleOffset));
            // var initAngle = CalculateSpawnAngles(spawnPosX);
            Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, spawnPosZ);
            Instantiate(bombPrefabs, spawnPos, Quaternion.identity);
            
            GameObject newBomb = Instantiate(bombPrefabs, spawnPos, initAngle);
            G80_GameManager.Instance.AddObjectToActiveList(newBomb);
            newBomb.GetComponent<Rigidbody>()
                .AddForce(newBomb.transform.up * shootForce * 0, ForceMode.Impulse);
            // newBomb.GetComponent<Rigidbody>()
            //     .AddTorque(newBomb.transform.forward * initAngle.z * shootForce ,
            //         ForceMode.VelocityChange);
            Destroy(newBomb, 4.0f);
            yield return null;
        }
        
    }
    
     public virtual IEnumerator UpdateBombTime()
        {
           
            {
                bombAvailable = false;
                float timeToNextBomb = Random.Range(randomTimeToNextBomb - 3.0f, randomTimeToNextBomb + 3.0f);
                while (timeToNextBomb > 0.0f)
                {
                    timeToNextBomb -= Time.unscaledDeltaTime;
                    yield return null;
                }
    
                bombAvailable = true;
            }
          
        }
   
    
}


