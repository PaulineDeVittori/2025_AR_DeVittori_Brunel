using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation.Samples;

public class DogEat2 : MonoBehaviour
{
    
    public PrefabImagePairManager prefabImagePairManager;
    

    public string dogImageName = "dog_marker";      // Nom exact dans la bibliothèque d'images AR pour le chien
    public string foodImageName = "food_marker";    // Nom exact dans la bibliothèque d'images AR pour la nourriture
    public float eatDistance = 0.5f;              // Distance seuil pour déclencher l'animation
    public string eatTriggerName = "Eat";         // Nom du trigger dans l'Animator
    

    public bool enableDebug = true;               // Activer/désactiver les logs de debug
    
    // Variables privées
    private Animator dogAnimator;
    private bool hasEaten = false;
    private GameObject lastDogObject;
    private GameObject lastFoodObject;
    
    void Start()
    {
        // Vérifications initiales
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

        // *Déclenche l'animation une fois si proche et ne la stoppe jamais*
        if (distance < eatDistance && !hasEaten)
        {
            if (dogAnimator != null && HasTrigger(dogAnimator, eatTriggerName))
            {
                dogAnimator.SetTrigger(eatTriggerName);
                hasEaten = true;
                if (enableDebug) Debug.Log($"Animation '{eatTriggerName}' déclenchée !");
            }
        }

        // Ne remet jamais hasEaten à false, donc l'animation ne s'arrête pas
    }
    }
    
    // Méthode utilitaire pour vérifier si un trigger existe dans l'Animator
    private bool HasTrigger(Animator animator, string triggerName)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
            return false;
            
        foreach (var parameter in animator.parameters)
        {
            if (parameter.name == triggerName && parameter.type == AnimatorControllerParameterType.Trigger)
            {
                return true;
            }
        }
        return false;
    }
    
    // Méthodes publiques pour debug et test
    public void ForceEat()
    {
        if (dogAnimator != null)
        {
            dogAnimator.SetTrigger(eatTriggerName);
            hasEaten = true;
            Debug.Log("DogEatBehavior: Animation forcée");
        }
    }
    
    public void ResetEatState()
    {
        hasEaten = false;
        Debug.Log("DogEatBehavior: State reset");
    }
    
    // Affichage de debug dans l'Inspector
    void OnGUI()
    {
        if (enableDebug && Application.isPlaying)
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 200));
            GUILayout.Label($"Dog Object: {(lastDogObject != null ? lastDogObject.name : "null")}");
            GUILayout.Label($"Food Object: {(lastFoodObject != null ? lastFoodObject.name : "null")}");
            GUILayout.Label($"Animator: {(dogAnimator != null ? "Found" : "null")}");
            GUILayout.Label($"Has Eaten: {hasEaten}");
            
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