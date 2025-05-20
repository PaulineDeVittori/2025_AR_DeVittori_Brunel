using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;


public class ImageRecognitionExample : MonoBehaviour
{
    private ARTrackedImageManager _aRTrackedImageManager;

    private void Awake ( )
    {
        _aRTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    private void OnEnable ( )
    {
        _aRTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    public void OnDisable ( )
    {
        _aRTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    public void OnImageChanged( ARTrackedImagesChangedEventArgs args )
    {
        foreach ( var trackedImage in args . added )
        {
            Debug . Log ( trackedImage .name ) ;
        }
    }
}
