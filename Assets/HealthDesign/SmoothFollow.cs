using UnityEngine;
using System.Collections;


public class SmoothFollow : MonoBehaviour
{
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public Vector3 cameraOffset;
	public bool useFixedUpdate = false;
	
	private PlayerPhysics _playerPhysics;
	private Vector3 _smoothDampVelocity;
	
	
	void Awake()
	{
        _playerPhysics = GetComponent<PlayerPhysics>();
	}
	
	void LateUpdate()
	{
		if( !useFixedUpdate )
			updateCameraPosition();
	}


	void FixedUpdate()
	{
		if( useFixedUpdate )
			updateCameraPosition();
	}


	void updateCameraPosition()
	{
		if( _playerPhysics.velocity.x > 0 )
		{
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, _playerPhysics.transform.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime);
		}
		else
		{
			var leftOffset = cameraOffset;
			leftOffset.x *= -1;
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, _playerPhysics.transform.position - leftOffset, ref _smoothDampVelocity, smoothDampTime);
		}
	}
	
}
