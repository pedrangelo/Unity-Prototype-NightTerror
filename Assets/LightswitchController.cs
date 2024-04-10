using UnityEngine;

public class LightSwitchController : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject targetObject;
    public GameObject objectToToggle;
    public Material highlightMaterial; // Assign a highlight material in the inspector
    private Material originalMaterial; // To store the original material
    public float interactRange = 3f;

    private Renderer targetRenderer; // Renderer of the target object

    void Start()
    {
        targetRenderer = targetObject.GetComponent<Renderer>();
        if (targetRenderer != null)
        {
            // Store the original material
            originalMaterial = targetRenderer.material;
        }
    }

    void Update()
    {
        bool isTargetInteractable = false;

        // Perform a raycast from the center of the camera's view
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // If the ray hits an object within interactRange...
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            Debug.Log($"Ray hit: {hit.collider.gameObject.name}");
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactRange, Color.red);
            // Check if the hit object is the target object
            if (hit.collider.gameObject == targetObject)
            {
                isTargetInteractable = true;
                Debug.Log("Target is interactable."); // Debug log

                // Check for mouse input
                if (Input.GetMouseButtonDown(0))
                {
                    // Toggle the active state of the objectToToggle
                    objectToToggle.SetActive(!objectToToggle.activeSelf);
                    Debug.Log($"Toggled object {(objectToToggle.activeSelf ? "On" : "Off")}"); // Debug log
                }
            }
        }

        // Highlight the target object if it's interactable, otherwise revert to the original material
        if (isTargetInteractable)
        {
            targetRenderer.material = highlightMaterial;
        }
        else
        {
            targetRenderer.material = originalMaterial;
        }
    }

    // Use Gizmos to draw interactable range and direction
    void OnDrawGizmos()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * interactRange);
            Gizmos.DrawWireSphere(playerCamera.transform.position + playerCamera.transform.forward * interactRange, 0.5f);
        }
    }
}
