using UnityEngine;

public class DogEatBehavior : MonoBehaviour
{
    public GameObject dog;   // À assigner dans l’inspecteur
    public GameObject food;  // À assigner dans l’inspecteur
    public float eatDistance = 0.5f;

    private bool hasEaten = false;

    void Update()
    {
        if (dog == null || food == null) return;

        float distance = Vector3.Distance(dog.transform.position, food.transform.position);

        if (distance < eatDistance && !hasEaten)
        {
            Animator animator = dog.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Eat");
                hasEaten = true;
            }
        }

        // Réinitialiser si la nourriture s’éloigne
        if (distance > eatDistance + 0.2f)
        {
            hasEaten = false;
        }
    }
}