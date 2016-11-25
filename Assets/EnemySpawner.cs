using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

    public GameObject enemyPrefab;
    public int numEnemies;

    public override void OnStartServer()
    {
        for(int i = 0; i<numEnemies; i++)
        {
            var pos = new Vector3(
                Random.Range(-1.0f, 1.0f),
                0.2f,
                Random.Range(-1.0f, 1.0f)
                );

            var rotation = Quaternion.Euler(
                0,0, 0);

            var enemy = (GameObject)Instantiate(enemyPrefab, pos, rotation);
            NetworkServer.Spawn(enemy);
            
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
