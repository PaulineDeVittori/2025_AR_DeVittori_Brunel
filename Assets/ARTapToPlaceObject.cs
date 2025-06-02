using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceMultipleObjects : MonoBehaviour
{
    public List<GameObject> prefabList; // Drag and drop multiple prefabs (plantes) in Inspector
    private ARRaycastManager _arRaycastManager;
    private GameObject selectedObject;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        Vector2 touchPosition = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            // 1. Sélectionner l'objet si on en touche un
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            RaycastHit hitObject;
            if (Physics.Raycast(ray, out hitObject))
            {
                if (hitObject.collider != null)
                {
                    selectedObject = hitObject.collider.gameObject;
                    return;
                }
            }

            // 2. Sinon, placer un nouvel objet si on touche le sol
            if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                // Choisir un prefab aléatoire depuis la liste
                if (prefabList.Count > 0)
                {
                    int randomIndex = Random.Range(0, prefabList.Count);
                    GameObject newObject = Instantiate(prefabList[randomIndex], hitPose.position, hitPose.rotation);
                }
            }
        }
        else if (touch.phase == TouchPhase.Moved && selectedObject != null)
        {
            // Déplacement de l'objet sélectionné
            if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose movePose = hits[0].pose;
                selectedObject.transform.position = movePose.position;
            }
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            selectedObject = null;
        }
    }
}