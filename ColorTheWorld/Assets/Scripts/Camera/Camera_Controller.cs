using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform m_target = null;
    [SerializeField] GameObject player;

    [Header("Collisions Infos")]
    public LayerMask collisionLayer;
    [SerializeField] float m_colliderRadius = .2f;

    Vector3 m_lerpedPosition = Vector3.zero;
    float m_lerpedDistance = 0f;

    [Header("Zoom Infos")]
    [SerializeField] float m_maxDistance = 5f;
    [SerializeField] float m_zoomedDistance = 0f;
    bool isZoomed = false;

    [Header("Offsets")]
    [SerializeField] Vector3 m_cameraOffset = Vector3.zero;
    [SerializeField] Vector3 m_pivotOffset = Vector3.zero;

    [Header("Lerp Values : Speed and Step")]
    [SerializeField, Range(.0f, 1.0F)] float m_positionLerpStep;
    [SerializeField, Range(.0f, 1.0F)] float m_distanceLerpStep;

    void Awake()
    {
        m_lerpedPosition = m_target.position;
        m_lerpedDistance = GetCollisionDistance();

        gameObject.transform.position = new Vector3(player.transform.position.x , player.transform.position.y, player.transform.position.z);

      
    }

    private void FixedUpdate()
    {
        MoveToPivotPosition();
        GoBack();
        MoveToCameraPosition();
    }

    private void GoBack()
    {
        m_lerpedDistance = Mathf.Lerp(m_lerpedDistance, GetCollisionDistance(), m_distanceLerpStep);

        //if (isZoomed)
        //    m_lerpedDistance = Mathf.Max(0.5f, m_zoomedDistance);

        transform.position -= transform.forward * m_lerpedDistance;


        //if (Input.GetAxisRaw("Target") > 0.5f || Input.GetButton("TargetMouse"))
        //{
        //    Zoom();
        //}
        //else
        //{
        //    Dezoom();
        //}

    }

    private float GetCollisionDistance()
    {
        RaycastHit hit;
        Debug.DrawRay(m_lerpedPosition, (-transform.forward * m_maxDistance) + transform.InverseTransformDirection(m_cameraOffset), Color.green);

        if (Physics.SphereCast(m_lerpedPosition, m_colliderRadius, -transform.forward + transform.InverseTransformDirection(m_cameraOffset), out hit, m_maxDistance, collisionLayer/*, QueryTriggerInteraction.Ignore*/))
        {
            return hit.distance;
        }
        else
            return m_maxDistance;

    }

    private void MoveToPivotPosition()
    {
        m_lerpedPosition = Vector3.Lerp(m_lerpedPosition, m_target.TransformPoint(m_pivotOffset), m_positionLerpStep);
        transform.position = m_lerpedPosition;
    }

    private void MoveToCameraPosition()
    {
        transform.position += transform.TransformDirection(m_cameraOffset);
    }


    public void Zoom()
    {
        isZoomed = true;
    }

    public void Dezoom()
    {
        isZoomed = false;
    }

}
