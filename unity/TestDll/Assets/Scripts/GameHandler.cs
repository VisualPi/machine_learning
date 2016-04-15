using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameHandler : MonoBehaviour
{

	public Camera _camera;
	public Material blue;
	public Material white;
	private int _RAYCAST_DISTANCE = 10000;

	public GameObject boules;
	private List<GameObject> balls;

	void Start()
	{
		balls = new List<GameObject>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
			balls.Add(boules.transform.GetChild(i).gameObject);
	}

	void Update()
	{
		if( Input.GetMouseButton(0) )
		{
			GameObject entity = tryGetTarget();
			if( entity )
			{
				entity.GetComponent<MeshRenderer>().material = blue;
				entity.GetComponent<BallMNISTInfo>().state = 1;
			}
				
		}
	}

	private Vector3 getTarget()
	{
		Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;

		if( Physics.Raycast(ray, out hit, _RAYCAST_DISTANCE) )
		{
			Debug.DrawLine(ray.origin, hit.point, Color.green, 1f);
			return hit.point;
		}
		return Vector3.zero;
	}
	private GameObject tryGetTarget()
	{
		Vector3 mousepos = Input.mousePosition;
		Ray ray = _camera.ScreenPointToRay(mousepos);
		RaycastHit hit;

		if( Physics.Raycast(ray, out hit, _RAYCAST_DISTANCE) )
		{
			Debug.DrawLine(ray.origin, hit.point, Color.red, 0.5f);
			return hit.collider.gameObject;
		}
		return null;
	}

	public void OnClearClick()
	{
		foreach( var b in balls )
		{
			b.GetComponent<MeshRenderer>().material = white;
			b.GetComponent<BallMNISTInfo>().state = 0;
        }
			
	}
}
