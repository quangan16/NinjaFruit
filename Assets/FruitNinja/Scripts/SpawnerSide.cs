using System;
using System.Collections;
using System.Collections.Generic;
using FruitNinja.Scripts;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;


[Serializable]
public class SpawnerSide : Spawner
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
                fruitPrefab.transform.rotation.eulerAngles.y, CalculateAngles(spawnPosX, randomAngleOffset));
            // var initAngle = CalculateAngles(spawnPosX);
            Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, spawnPosZ);
            if(shootForce > 0.0f)
            {
                if (GameManager.Instance.CurrentMode > SpawnMode.FAST && shootForce > 0.0f)
                {
                    if (currentBusrtFruits < maxBurstFruits)
                    {
                        GameObject newFruit = Instantiate(fruitPrefab, spawnPos, initAngle);
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
                else
                {
                    GameObject newFruit = Instantiate(fruitPrefab, spawnPos, initAngle);
                    newFruit.GetComponent<Rigidbody>().AddForce(transform.up * shootForce, ForceMode.VelocityChange);
                    newFruit.GetComponent<Rigidbody>()
                        .AddTorque(newFruit.transform.forward * initAngle.z * shootForce * 3, ForceMode.Impulse);
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
        sideMinForce = GameManager.Instance.GetCurrentModeData().minSideForce;
        sideMaxForce = GameManager.Instance.GetCurrentModeData().maxSideForce;
    }

    public override void SpawnBomb()
    {

    }
    
}


