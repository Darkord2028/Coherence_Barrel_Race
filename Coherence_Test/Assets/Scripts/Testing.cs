using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;

    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

        transform.forward = Vector3.Slerp(transform.forward, movement, rotationSpeed * Time.deltaTime);
    }
}
