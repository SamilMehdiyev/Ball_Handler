using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;


public class BallHandler : MonoBehaviour
{
    [SerializeField] private float _detachDelay;
    [SerializeField] private float _respawnDelay;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Rigidbody2D _pivot;



    private Camera _mainCamera;
    private bool _isDragging;
    private Rigidbody2D _ballRigiBody;
    private SpringJoint2D _ballSpringJoint;


    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        this.spawnNewBall();
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

        Invoke(nameof(spawnNewBall), _respawnDelay);
    }

    private void spawnNewBall()
    {
        var ball = Instantiate(_ballPrefab, _pivot.position, Quaternion.identity);

        _ballRigiBody = ball.GetComponent<Rigidbody2D>();

        _ballSpringJoint = ball.GetComponent<SpringJoint2D>();
        _ballSpringJoint.connectedBody = _pivot; 
    }
}
