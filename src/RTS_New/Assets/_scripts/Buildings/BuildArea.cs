using UnityEngine;

public class BuildArea : MonoBehaviour
{
    public GameObject Area;
    public MeshRenderer Renderer;

    /// <summary>
    /// Set size of Area to visualise size of building object.
    /// </summary>
    /// <param name="size">The Size of the object.</param>
    public void SetSize(Vector3 size)
    {
        Area.transform.localScale = size;
        var o = transform.position;
        Area.transform.position = o + new Vector3(0, size.y / 2, 0);
    }

    /// <summary>
    /// Set Color of material to indicate valid placement.
    /// </summary>
    /// <param name="newColor">The indicating Color.</param>
    public void SetColor(Color newColor)
    {
        Renderer.material.color = newColor;
    }

    public void Hide(bool hide)
    {
        gameObject.SetActive(!hide);
    }
}
