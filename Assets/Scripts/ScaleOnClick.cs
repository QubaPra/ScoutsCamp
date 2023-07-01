using System.Collections;
using UnityEngine;

public class ScaleOnClick : MonoBehaviour
{
    public float scaleSpeed = 0.5f;

    private GameObject currentObject;
    private Renderer currentRenderer;
    private float currentScale = 1f;
    private bool isScalingUp = true;
    private bool isScaling = false;
    private Vector3 originalScale; // dodany wektor przechowuj¹cy pierwotn¹ skalê obiektu
    private bool isScalingCoroutineActive = false; // dodana zmienna do œledzenia stanu coroutine

    void Start()
    {
        originalScale = transform.localScale; // zapisz pierwotn¹ skalê obiektu\        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (clickedObject != currentObject && clickedObject.name != "Terrain")
                {
                    currentObject = clickedObject;
                    currentRenderer = currentObject.GetComponent<Renderer>();

                    if (currentRenderer != null)
                    {                        
                        currentScale = 1f;

                        if (!isScalingCoroutineActive) // SprawdŸ, czy coroutine nie jest ju¿ aktywny
                        {
                            StartCoroutine(ScaleObject());
                        }
                    }
                }

                if (clickedObject.name == "Terrain") // sprawdŸ tag obiektu
                {
                    isScaling = false; // zatrzymaj skalowanie
                    if (currentRenderer)
                    {
                        currentRenderer.transform.localScale = originalScale; // przywróæ pierwotn¹ skalê obiektu
                                            
                    }                    
                    currentObject = null;
                    currentRenderer = null;
                }
            }
        }
    }

    IEnumerator ScaleObject()
    {
        isScalingCoroutineActive = true; // Ustaw wartoœæ na true, gdy coroutine jest uruchomiony

        isScaling = true;

        while (isScaling)
        {
            if (isScalingUp)
            {
                currentScale += Time.deltaTime * scaleSpeed;
                if (currentScale >= 1.2f)
                {
                    currentScale = 1.2f;
                    isScalingUp = false;
                }
            }
            else
            {
                currentScale -= Time.deltaTime * scaleSpeed;
                if (currentScale <= 1f)
                {
                    currentScale = 1f;
                    isScalingUp = true;
                }
            }

            currentRenderer.transform.localScale = Vector3.one * currentScale;

            yield return null;
        }

        isScalingCoroutineActive = false; // Ustaw wartoœæ na false, gdy coroutine zostanie zatrzymany
    }
}
