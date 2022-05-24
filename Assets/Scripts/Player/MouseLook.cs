using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Tooltip("Camera to rotate")]
    [SerializeField] Camera _camera;
    [Tooltip("Object to rotate with camera")]
    [SerializeField] Transform _playerTransform;

    [SerializeField] float mouseSensitivity = 100f;
    //[SerializeField] bool _lockCursor = true;

    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        _camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);


        _playerTransform.Rotate(Vector3.up * mouseX);
    }
}
