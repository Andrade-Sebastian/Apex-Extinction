using System.Collections;
using UnityEngine;

public class CommanderBossMoves : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;


    private float timer;
    private float bulletTimer;
    private GameObject player;
    private Vector3 playerCurPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }
    void Update()
    {
        timer += Time.deltaTime;
        bulletTimer += Time.deltaTime;
        playerCurPos = player.transform.position;
        TeleportMove();
        Shoot();
    }

    void TeleportMove()
    {
        int randomNumber = Random.Range(0, 2);
        if(timer > 15)
        {
            if(randomNumber == 0){
                transform.position = new Vector3(playerCurPos.x + 5f, playerCurPos.y, playerCurPos.z);
            }else{
                transform.position = new Vector3(playerCurPos.x - 5f, playerCurPos.y, playerCurPos.z);
            }

            timer = 0;
        }
    }
      
    void Shoot(){
        if(bulletTimer > 5)
            {
                bulletTimer = 0;
                Vector3 top = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                Vector3 bot = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
                Instantiate(bullet, top, Quaternion.identity);
                Instantiate(bullet, transform.position, Quaternion.identity);
                Instantiate(bullet, bot, Quaternion.identity);

            }
    }
}
    