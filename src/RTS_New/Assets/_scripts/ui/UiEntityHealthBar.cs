using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiEntityHealthBar : MonoBehaviour
{
    [FormerlySerializedAs("_barGo")] [SerializeField] private Canvas _barCanvas;
    [SerializeField] private Image _healthBar;

    private static Camera _mCamera;
    
    public void Init()
    {
        GetComponent<EntityHealth>().HealthChanged += UpdateHealthBar;
        _barCanvas.enabled = false;
        if (!_mCamera)
            _mCamera = Camera.main;
        _barCanvas.worldCamera = _mCamera;
    }
    
    public void OnMouseEnter()
    {
         _barCanvas.enabled = true;
         _barCanvas.transform.LookAt(_mCamera.transform.position);
    }

    public void OnMouseExit()
    {
        _barCanvas.enabled = false;
    }

    private void UpdateHealthBar(float current, float max)
    {
        _healthBar.fillAmount = current / max;
    }
}
