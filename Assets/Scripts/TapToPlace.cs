using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapToPlace : MonoBehaviour
{
    private List<ARRaycastHit> s_Hits;

    private ARRaycastManager m_RaycastManager;
    private ARReferencePointManager m_ReferencePointManager;
    private List<ARReferencePoint> m_ReferencePoint;
    private ARPlaneManager m_PlaneManager;

    public GameObject nextButton;

    //Remove all reference points created
    public void RemoveAllReferencePoints()
    {
        foreach (var referencePoint in m_ReferencePoint)
        {
            m_ReferencePointManager.RemoveReferencePoint(referencePoint);
        }
        m_ReferencePoint.Clear();
    }

    void Start()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_ReferencePointManager = GetComponent<ARReferencePointManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
        m_ReferencePoint = new List<ARReferencePoint>();
        s_Hits = new List<ARRaycastHit>();
        nextButton.SetActive(false);
    }


    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;
        // touchPosition = new Vector2(touchPosition.x, touchPosition.y + 600);
        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;
            TrackableId planeId = s_Hits[0].trackableId; //get the ID of the plane hit by the raycast
            var referencePoint = m_ReferencePointManager.AttachReferencePoint(m_PlaneManager.GetPlane(planeId), hitPose);
            if (referencePoint != null)
            {
                RemoveAllReferencePoints();
                m_ReferencePoint.Add(referencePoint);
            }
            foreach (var plane in m_PlaneManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
            nextButton.SetActive(true);
            enabled = false;
        }
    }
}
