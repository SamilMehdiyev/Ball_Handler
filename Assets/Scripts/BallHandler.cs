using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;


public class BallHandler : MonoBehaviour
{
    private Camera _mainCamera;
    private bool _isDragging;

    [SerializeField] private Rigidbody2D _ballRigiBody;
    [SerializeField] private SpringJoint2D _ballSpringJoint;
    [SerializeField] private float _detachDelay;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (_ballRigiBody == null) { return; }

        if (!Touchscreen.current.primaryTouch.press.isPressed) {
            if (_isDragging)
            {
                this.launchBall();
            }

            _isDragging = false;

            return;
        }

        _isDragging = true;

        _ballRigiBody.isKinematic = true;

        var touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        var worldPosition = _mainCamera.ScreenToWorldPoint(touchPosition);

        _ballRigiBody.position = worldPosition;
    }


    private void launchBall() 
    {
        _ballRigiBody.isKinematic = false;
        _ballRigiBody = null;

        Invoke(nameof(detachBall), _detachDelay);
    }

    private void detachBall()
    {
        _ballSpringJoint.enabled = false;
        _ballSpringJoint = null;
    }
}
