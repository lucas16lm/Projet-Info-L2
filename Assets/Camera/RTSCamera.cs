using UnityEngine;
using UnityEngine.InputSystem;

public class RTSCamera : MonoBehaviour
{
    public float speed;
    private InputAction inputAction;
    [SerializeField]
    private Vector2 moveInput;
    

    void Start()
    {
         inputAction = InputSystem.actions.FindAction("Movement");
    }

    void Update()
    {
       moveInput = inputAction.ReadValue<Vector2>();
       if(moveInput != Vector2.zero) transform.position+=new Vector3(moveInput.x, 0, moveInput.y)*speed*Time.deltaTime;
    }

}
