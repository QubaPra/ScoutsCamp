using UnityEngine;

public class TentPlacement : MonoBehaviour
{
    public GameObject tentPrefab;  // Prefab of the tent
    private GameObject instantiatedTent;  // Reference to the instantiated tent
    private bool followCamera;  // Flag to indicate if the tent should follow the camera
    public float gridSize = 1f; // Size of the grid

    private Renderer planeRenderer; // Renderer component of the plane inside the prefab

    void Start()
    {
        // Get the Renderer component of the plane inside the prefab
        planeRenderer = tentPrefab.GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (instantiatedTent == null)
            {
                PlaceTent();
                followCamera = true;
            }
            else if (followCamera)
            {
                followCamera = false;
                // Find and remove the plane object inside the instantiated tent
                GameObject planeObject = instantiatedTent.transform.Find("Plane").gameObject;
                Destroy(planeObject);
            }
            else
            {
                PlaceTent();
                followCamera = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (instantiatedTent != null)
            {
                // Rotate the tent by 90 degrees around the y-axis
                instantiatedTent.transform.Rotate(0f, 90f, 0f);

                // Change the color of the plane to red
                planeRenderer.material.color = Color.red;
            }
        }

        if (followCamera && instantiatedTent != null)
        {
            // Update the position of the tent to match the camera's center
            Vector3 cameraCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            Ray ray = Camera.main.ScreenPointToRay(cameraCenter);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Map"))
            {
                Vector3 tentPosition = hit.point;
                tentPosition.y = -20f;  // Set the y-coordinate to -20

                // Snap tent position to the grid
                tentPosition.x = Mathf.Round(tentPosition.x / gridSize) * gridSize;
                tentPosition.z = Mathf.Round(tentPosition.z / gridSize) * gridSize;

                instantiatedTent.transform.position = tentPosition;
            }
        }
    }

    void PlaceTent()
    {
        // Calculate the center position of the screen
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

        // Create a ray from the camera to the screen center position
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        // Perform a raycast and check if it hits something
        if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Map"))
        {
            // Get the hit point on the map
            Vector3 tentPosition = hit.point;
            tentPosition.y = -20f;  // Set the y-coordinate to -20

            // Snap tent position to the grid
            tentPosition.x = Mathf.Round(tentPosition.x / gridSize) * gridSize;
            tentPosition.z = Mathf.Round(tentPosition.z / gridSize) * gridSize;

            // Instantiate the tent prefab at the calculated position
            instantiatedTent = Instantiate(tentPrefab, tentPosition, Quaternion.identity);

            // Get the Renderer component of the plane inside the instantiated tent
            planeRenderer = instantiatedTent.GetComponentInChildren<Renderer>();
        }
    }
}
