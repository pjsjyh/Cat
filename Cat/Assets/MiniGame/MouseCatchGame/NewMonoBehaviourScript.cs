using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Debug.Log($"Rat Z position: {transform.position.z}");
        Debug.Log($"Camera Z position: {Camera.main.transform.position.z}");

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse clicked!");
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse entered!");
    }
}
