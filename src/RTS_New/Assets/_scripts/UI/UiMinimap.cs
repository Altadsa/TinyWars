using UnityEngine;
using UnityEngine.EventSystems;

public class UiMinimap : MonoBehaviour, IPointerDownHandler
{
    private Camera _mCamera;
    public Transform PlayerParent;
    public Camera MiniCam;
    
    private void Awake()
    {
        _mCamera = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Ray mRay = _mCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mRay, out hit))
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }
}
