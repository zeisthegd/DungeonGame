using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    Transform playerBody;
    [SerializeField]
    MovementSettings settings;

    private float xRotation = 0F;
    void Start()
    {
        playerBody = (Transform)GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayerAndCam();
    }

    private void RotatePlayerAndCam()
    {
        Vector2 mouseRot = GetMouseRotation() * settings.mouseSensitivity * Time.deltaTime;

        xRotation -= mouseRot.y;
        xRotation = Mathf.Clamp(xRotation, -90F, 90F);

        settings.camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);//Rotate cam
        playerBody.Rotate(Vector3.up * mouseRot.x);//Rotate player on Y
    }

    private Vector2 GetMouseRotation()
    {
        float horiRot = Input.GetAxis("Mouse X");
        float verRot = Input.GetAxis("Mouse Y");

        return new Vector2(horiRot, verRot);

    }
}
