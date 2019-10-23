using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public Transform lookRootTransform;

    public Transform playerTransform;

    [SerializeField]
    private float sensitive = 5f;

    [SerializeField]
    private Vector2 default_Look_limit = new Vector2(-70f, 80f);

    [SerializeField]
    private bool invert = false;

    private Vector2 lookAngle;
    private Vector2 current_Mouse_Look;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        LockAndUnlockCursor();

        if(Cursor.lockState == CursorLockMode.Locked)
        {
            LookAround();
        }
    }

    void LockAndUnlockCursor()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        
    }

    void LookAround()
    {
        current_Mouse_Look = new Vector2(Input.GetAxis(Mouse_Axis.MOUSE_Y), Input.GetAxis(Mouse_Axis.MOUSE_X));
        lookAngle.x += current_Mouse_Look.x * sensitive * (invert ? 1f : -1f);
        lookAngle.y += current_Mouse_Look.y * sensitive;

        lookAngle.x = Mathf.Clamp(lookAngle.x, default_Look_limit.x, default_Look_limit.y);

        lookRootTransform.localRotation = Quaternion.Euler(lookAngle.x, 0f, 0f);
        playerTransform.localRotation = Quaternion.Euler(0f, lookAngle.y, 0f);
        
    }
}
