using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : SingletonMonoBehaviour<InputHandler>
{
    private InputAction _clickAction;
    private InputAction _moveAction;
    public Vector2 MoveInput => _moveAction.ReadValue<Vector2>();

    private void Awake()
    {
        _clickAction = new InputAction(binding: "<Mouse>/leftButton");
        _moveAction = new InputAction(type: InputActionType.Value);

        _moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");


        _clickAction.performed += OnClick;
    }

    private void OnEnable()
    {
        _clickAction.Enable();
        _moveAction.Enable();
    }

    private void OnDisable()
    {
        _clickAction.Disable();
        _moveAction.Disable();
    }

    private void OnDestroy()
    {
        _clickAction.performed -= OnClick;


        _clickAction.Dispose();
        _moveAction.Dispose();
    }

    private void OnCollisionEnter(Collision other)
    {
        throw new NotImplementedException();
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main!.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        LayerMask clickableMask = 1 << 3;

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, clickableMask)) return;

        Item clickable = hit.collider.GetComponentInParent<Item>();

        if (clickable == null) return;

        EventManager.Fire(new Events.OnItemClicked(clickable));
    }
}