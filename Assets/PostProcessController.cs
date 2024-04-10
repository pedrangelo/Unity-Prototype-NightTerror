using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SanityEffectsControllerURP : MonoBehaviour
{
    public Transform targetObject;
    public Volume volume;
    private Vignette vignette;
    public float maxVignetteIntensity = 0.5f;
    public float vignetteIncreaseSpeed = 0.5f;
    public float vignetteDecreaseSpeed = 1f;

    private bool targetInView = false;

    void Start()
    {
        if (!volume.profile.TryGet(out vignette))
        {
            Debug.LogError("Vignette effect not found on the volume profile.");
        }
    }

    void Update()
{
    RaycastHit hit;

    // Cast a ray from the center of the camera's view
    Ray ray = new Ray(transform.position, transform.forward);

    if (Physics.Raycast(ray, out hit))
    {
        // Check if the ray hits the target object
        targetInView = hit.transform == targetObject;
    }
    else
    {
        targetInView = false;
    }

    // Adjust vignette based on whether the target is in view
    if (targetInView)
    {
        IncreaseEffects();
    }
    else
    {
        DecreaseEffects();
    }
}

    void IncreaseEffects()
    {
        // Ensure vignette is not null
        if (vignette != null)
        {
            // Smoothly increase the vignette intensity
            vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, maxVignetteIntensity, vignetteIncreaseSpeed * Time.deltaTime);
        }
    }

    void DecreaseEffects()
    {
        // Ensure vignette is not null
        if (vignette != null)
        {
            // Smoothly decrease the vignette intensity
            vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, 0, vignetteDecreaseSpeed * Time.deltaTime);
        }
    }
}
