using UnityEngine;

public class CameraMovement
{
    IInput _input;
    Transform _controller;
    Vector3 InputDirection => (_controller.forward * _input.Vertical) + (_controller.right * _input.Horizontal);

    public CameraMovement(Transform controllerTransform, IInput controlInput)
    {
        _controller = controllerTransform;
        _input = controlInput;
    }

    public bool MoveCamera()
    {
        var input = _input.ReadInput();
        _controller.position += InputDirection;
        return input;
    }
}