using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base script of all projectile behavior

public class ProjectileWeaponBehavior : MonoBehaviour
{
    protected Vector3 direction;
    public float destroyAfterSeconds; //destroy the projectile after a certain amnt of seconds


    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;
        float dirx = direction.x;
        float diry = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if (dirx < 0 && diry == 0 )// if we are facing left 
        {
            //reverse the sprite to face the left
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if (dirx == 0 && diry < 0) //down
        {
            scale.y = scale.y * -1;
            rotation.z = -90f;
        }
        else if (dirx == 0 && diry > 0) //up
        {
            scale.x = scale.x * -1;
            rotation.z = -90f;
        }
        else if (dirx > 0 && diry > 0) //right up
        {
            rotation.z = 45f;
        }
        else if (dirx > 0 && diry < 0) //right down
        {
            rotation.z = -45f;
        }
        else if (dirx < 0 && diry > 0) //left up
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = -45f;
        }
        else if (dirx < 0 && diry < 0) //left down
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = 45f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation); //converting

    }
}
