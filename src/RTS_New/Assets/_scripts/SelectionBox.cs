using UnityEngine;
using UnityEngine.Serialization;


/// <summary>
/// Draws the selection area for the player
/// </summary>
public class SelectionBox : MonoBehaviour
{
    private const int LEFT_MOUSE_BUTTON = 0;

    [SerializeField] RectTransform _selectionBox;
    [SerializeField] CameraController _cameraController;

    Vector3 _boxStart, _boxEnd;

    private void Start()
    {
        _selectionBox.gameObject.SetActive((false));
    }

    void Update()
    {
        DrawSelectionBox();
    }

    private void DrawSelectionBox()
    {
        if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON))
        {
            if (_cameraController.Hit.HasValue) 
                _boxStart = _cameraController.Hit.Value.point;
        }

        if (Input.GetMouseButtonUp(LEFT_MOUSE_BUTTON))
        {
            _selectionBox.gameObject.SetActive(false);
        }

        if (Input.GetMouseButton(LEFT_MOUSE_BUTTON))
        {
            if (!_selectionBox.gameObject.activeInHierarchy)
            {
                _selectionBox.gameObject.SetActive(true);
            }

            _boxEnd = Input.mousePosition;

            Vector3 rectStart = Camera.main.WorldToScreenPoint(_boxStart);
            Vector3 centre = (rectStart + _boxEnd) / 2;
            rectStart.z = 0;
            float sizeX = Mathf.Abs(rectStart.x - _boxEnd.x);
            float sizeY = Mathf.Abs(rectStart.y - _boxEnd.y);

            _selectionBox.sizeDelta = new Vector2(sizeX, sizeY);
            _selectionBox.position = centre;
        }
    }
}