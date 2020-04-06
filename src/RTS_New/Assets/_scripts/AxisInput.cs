using UnityEngine;

public class AxisInput : IInput
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool ReadInput()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        return Mathf.Abs(Horizontal) > Mathf.Epsilon || Mathf.Abs(Vertical) > Mathf.Epsilon;
    }
}