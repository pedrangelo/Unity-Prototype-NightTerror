using UnityEngine;
using System.Collections.Generic;

public class ActivityChecker : MonoBehaviour
{
    public List<GameObject> objectsToCheck; // Assign this list in the inspector with the objects you want to check
    public GameObject targetObjectToDisable; // Assign the target object that should be disabled if any objects in the list are active

    // Update is called once per frame
    void Update()
    {
        foreach (var obj in objectsToCheck)
        {
            // Check if the current object is not null and active in the hierarchy
            if (obj != null && obj.activeInHierarchy)
            {
                // If any object is active, disable the target object and stop checking further
                if (targetObjectToDisable != null)
                {
                    targetObjectToDisable.SetActive(false);
                }
                break; // Exit the loop as we found an active object
            }
        }
    }
}
