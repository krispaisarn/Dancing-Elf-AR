using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObjectsOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    [SerializeField]

    Transform _arCamera;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    /// <summary>
    /// Invoked whenever an object is placed in on a plane.
    /// </summary>
    public static event Action onPlacedObject;

    ARRaycastManager m_RaycastManager;

    [SerializeField] private ARPlaneManager _planeManager;
    [SerializeField] private ARPointCloudManager _cloudManager;


    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    [SerializeField]
    int m_MaxNumberOfObjectsToPlace = 1;

    int m_NumberOfPlacedObjects = 0;

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = s_Hits[0].pose;

                    if (m_NumberOfPlacedObjects < m_MaxNumberOfObjectsToPlace)
                    {
                        spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                        Vector3 direction_to_camera = _arCamera.transform.position - spawnedObject.transform.position;
                        Quaternion rotation = Quaternion.LookRotation(direction_to_camera, Vector3.up);
                        transform.rotation = rotation;

                        HidePlane();
                        m_NumberOfPlacedObjects++;
                    }
                    else
                    {
                        spawnedObject.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                        HidePlane();
                    }

                    if (onPlacedObject != null)
                    {
                        onPlacedObject();
                    }
                }
            }
        }
    }

    public void HidePlane()
    {
        TogglePlaneDetection(false);

        if (spawnedObject != null)
            spawnedObject.SetActive(true);

    }

    public void ShowPlane()
    {
        TogglePlaneDetection(true);
        if (spawnedObject != null)
            spawnedObject.SetActive(false);
    }

    /// <summary>
    /// Toggles plane detection and the visualization of the planes.
    /// </summary>
    public void TogglePlaneDetection(bool _isEnabled)
    {
        SetAllPlanesActive(_isEnabled);

    }

    /// <summary>
    /// Iterates over all the existing planes and activates
    /// or deactivates their <c>GameObject</c>s'.
    /// </summary>
    /// <param name="value">Each planes' GameObject is SetActive with this value.</param>
    void SetAllPlanesActive(bool value)
    {
        foreach (var plane in _planeManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }

        _planeManager.planePrefab.SetActive(value);
        _cloudManager.pointCloudPrefab.gameObject.SetActive(value);
    }
}
