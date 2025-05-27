using UnityEngine;
using UnityEngine.XR.ARFoundation.Samples;

public class DogEatBehavior : MonoBehaviour
{
    public PrefabImagePairManager prefabImagePairManager;

    public string dogImageName = "chien";      // Nom exact dans la bibliothèque d'images AR pour le chien
    public string foodImageName = "nourriture";    // Nom exact dans la bibliothèque d'images AR pour la nourriture

    public float eatDistance = 0.5f;               // Distance seuil pour déclencher l'animation

    private Animator dogAnimator;
    private bool hasEaten = false;

    void Update()
    {
        // Récupère les objets instanciés à partir des noms d'image
        GameObject dog = prefabImagePairManager.GetInstantiatedPrefabByName(dogImageName);
        GameObject food = prefabImagePairManager.GetInstantiatedPrefabByName(foodImageName);
        Debug.Log(dog.activeInHierarchy);
        Debug.Log(food.activeInHierarchy);
        // Vérifie que les objets existent et sont actifs dans la scène
        if (dog != null && food != null && dog.activeInHierarchy && food.activeInHierarchy)
        {
            if (dogAnimator == null)
            {
                dogAnimator = dog.GetComponent<Animator>();
            }

            float distance = Vector3.Distance(dog.transform.position, food.transform.position);

            if (distance < eatDistance && !hasEaten)
            {
                dogAnimator.SetTrigger("Eat");
                hasEaten = true;
            }
            else if (distance >= eatDistance && hasEaten)
            {
                // Optionnel : reset du trigger si la nourriture s’éloigne (selon ton besoin)
                hasEaten = false;
            }
        }
    }
}