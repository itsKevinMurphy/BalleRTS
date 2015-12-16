using UnityEngine;
using System.Collections;

public class UnitSpawner : MonoBehaviour {

	public Transform target;
	public GameObject unit;
	
	public float spawnTime = 0.1f;
	float spawnTimeLeft = 3f;

	// Update is called once per frame
	void Update () {
		if(spawnTimeLeft <=0) {
			GameObject go = (GameObject)Instantiate(unit, transform.position, transform.rotation);
			go.GetComponent<AIPath>().target = target;
			spawnTimeLeft = spawnTime;
		}
		else {
			spawnTimeLeft -= Time.deltaTime;
		}
	}
}
