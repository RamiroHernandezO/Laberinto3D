using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pelota : MonoBehaviour
{
    private Rigidbody rig;

    [SerializeField] private float velocidad;
    [SerializeField] private float mouseSensitivity = 500f;
    [SerializeField] private Transform cameraTransform;

    private float xRotation = 0f;
    private Vector3 cameraOffset = new Vector3(0f, 0.55f, 0f);

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();

        // Desvincular la cámara de la pelota y configurar la posición inicial
        if (cameraTransform != null)
        {
            cameraTransform.parent = null;
            cameraTransform.position = transform.position + cameraOffset;
            // Asegurarse de que la cámara comienza mirando hacia adelante
            cameraTransform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        }

        // Bloquear y ocultar el cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Movimiento de la pelota basado en la orientación de la cámara
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rig.AddForce(moveDirection.normalized * velocidad * Time.deltaTime);
        }

        // Control de la cámara con el mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotación horizontal y vertical de la cámara
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localEulerAngles = new Vector3(xRotation, cameraTransform.localEulerAngles.y + mouseX, 0f);
    }

    private void LateUpdate()
    {
        // Asegurarse de que la cámara sigue a la pelota, manteniendo el offset
        if (cameraTransform != null)
        {
            cameraTransform.position = transform.position + cameraOffset;
        }
    }
}
