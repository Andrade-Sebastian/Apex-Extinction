using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    public List<GameObject> terrainChunks;
    public GameObject player;
    public GameObject currentChunk;
    public float checkerRadius;
    Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    PlayerMovement pm;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    public GameObject latestChunk;
    public float maxOpDist;
    float opDist;
    float optimizerCooldown;
    public float optimizerCooldownDur;

    public Vector3 up;
    public Vector3 down;
    public Vector3 left;
    public Vector3 right;
    public Vector3 downleft;
    public Vector3 downright;
    public Vector3 upleft;
    public Vector3 upright;
    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    // void ChunkChecker()
    // {
    //     if(!currentChunk){
    //         return;
    //     }
    //     // right
    //     if(pm.moveDir.x > 0 && pm.moveDir.y == 0){
    //         if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask)){
    //             noTerrainPosition = currentChunk.transform.Find("Right").position;
    //             SpawnChunk();
    //         }
    //     }
    //     // left
    //     else if(pm.moveDir.x < 0 && pm.moveDir.y == 0){
    //         if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask)){
    //             noTerrainPosition = currentChunk.transform.Find("Left").position;
    //             SpawnChunk();
    //         }
    //     }
    //     // up
    //     else if(pm.moveDir.x == 0 && pm.moveDir.y > 0){
    //         if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask)){
    //             noTerrainPosition = currentChunk.transform.Find("Up").position;
    //             SpawnChunk();
    //         }
    //     }
    //     // down
    //     else if(pm.moveDir.x == 0 && pm.moveDir.y < 0){
    //         if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask)){
    //             noTerrainPosition = currentChunk.transform.Find("Down").position;
    //             SpawnChunk();
    //         }
    //     }
    //     // right up
    //     else if(pm.moveDir.x > 0 && pm.moveDir.y > 0){
    //         if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Up").position, checkerRadius, terrainMask)){
    //             noTerrainPosition = currentChunk.transform.Find("Right Up").position;
    //             SpawnChunk();
    //         }
    //     }
    //     // right down
    //     else if(pm.moveDir.x > 0 && pm.moveDir.y < 0){
    //         if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Down").position, checkerRadius, terrainMask)){
    //             noTerrainPosition = currentChunk.transform.Find("Right Down").position;
    //             SpawnChunk();
    //         }
    //     }
    //     // left up
    //     else if(pm.moveDir.x > 0 && pm.moveDir.y < 0){
    //         if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Up").position, checkerRadius, terrainMask)){
    //             noTerrainPosition = currentChunk.transform.Find("Left Up").position;
    //             SpawnChunk();
    //         }
    //     }
    //     // left down
    //     else if(pm.moveDir.x > 0 && pm.moveDir.y < 0){
    //         if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Down").position, checkerRadius, terrainMask)){
    //             noTerrainPosition = currentChunk.transform.Find("Left Down").position;
    //             SpawnChunk();
    //         }
    //     }
    // }

    // void SpawnChunk(){
    //     int rand = Random.Range(0, terrainChunks.Count);
    //     latestChunk = Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
    //     spawnedChunks.Add(latestChunk);
    // }

void ChunkChecker()
    {

        if (!currentChunk)
        {
            return;
        }

        if (pm.moveDir.y != 0 || pm.moveDir.x != 0)
        {
            int chunkSize = 32;

            up = currentChunk.transform.position + new Vector3(0, chunkSize, 0);
            down = currentChunk.transform.position + new Vector3(0, -chunkSize, 0);

            right = currentChunk.transform.position + new Vector3(chunkSize, 0, 0);
            left = currentChunk.transform.position + new Vector3(-chunkSize, 0, 0);

            upright = currentChunk.transform.position + new Vector3(chunkSize, chunkSize, 0);
            upleft = currentChunk.transform.position + new Vector3(-chunkSize, chunkSize, 0);

            downright = currentChunk.transform.position + new Vector3(chunkSize, -chunkSize, 0);
            downleft = currentChunk.transform.position + new Vector3(-chunkSize, -chunkSize, 0);

            if (!Physics2D.OverlapCircle(up, checkerRadius, terrainMask))
            {
                SpawnChunk(up);
            }
            if (!Physics2D.OverlapCircle(down, checkerRadius, terrainMask))
            {
                SpawnChunk(down);
            }
            if (!Physics2D.OverlapCircle(right, checkerRadius, terrainMask))
            {
                SpawnChunk(right);
            }
            if (!Physics2D.OverlapCircle(left, checkerRadius, terrainMask))
            {
                SpawnChunk(left);
            }
            if (!Physics2D.OverlapCircle(upright, checkerRadius, terrainMask))
            {
                SpawnChunk(upright);
            }
            if (!Physics2D.OverlapCircle(upleft, checkerRadius, terrainMask))
            {
                SpawnChunk(upleft);
            }
            if (!Physics2D.OverlapCircle(downright, checkerRadius, terrainMask))
            {
                SpawnChunk(downright);
            }
            if (!Physics2D.OverlapCircle(downleft, checkerRadius, terrainMask))
            {
                SpawnChunk(downleft);
            }

        }
    }

    void SpawnChunk(Vector3 positionToSpawn)
    {

        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], positionToSpawn, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }
    void ChunkOptimizer(){
        optimizerCooldown -= Time.deltaTime;

        if(optimizerCooldown <= 0f){
            optimizerCooldown = optimizerCooldownDur;
        }else{
            return;
        }

        foreach(GameObject chunk in spawnedChunks){
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if(opDist > maxOpDist){
                chunk.SetActive(false);
            }else{

                chunk.SetActive(true);
            }
        }
    }
}
