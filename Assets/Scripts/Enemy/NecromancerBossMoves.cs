using System.Collections;
using UnityEngine;

public class NecromancerBossMoves : MonoBehaviour
{
    public EnemyStats enemyData;
    public GameObject prefabToInstantiate;
    public float dashCooldown = 5.0f;
    public float dashDuration = 0.01f;
    private bool isDashing = false;
    private bool isSpawning = false;
    private float normalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        enemyData = GetComponent<EnemyStats>();
        normalSpeed = enemyData.currentMoveSpeed;
        StartCoroutine(DashMove());
    }

    void Update()
    {
        if (!isDashing)
        {
            StartCoroutine(DashMove());
        }
        if (!isSpawning)
        {
            StartCoroutine(SpawnZombie());
        }
    }

    IEnumerator DashMove()
    {
        isDashing = true;

        enemyData.currentMoveSpeed *= 5.0f;

        yield return new WaitForSeconds(dashDuration);

        enemyData.currentMoveSpeed = normalSpeed;

        yield return new WaitForSeconds(dashCooldown);

        isDashing = false;
    }

    IEnumerator SpawnZombie()
    {
        isSpawning = true;
        yield return new WaitForSeconds(2f);

        Instantiate(prefabToInstantiate, transform.position, transform.rotation);

        yield return new WaitForSeconds(2f);

        isSpawning = false;
    }

}
