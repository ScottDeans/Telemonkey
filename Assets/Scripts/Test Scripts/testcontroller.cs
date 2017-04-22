/*----------------
 Scott Deans-1700147
 GrantMacewan University
 4th Year Computer Science
------------------*/
using UnityEngine;
using System.Collections;

public class testcontroller : MonoBehaviour {
	private bool enabled;
	// Use this for initialization
	void Start () {
		if(!networkView.isMine)
			enabled=false;
	
	else
		enabled=true;
	}
	public float speed = 10f;
	// Update is called once per frame
	void Update () {
		if (enabled) {
						InputMovement ();
				}
	}
	void InputMovement()
	{
		if (Input.GetKey(KeyCode.W))
			rigidbody.MovePosition(rigidbody.position + Vector3.forward * speed * Time.deltaTime);
		
		if (Input.GetKey(KeyCode.S))
			rigidbody.MovePosition(rigidbody.position - Vector3.forward * speed * Time.deltaTime);
		
		if (Input.GetKey(KeyCode.D))
			rigidbody.MovePosition(rigidbody.position + Vector3.right * speed * Time.deltaTime);
		
		if (Input.GetKey(KeyCode.A))
			rigidbody.MovePosition(rigidbody.position - Vector3.right * speed * Time.deltaTime);
	}
}
