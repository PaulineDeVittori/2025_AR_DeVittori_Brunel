using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        // Vérification de la référence au manager
        if (prefabImagePairManager == null)
        {
            if (enableDebug)
                Debug.LogWarning("DogEatBehavior: PrefabImagePairManager est null");
            return;
        }
        
        // Récupère les objets instanciés à partir des noms d'image
        GameObject dog = prefabImagePairManager.GetInstantiatedPrefabByName(dogImageName);
        GameObject food = prefabImagePairManager.GetInstantiatedPrefabByName(foodImageName);
        
        // Debug : Vérifier si les objets sont trouvés
        if (enableDebug)
        {
            if (dog != lastDogObject)
            {
                Debug.Log($"DogEatBehavior: Chien trouvé = {(dog != null ? dog.name : "null")}");
                lastDogObject = dog;
            }
            
            if (food != lastFoodObject)
            {
                Debug.Log($"DogEatBehavior: Nourriture trouvée = {(food != null ? food.name : "null")}");
                lastFoodObject = food;
            }
        }
        
        // Vérifie que les objets existent et sont actifs dans la scène
        if (dog != null && food != null)
        {
            // Vérifier si les objets sont actifs
            bool dogActive = dog.activeInHierarchy;
            bool foodActive = food.activeInHierarchy;
            
            if (enableDebug && (!dogActive || !foodActive))
            {
                Debug.Log($"DogEatBehavior: Chien actif = {dogActive}, Nourriture active = {foodActive}");
            }
            
            if (dogActive && foodActive)
            {
                // Récupérer l'Animator si pas encore fait ou si l'objet a changé
                if (dogAnimator == null || lastDogObject != dog)
                {
                    dogAnimator = dog.GetComponent<Animator>();
                    
                    // Si pas d'Animator sur l'objet principal, chercher dans les enfants
                    if (dogAnimator == null)
                    {
                        dogAnimator = dog.GetComponentInChildren<Animator>();
                    }
                    
                    if (enableDebug)
                    {
                        Debug.Log($"DogEatBehavior: Animator trouvé = {(dogAnimator != null ? "Oui" : "Non")}");
                    }
                    
                    if (dogAnimator == null)
                    {
                        Debug.LogError($"DogEatBehavior: Aucun Animator trouvé sur {dog.name} ou ses enfants !");
                        return;
                    }
                }
                
                // Calculer la distance
                float distance = Vector3.Distance(dog.transform.position, food.transform.position);
                
                if (enableDebug)
                {
                    Debug.Log($"DogEatBehavior: Distance = {distance:F2}m (seuil: {eatDistance}m)");
                }
                
                // Vérifier la distance et déclencher l'animation
                if (distance < eatDistance && !hasEaten)
                {
                    if (dogAnimator != null)
                    {
                        // Vérifier si le trigger existe
                        if (HasTrigger(dogAnimator, eatTriggerName))
                        {
                            dogAnimator.SetTrigger(eatTriggerName);
                            hasEaten = true;
                            
                            if (enableDebug)
                            {
                                Debug.Log($"DogEatBehavior: Animation '{eatTriggerName}' déclenchée !");
                            }
                        }
                        else
                        {
                            Debug.LogError($"DogEatBehavior: Le trigger '{eatTriggerName}' n'existe pas dans l'Animator !");
                        }
                    }
                }
                else if (distance >= eatDistance && hasEaten)
                {
                    // Reset pour permettre une nouvelle animation
                    hasEaten = false;
                    
                    if (enableDebug)
                    {
                        Debug.Log("DogEatBehavior: Reset - peut manger à nouveau");
                    }
                }
            }
        }
        else
        {
            // Reset si les objets ne sont plus disponibles
            if (hasEaten)
            {
                hasEaten = false;
                dogAnimator = null;
                
                if (enableDebug)
                {
                    Debug.Log("DogEatBehavior: Objets perdus - reset du state");
                }
            }
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