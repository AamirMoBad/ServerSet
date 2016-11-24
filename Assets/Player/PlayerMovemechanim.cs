using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMovemechanim : NetworkBehaviour
{
    Animator anim;
    public GameObject bulletPrefab;

    public override void OnStartLocalPlayer()
    {
        anim = GetComponent<Animator>();
    }

    //Command runs function on server
    [Command]
    void CmdFire()
    {
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            transform.position + transform.right, //set var
            Quaternion.identity); // rotation

        bullet.GetComponent<Rigidbody2D>().velocity = transform.right * 4; 

        //spawn on clients
        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2.0f);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxisRaw("Horizontal") * 0.1f;
        var y = Input.GetAxisRaw("Vertical") * 0.1f;


        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {


            bool isWalking = (Mathf.Abs(x) + Mathf.Abs(y)) > 0;

            anim.SetBool("isWalking", isWalking);

            if (isWalking)
            {
                anim.SetFloat("x", x);
                anim.SetFloat("y", y);
                //transform.Translate(x, y, 0);
                transform.position += new Vector3(x, y, 0).normalized * Time.deltaTime;

            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
            Debug.Log("Firing");
        }
    }


}
