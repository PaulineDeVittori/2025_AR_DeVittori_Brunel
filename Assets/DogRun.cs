using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation.Samples;


public class DogRun : MonoBehaviour
{
    public PrefabImagePairManager prefabImagePairManager;

    public string dogImageName = "dog_marker";
    public string ballImageName = "ball_marker";

    public float runDistance = 0.7f;
    public string runTriggerName = "Run";

    public bool enableDebug = true;

    private Animator dogAnimator;
    private bool isRunning = false;

    private GameObject lastDogObject;
    private GameObject lastBallObject;

    void Update()
    {
        if (prefabImagePairManager == null) return;

        GameObject dog = prefabImagePairManager.GetInstantiatedPrefabByName(dogImageName);
        GameObject ball = prefabImagePairManager.GetInstantiatedPrefabByName(ballImageName);

        if (dog == null || ball == null) 
        {
            if (isRunning)
            {
                isRunning = false;
                if (dogAnimator != null) dogAnimator.ResetTrigger(runTriggerName);
                if (enableDebug) Debug.Log("DogRunBehavior: Chien ou balle non trouvés, arrêt de la course");
            }
            return;
        }

        if (dogAnimator == null || lastDogObject != dog)
        {
            dogAnimator = dog.GetComponent<Animator>() ?? dog.GetComponentInChildren<Animator>();
            if (dogAnimator == null)
            {
                Debug.LogError("DogRunBehavior: Aucun Animator trouvé sur le chien !");
                return;
            }
            lastDogObject = dog;
        }

        float distance = Vector3.Distance(dog.transform.position, ball.transform.position);

        if (enableDebug)
            Debug.Log($"DogRunBehavior: Distance chien-balle = {distance:F2} (seuil = {runDistance})");

        if (distance < runDistance && !isRunning)
        {
            if (HasTrigger(dogAnimator, runTriggerName))
            {
                dogAnimator.SetTrigger(runTriggerName);
                isRunning = true;
                if (enableDebug) Debug.Log("DogRunBehavior: Animation 'Run' déclenchée");
            }
            else
            {
                Debug.LogError($"DogRunBehavior: Trigger '{runTriggerName}' absent dans l'Animator");
            }
        }
        // Note : On ne fait rien pour arrêter la course (pas de reset du trigger),
        // car tu souhaites que le chien continue de courir sans s'arrêter.
    }

    private bool HasTrigger(Animator animator, string triggerName)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
            return false;

        foreach (var param in animator.parameters)
        {
            if (param.name == triggerName && param.type == AnimatorControllerParameterType.Trigger)
                return true;
        }
        return false;
    }
}