using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class PlayerMovemechanim : NetworkBehaviour
{
    Animator anim;
    public GameObject bulletPrefab;
    //[SyncVar]
    public Vector2 fireOffSet;
    //[SyncVar]
    public Vector3 fireDirection;
    [SyncVar]
    public Vector3 shootDir;

    public override void OnStartLocalPlayer()
    {
        anim = GetComponent<Animator>();
    }

    [Command]
    void CmdWalk(bool isWalking, float x, float y)
    {
        //Debug.Log("CmdWalk");
        RpcWalk(isWalking, x, y);
    }

    [ClientRpc]
    void RpcWalk(bool isWalking, float x, float y)
    {
        //Debug.Log("RpcWalk");
        //anim = GetComponent<Animator>();
        anim.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            anim.SetFloat("x", x);
            anim.SetFloat("y", y);
            //transform.Translate(x, y, 0);
            transform.position += new Vector3(x, y, 0).normalized * Time.deltaTime;

        }
    }



    //Command runs function on server
    [Command]
    void CmdFire(Vector3 direction, Vector2 offset)
    {
        //Debug.Log(playerrot);

        //transform.position+1.0*transform.forward,transform.rotation));
        var bullet = (GameObject)Instantiate(bulletPrefab,
            new Vector3(offset.x, offset.y, transform.position.z),
            Quaternion.identity); // rotation

        bullet.GetComponent<Rigidbody2D>().velocity = direction * 6;
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.GetComponent<Rigidbody2D>().transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        //spawn on clients
        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2.0f);
    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;
        //anim = GetComponent<Animator>();
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        fireDir(x,y);
        Debug.Log(transform.rotation);

        
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            bool isWalking = (Mathf.Abs(x) + Mathf.Abs(y)) > 0;
            CmdWalk(isWalking, x, y);
            //RpcWalk(isWalking, x, y);


        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var xsend = transform.position.x;
            var ysend = transform.position.y;
            var zsend = transform.position.z;

            Quaternion rot = transform.rotation;

            CmdFire(fireDirection, fireOffSet);
            //Debug.Log("Firing");
        }
    }

    void fireDir(float x, float y)
    {
        //Debug.Log(fireOffSet);
        if (x == -1 && y == 0)
        {
            fireDirection.x = -0.2f;
            fireDirection.y = 0;
            fireDirection.z = 0;
            fireOffSet = transform.position + fireDirection;
        }
        else if (x == 1 && y == 0)
        {
            fireDirection.x = 0.2f;
            fireDirection.y = 0;
            fireDirection.z = 0;
            fireOffSet = transform.position + fireDirection;
        }
        else if (x == 0 && y == 1)
        {
            fireDirection.x = 0;
            fireDirection.y = 0.2f;
            fireDirection.z = 0;
            fireOffSet = transform.position + fireDirection;
        }
        else if (x == 0 && y == -1)
        {
            fireDirection.x = 0;
            fireDirection.y = -0.2f;
            fireDirection.z = 0;
            fireOffSet = transform.position + fireDirection;
        }
        else if (x == -1 && y == -1)
        {
            fireDirection.x = -0.2f;
            fireDirection.y = -0.2f;
            fireDirection.z = 0;
            fireOffSet = transform.position + fireDirection;
        }
        else if (x == 1 && y == -1)
        {
            fireDirection.x = 0.2f;
            fireDirection.y = -0.2f;
            fireDirection.z = 0;
            fireOffSet = transform.position + fireDirection;
        }
        else if (x == 1 && y == 1)
        {
            fireDirection.x = 0.2f;
            fireDirection.y = 0.2f;
            fireDirection.z = 0;
            fireOffSet = transform.position + fireDirection;
        }
        else if (x == -1 && y == 1)
        {
            fireDirection.x = -0.2f;
            fireDirection.y = 0.2f;
            fireDirection.z = 0;
            fireOffSet = transform.position + fireDirection;
        }
    }
}
