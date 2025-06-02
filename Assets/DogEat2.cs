using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation.Samples;

public class DogEat2 : MonoBehaviour
{
    public PrefabImagePairManager prefabImagePairManager;

    public string dogImageName = "dog_marker";
    public string foodImageName = "food_marker";
    public float eatDistance = 0.5f;
    public string eatTriggerName = "Eat";
    public bool enableDebug = true;

    private Animator dogAnimator;
    private GameObject dog;
    private GameObject food;
    private int eatCount = 0;
    private bool wasInRange = false;

    void Update()
    {
        if (prefabImagePairManager == null) return;

        dog = prefabImagePairManager.GetInstantiatedPrefabByName(dogImageName);
        food = prefabImagePairManager.GetInstantiatedPrefabByName(foodImageName);

        if (dog == null || food == null || !dog.activeInHierarchy || !food.activeInHierarchy)
            return;

        if (dogAnimator == null)
            dogAnimator = dog.GetComponentInChildren<Animator>();

        float distance = Vector3.Distance(dog.transform.position, food.transform.position);
        bool inRange = distance < eatDistance;

        if (enableDebug)
            Debug.Log($"Distance: {distance:F2}, inRange: {inRange}, eatCount: {eatCount}");

        // Passage de hors portée → en portée
        if (inRange && !wasInRange)
        {
            if (eatCount < 3 && HasTrigger(dogAnimator, eatTriggerName))
            {
                dogAnimator.SetTrigger(eatTriggerName);
                eatCount++;
                if (enableDebug) Debug.Log($"Eat déclenché ({eatCount}/3)");
            }
        }

        // Passage de en portée → hors portée
        if (!inRange && wasInRange)
        {
            if (enableDebug) Debug.Log("Carte éloignée → on réinitialise eatCount");
            eatCount = 0;
        }

        wasInRange = inRange;
    }

    private bool HasTrigger(Animator animator, string triggerName)
    {
        if (animator == null || animator.runtimeAnimatorController == null) return false;

        foreach (var param in animator.parameters)
            if (param.name == triggerName && param.type == AnimatorControllerParameterType.Trigger)
                return true;

        return false;
    }
}
