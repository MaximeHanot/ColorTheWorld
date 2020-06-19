using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraController : MonoBehaviour 
{
	[SerializeField] Vector2 m_rotationSpeed = Vector2.one;
	[SerializeField] Vector3 m_pivotOffset = Vector3.zero;

	[SerializeField] float m_maxDistance = 5f;
	[SerializeField] float m_zoomedDistance = 0f;
	[SerializeField] float m_colliderRadius = .2f;
	[SerializeField] LayerMask m_colliderMask;

	[SerializeField] Vector3 m_cameraOffset = Vector3.zero;

	[SerializeField] Transform m_target = null;

	[SerializeField] GameObject player;

	Vector2 m_inputRotation;

	[Header("Camera Clamped Vertical Values : only positive values")]
	[SerializeField]float minVerticalClampedRotation = 45;
	[SerializeField]float maxVerticalClampedRotation = 45;

	[Header("Lerp Values : Speed and Step")]
	[SerializeField, Range(.0f, 1.0F)] float m_positionLerpStep;

	[SerializeField, Range(.0f, 1.0F)] float m_rotationLerpStep;

	[SerializeField, Range(.0f, 1.0F)] float m_distanceLerpStep;

	[SerializeField, Range(.0f, 1.0F)] float m_recenterLerpStep;

	bool isZoomed = false;


	[HideInInspector] public bool isStatic;

	Vector3 m_lerpedPosition = Vector3.zero;
	Quaternion m_lerpedRotation = Quaternion.identity;
	float m_lerpedDistance = 0f;


	public Vector3 forward 
	{ 
		get 
		{
			return Quaternion.Euler (0f, transform.eulerAngles.y, 0f) * Vector3.forward;
		} 
	}

	public Vector3 right
	{
		get 
		{
			return Quaternion.Euler ( 0f, transform.eulerAngles.y, 0f ) * Vector3.right;
		}
	}
    
	public void Zoom()
	{
		isZoomed = true;
	}

	public void Dezoom()
	{
		isZoomed = false;
	}

	public void Rotate( float _x, float _y )
	{

		m_inputRotation.x = Mathf.Clamp (m_inputRotation.x, -minVerticalClampedRotation, maxVerticalClampedRotation);
		m_inputRotation.x += _x;
		m_inputRotation.y += _y;

	}
    
	private void MoveToPivotPosition()
	{
		m_lerpedPosition = Vector3.Lerp ( m_lerpedPosition,  m_target.TransformPoint( m_pivotOffset ), m_positionLerpStep);
		transform.position = m_lerpedPosition;
	}

	private void MoveToCameraPosition()
	{
		transform.position += transform.TransformDirection( m_cameraOffset );
	}

	private void RotateToRotation()
	{
		m_lerpedRotation = Quaternion.Lerp(m_lerpedRotation,Quaternion.Euler ( m_inputRotation.x, m_inputRotation.y, 0), m_rotationLerpStep);

		transform.rotation = m_lerpedRotation;
	}

	private void GoBack()
	{
		m_lerpedDistance = Mathf.Lerp ( m_lerpedDistance, GetCollisionDistance (), m_distanceLerpStep);

		if ( isZoomed )
			m_lerpedDistance = Mathf.Max ( 0.5f, m_zoomedDistance );

		transform.position -= transform.forward * m_lerpedDistance;
	}

	private float GetCollisionDistance()
	{
		RaycastHit hit;
		Debug.DrawRay ( m_lerpedPosition, ( -transform.forward * m_maxDistance ) + transform.InverseTransformDirection ( m_cameraOffset ), Color.green );

		if ( Physics.SphereCast ( m_lerpedPosition, m_colliderRadius, -transform.forward + transform.InverseTransformDirection ( m_cameraOffset ), out hit, m_maxDistance, m_colliderMask, QueryTriggerInteraction.Ignore) )
		{
			return hit.distance;
		}
		else
			return m_maxDistance;
	}

	void Awake()
	{
		m_lerpedPosition = m_target.position;
		m_lerpedDistance = GetCollisionDistance ();

		gameObject.transform.position = new Vector3(player.transform.position.x - 1.5f, player.transform.position.y, player.transform.position.z);
	}

	float xRotation;
	void LateUpdate()
	{		

		float yRotation = (/*Input.GetAxis ("RightStickHorizontal") + */Input.GetAxis ("Mouse X"))* m_rotationSpeed.x ;

		//if(PlayerPrefs.GetInt ("YAxis") == 0)
		//{
			xRotation = /*-*/(/*-Input.GetAxis ("RightStickVertical")*/ - Input.GetAxis ("Mouse Y"))* m_rotationSpeed.y ;
		//}
		//else if (PlayerPrefs.GetInt ("YAxis") == 1)
		//{
		//	xRotation = -(/*-Input.GetAxis ("RightStickVertical")*/ - Input.GetAxis ("Mouse Y"))* m_rotationSpeed.y ;
		//}

		Rotate ( xRotation  * Time.deltaTime, yRotation  * Time.deltaTime );

		MoveToPivotPosition ();

		RotateToRotation ();

		GoBack ();

		MoveToCameraPosition ();



        
		//if (Input.GetAxisRaw ("Target") > 0.5f  || Input.GetButton ("TargetMouse")) { 
		//	Zoom ();
		//} else {
		//	Dezoom ();
		//}
	}

}
