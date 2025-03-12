using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class RTSCamera : MonoBehaviour
{
    public float speed;
    private InputAction inputAction;
    [SerializeField]
    private Vector2 moveInput;


    void OnEnable()
    {
        inputAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        moveInput = inputAction.ReadValue<Vector2>();
        Vector3 direction = (Camera.main.transform.forward*moveInput.y)+(Camera.main.transform.right*moveInput.x);
        direction.y=0;
        if(moveInput != Vector2.zero) transform.position+=direction.normalized*speed*Time.deltaTime;
    }

}
