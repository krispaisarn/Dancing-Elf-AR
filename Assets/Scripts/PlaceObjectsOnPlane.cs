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

    [SerializeField] private UIElement btn_hide;


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

    [SerializeField] private bool _isPlaneVisible = true;

    [SerializeField] private Transform _fxT;
    [SerializeField] private GameObject _floor;

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
                        spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, m_PlacedPrefab.transform.rotation);

                        m_NumberOfPlacedObjects++;

                        HidePlane();
                    }

                    if (_isPlaneVisible)
                    {
                        spawnedObject.transform.SetPositionAndRotation(hitPose.position, spawnedObject.transform.rotation);
                        HidePlane();
                    }

                    if (onPlacedObject != null)
                    {
                        onPlacedObject();
                    }
                }
            }
        }
        RotateChar();

    }

    private float _touchDownX;
    private float _rotateSpeed = 10;
    private void RotateChar()
    {
        if (spawnedObject == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _touchDownX = Input.mousePosition.x;
        }

        if (Input.GetMouseButton(0))
        {
            float touchXDelta = Input.mousePosition.x - _touchDownX;
            spawnedObject.transform.Rotate(Vector3.up, -touchXDelta * _rotateSpeed * Time.deltaTime);

            _touchDownX = Input.mousePosition.x;
        }
    }
    public void HidePlane()
    {
        _fxT.position = spawnedObject.transform.position;
        _floor.transform.position = spawnedObject.transform.position; // track effect and floor to character
        _floor.SetActive(true); //to not block raycast

        TogglePlaneDetection(false);
        btn_hide.gameObject.SetActive(true);
        _isPlaneVisible = false;

        if (spawnedObject != null)
            spawnedObject.SetActive(true);

    }

    public void ShowPlane()
    {
        _floor.SetActive(false);

        TogglePlaneDetection(true);
        btn_hide.gameObject.SetActive(false);
        _isPlaneVisible = true;

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
        var planes = _planeManager.trackables;
        foreach (var plane in planes)
        {
            plane.gameObject.SetActive(value);
        }

        _planeManager.enabled = value;

        var points = _cloudManager.trackables;
        foreach (var pts in points)
        {
            pts.gameObject.SetActive(value);
        }
        _cloudManager.enabled = value;
    }
}
