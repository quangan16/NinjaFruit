using System;
using System.Collections;
using System.Collections.Generic;
using FruitNinja.Scripts;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;


[Serializable]
public class G80_SpawnerSide : G80_Spawner
{
    public float sideMinForce;
    public float sideMaxForce;
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
            var shootForce = Random.Range(sideMinForce, sideMaxForce);
            var randomAngleOffset = Random.Range(minInitAngle, maxInitAngle);
            var initAngle = Quaternion.Euler(fruitPrefab.transform.rotation.eulerAngles.x,
                fruitPrefab.transform.rotation.eulerAngles.y, CalculateSpawnAngles(spawnPosX, randomAngleOffset));
            // var initAngle = CalculateSpawnAngles(spawnPosX);
            Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, spawnPosZ);
            if(shootForce > 0.0f)
            {
                if (G80_GameManager.Instance.CurrentMode == SpawnMode.BOSS && gameObject.CompareTag("SpawnerRight"))
                {
                    SpawnBossFruit(spawnPos, shootForce, initAngle);
                  
                    yield break;
                    
                }
                //Burst Mode
                else if (maxBurstFruits > 0)
                {
                    if (currentBusrtFruits < maxBurstFruits)
                    {
                        GameObject newFruit = Instantiate(fruitPrefab, spawnPos, initAngle);
                        G80_SoundManager.Instance.PlaySoundOnce(G80_SoundManager.Instance.sfx.launchClip);
                        newFruit.GetComponent<Rigidbody>()
                            .AddForce(transform.up * shootForce, ForceMode.VelocityChange);
                        newFruit.GetComponent<Rigidbody>()
                            .AddTorque(newFruit.transform.forward * initAngle.z * shootForce * 3,
                                ForceMode.VelocityChange);
                        Destroy(newFruit, fruitLifetime);
                        yield return new WaitForSeconds(burstInterval);
                        currentBusrtFruits++;
                    }
                    else
                    {
                        yield return new WaitForSeconds(intervalTime);
                        currentBusrtFruits = 0;
                    }

                }
                //Normal mode
                else
                {
                    GameObject newFruit = Instantiate(fruitPrefab, spawnPos, initAngle);
                    G80_SoundManager.Instance.PlaySoundOnce(G80_SoundManager.Instance.sfx.launchClip);
                    newFruit.GetComponent<Rigidbody>().AddForce(transform.up * shootForce, ForceMode.VelocityChange);
                    newFruit.GetComponent<Rigidbody>()
                        .AddTorque(newFruit.transform.forward * initAngle.z * shootForce * 3, ForceMode.VelocityChange);
                    Destroy(newFruit, fruitLifetime);
                    yield return new WaitForSeconds(intervalTime);
                }
            }
            else
            {
                yield break;
            }
           
         
          
           
        }
        yield  break;
    }
    
    public override void UpdateData()
    {
        base.UpdateData();
        sideMinForce = G80_GameManager.Instance.GetCurrentModeData().minSideForce;
        sideMaxForce = G80_GameManager.Instance.GetCurrentModeData().maxSideForce;
    }

    public override IEnumerator SpawnBomb()
    {
        yield break;
    }

    public void SpawnBossFruit(Vector3 spawnPos, float shootForce, Quaternion initAngle)
    {


        GameObject superFruit = Instantiate(fruitPrefabs[0], spawnPos, initAngle);
        superFruit.GetComponent<Rigidbody>().AddForce(transform.up * shootForce, ForceMode.VelocityChange);
        superFruit.GetComponent<Rigidbody>()
            .AddTorque(superFruit.transform.forward * initAngle.z * shootForce * 3,
                ForceMode.VelocityChange);
        Destroy(superFruit, fruitLifetime);
  
        
     
        
    }
    
}


