using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    public List<GameObject> terrainChunks;
    public GameObject player;
    public GameObject currentChunk;
    public float checkerRadius;
    // Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    Vector3 playerLastPosition;
    // PlayerMovement pm;

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
        // pm = FindObjectOfType<PlayerMovement>();
        playerLastPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }


    void ChunkChecker()
    {

        if (!currentChunk)
        {
            return;
        }

        // if (pm.moveDir.y != 0 || pm.moveDir.x != 0)
        // {
        //     int chunkSize = 32;

        //     up = currentChunk.transform.position + new Vector3(0, chunkSize, 0);
        //     down = currentChunk.transform.position + new Vector3(0, -chunkSize, 0);

        //     right = currentChunk.transform.position + new Vector3(chunkSize, 0, 0);
        //     left = currentChunk.transform.position + new Vector3(-chunkSize, 0, 0);

        //     upright = currentChunk.transform.position + new Vector3(chunkSize, chunkSize, 0);
        //     upleft = currentChunk.transform.position + new Vector3(-chunkSize, chunkSize, 0);

        //     downright = currentChunk.transform.position + new Vector3(chunkSize, -chunkSize, 0);
        //     downleft = currentChunk.transform.position + new Vector3(-chunkSize, -chunkSize, 0);

        //     if (!Physics2D.OverlapCircle(up, checkerRadius, terrainMask))
        //     {
        //         SpawnChunk(up);
        //     }
        //     if (!Physics2D.OverlapCircle(down, checkerRadius, terrainMask))
        //     {
        //         SpawnChunk(down);
        //     }
        //     if (!Physics2D.OverlapCircle(right, checkerRadius, terrainMask))
        //     {
        //         SpawnChunk(right);
        //     }
        //     if (!Physics2D.OverlapCircle(left, checkerRadius, terrainMask))
        //     {
        //         SpawnChunk(left);
        //     }
        //     if (!Physics2D.OverlapCircle(upright, checkerRadius, terrainMask))
        //     {
        //         SpawnChunk(upright);
        //     }
        //     if (!Physics2D.OverlapCircle(upleft, checkerRadius, terrainMask))
        //     {
        //         SpawnChunk(upleft);
        //     }
        //     if (!Physics2D.OverlapCircle(downright, checkerRadius, terrainMask))
        //     {
        //         SpawnChunk(downright);
        //     }
        //     if (!Physics2D.OverlapCircle(downleft, checkerRadius, terrainMask))
        //     {
        //         SpawnChunk(downleft);
        //     }

        // }

        Vector3 moveDir = player.transform.position - playerLastPosition;
        playerLastPosition = player.transform.position;

        string directionName = GetDirectionName(moveDir);

        // if(!Physics2D.OverlapCircle(currentChunk.transform.Find(directionName).position, checkerRadius, terrainMask))
        // {
        //     SpawnChunk(currentChunk.transform.Find(directionName).position);

        //     if(directionName.Contains("Up") && directionName.Contains("Right"))
        //     {
        //         if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
        //         {
        //             SpawnChunk(currentChunk.transform.Find("Up").position);
        //         }
        //         if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
        //         {
        //             SpawnChunk(currentChunk.transform.Find("Right").position);
        //         }
        //     }
        //     else if(directionName.Contains("Down") && directionName.Contains("Right"))
        //     {
        //         if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
        //         {
        //             SpawnChunk(currentChunk.transform.Find("Down").position);
        //         }
        //         if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
        //         {
        //             SpawnChunk(currentChunk.transform.Find("Right").position);
        //         }
        //     }
        //     else if(directionName.Contains("Down") && directionName.Contains("Left"))
        //     {
        //         if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
        //         {
        //             SpawnChunk(currentChunk.transform.Find("Down").position);
        //         }
        //         if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
        //         {
        //             SpawnChunk(currentChunk.transform.Find("Left").position);
        //         }
        //     }
        // }
        CheckAndSpawnChunk(directionName);
        if(directionName.Contains("Up"))
        {
            CheckAndSpawnChunk("Up");
        }
        else if(directionName.Contains("Down"))
        {
            CheckAndSpawnChunk("Down");
        }
        else if(directionName.Contains("Right"))
        {
            CheckAndSpawnChunk("Right");
        }
        else if(directionName.Contains("Left"))
        {
            CheckAndSpawnChunk("Left");
        }
    }

    void CheckAndSpawnChunk(string direction)
    {
        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(direction).position, checkerRadius, terrainMask))
        {
            SpawnChunk(currentChunk.transform.Find(direction).position);
        }
    }

    string GetDirectionName(Vector3 direction)
    {
        direction = direction.normalized;

        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // moving more horizontally than vertically
            if( direction.y > 0.5f)
            {
                return direction.x > 0 ? "Right Up" : "Left Up";
            }
            else if (direction.y < -0.5f) 
            {
                return direction.x > 0 ? "Right Down" : "Left Down";

            }
            else
            {
                return direction.x > 0 ? "Right" : "Left";

            }
        }
        else
        {
            // moving more vertically than horizontally
            if( direction.x > 0.5f)
            {
                return direction.y > 0 ? "Right Up" : "Right Down";

            }
            else if (direction.x < -0.5f) 
            {
                return direction.y > 0 ? "Left Up" : "Left Down";

            }
            else
            {
                return direction.y > 0 ? "Up" : "Down";

            }

        }
    }

    void SpawnChunk(Vector3 spawnPosition)
    {

        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], spawnPosition, Quaternion.identity);
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
