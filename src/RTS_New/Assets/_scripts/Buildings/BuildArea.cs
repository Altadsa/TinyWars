using UnityEngine;

public class BuildArea : MonoBehaviour
{
    public GameObject Area;
    public MeshRenderer Renderer;

    private readonly Color _valid = new Color(0.0f,1.0f,0.0f,0.5f);
    private readonly Color _invalid = new Color(1.0f,0.0f,0.0f,0.5f);
    
    /// <summary>
    /// Set size of Area to visualise size of building object.
    /// </summary>
    /// <param name="size">The Size of the object.</param>
    public void SetSize(Vector3 size)
    {
        Area.transform.localScale = size/2;
        var o = transform.position;
        Area.transform.position = o + new Vector3(0, size.y / 4 + 1, 0);
    }

    /// <summary>
    /// Set Color of material to indicate valid placement.
    /// </summary>
    /// <param name="newColor">The indicating Color.</param>
    public void SetColor(bool valid)
    {
        var newColor = valid ? _valid : _invalid;
        Renderer.material.color = newColor;
    }

    public void Hide(bool hide)
    {
        gameObject.SetActive(!hide);
    }
}
