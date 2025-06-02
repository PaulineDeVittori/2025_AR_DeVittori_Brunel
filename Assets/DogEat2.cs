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
    public string runTriggerName = "Run";

    public int repeatEatCount = 3; // 🔁 Nombre de répétitions
    public float animationInterval = 0f; // ⏱ Délai entre deux animations (adapter à la durée réelle de ton anim)

    public bool enableDebug = true;

    private Animator dogAnimator;
    private bool isEating = false;
    private GameObject lastDogObject;
    private GameObject lastFoodObject;

    void Start()
    {
        if (prefabImagePairManager == null)
        {
            Debug.LogError("DogEatBehavior: PrefabImagePairManager n'est pas assigné !");
            return;
        }

        if (enableDebug)
        {
            Debug.Log($"DogEatBehavior: Recherche d'objets avec les noms '{dogImageName}' et '{foodImageName}'");
        }
    }

    void Update()
    {
        if (prefabImagePairManager == null) return;

        GameObject dog = prefabImagePairManager.GetInstantiatedPrefabByName(dogImageName);
        GameObject food = prefabImagePairManager.GetInstantiatedPrefabByName(foodImageName);

        if (dog != null && food != null && dog.activeInHierarchy && food.activeInHierarchy)
        {
            if (dogAnimator == null || lastDogObject != dog)
            {
                dogAnimator = dog.GetComponent<Animator>() ?? dog.GetComponentInChildren<Animator>();
                lastDogObject = dog;
            }

            float distance = Vector3.Distance(dog.transform.position, food.transform.position);

            if (enableDebug)
                Debug.Log($"Distance = {distance:F2}m (seuil: {eatDistance}m)");

            if (distance < eatDistance && !isEating)
            {
                if (dogAnimator != null && HasTrigger(dogAnimator, eatTriggerName))
                {
                    StartCoroutine(PlayEatAnimationRepeatedly());
                }
            }
        }
    }

    IEnumerator PlayEatAnimationRepeatedly()
    {
        isEating = true;

        for (int i = 0; i < repeatEatCount; i++)
        {
            dogAnimator.SetTrigger(eatTriggerName);
            if (enableDebug) Debug.Log($"Animation '{eatTriggerName}' déclenchée ({i + 1}/{repeatEatCount})");
            yield return new WaitForSeconds(animationInterval);
        }

        isEating = false;
    }

    private bool HasTrigger(Animator animator, string triggerName)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
            return false;

        foreach (var parameter in animator.parameters)
        {
            if (parameter.name == triggerName && parameter.type == AnimatorControllerParameterType.Trigger)
                return true;
        }
        return false;
    }

    public void ForceEat()
    {
        if (dogAnimator != null && !isEating)
        {
            StartCoroutine(PlayEatAnimationRepeatedly());
            Debug.Log("DogEatBehavior: Animation forcée");
        }
    }

    public void ResetEatState()
    {
        isEating = false;
        Debug.Log("DogEatBehavior: State reset");
    }

    void OnGUI()
    {
        if (enableDebug && Application.isPlaying)
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 200));
            GUILayout.Label($"Dog Object: {(lastDogObject != null ? lastDogObject.name : "null")}");
            GUILayout.Label($"Food Object: {(lastFoodObject != null ? lastFoodObject.name : "null")}");
            GUILayout.Label($"Animator: {(dogAnimator != null ? "Found" : "null")}");
            GUILayout.Label($"Is Eating: {isEating}");

            if (lastDogObject != null && lastFoodObject != null)
            {
                float dist = Vector3.Distance(lastDogObject.transform.position, lastFoodObject.transform.position);
                GUILayout.Label($"Distance: {dist:F2}m");
            }

            if (GUILayout.Button("Force Eat"))
            {
                ForceEat();
            }

            if (GUILayout.Button("Reset State"))
            {
                ResetEatState();
            }

            GUILayout.EndArea();
        }
    }
}