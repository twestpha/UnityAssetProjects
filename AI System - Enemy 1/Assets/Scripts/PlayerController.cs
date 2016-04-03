using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private NavMeshAgent agent;

	void Start(){
		agent = gameObject.GetComponent<NavMeshAgent>();
	}

	void Update(){
		Vector3 zMovement = Vector3.zero;

		if(Input.GetKey("w")){
			zMovement = Vector3.forward;
		} else if(Input.GetKey("s")){
			zMovement = Vector3.forward * -1;
		}



		NavMeshPath path = new NavMeshPath();
		agent.CalculatePath(transform.position + zMovement, path);

		if(path.status == NavMeshPathStatus.PathComplete){
			// print("The agent can reach the destination");
			agent.SetPath(path);
		}
	}
}
