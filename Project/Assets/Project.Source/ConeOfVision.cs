using UnityEngine;
using UnityEngine.InputSystem;

public class ConeOfVision : MonoBehaviour, MouseInput.IMouseActions
{
    public Camera referenceCamera;

    private MouseInput mouseInput;
    private Vector2 mousePosition;

    private void Awake()
    {
        mouseInput = new MouseInput();
        mouseInput.Mouse.SetCallbacks(this);
    }

    private void OnEnable()
    {
        mouseInput.Enable();
    }

    private void OnDisable()
    {
        mouseInput.Disable();
    }

    private void LateUpdate()
    {
        var ray = referenceCamera.ScreenPointToRay(mousePosition);
        var plane = new Plane(Vector3.back, transform.position);

        if (!plane.Raycast(ray, out var distance))
        {
            return;
        }

        var worldPosition = ray.GetPoint(distance);
        var offset = worldPosition - transform.position;
        
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnPosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }
}