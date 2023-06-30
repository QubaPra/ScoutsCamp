using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 touchStart;
    public float zoomOutMin = 5;
    public float zoomOutMax = 12;
    public float moveSpeed = 5;
    private float originalYPosition;

    private void Start()
    {
        originalYPosition = transform.position.y; // Store the original y position
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * 0.01f);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction.y = 0; // Ignore the y component of the direction
            Camera.main.transform.position += direction * moveSpeed * Time.deltaTime;
        }

        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
        Vector3 newPosition = Camera.main.transform.position;
        newPosition.y = originalYPosition; // Set the y position to the original value
        Camera.main.transform.position = newPosition;
    }
}
