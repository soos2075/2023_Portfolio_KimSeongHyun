using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{

    GameObject spawnArea;

    [SerializeField]
    Vector3 spawnPosition;
    [SerializeField]
    float sizeX = 25f;
    [SerializeField]
    float sizeZ = 20f;


    public int CurrentConut { get; set; }
    public int maxCount;

    void Start()
    {
        spawnArea = GameObject.FindGameObjectWithTag("SlimeField");
        spawnPosition = spawnArea.transform.position;
    }

    void Update()
    {
        if (CurrentConut < maxCount)
        {
            StartCoroutine(RespawnSlime());
            CurrentConut++;
        }
    }


    IEnumerator RespawnSlime()
    {
        yield return new WaitForSeconds(Random.Range(2, 6));
        Vector3 pos = new Vector3(Random.Range(-sizeX, sizeX) + spawnPosition.x, 0, Random.Range(-sizeZ, sizeZ) + spawnPosition.z);
        GameObject slime = Managers.Resource.Instantiate("Character/Slime", pos);

    }
}
