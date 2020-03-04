using UnityEngine;

public class BuildingRallyPoint : MonoBehaviour
{
    public void SetRallyPoint(Vector3 pos)
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        transform.position = pos;
    }

    public void RallyPointSet()
    {
        gameObject.SetActive(false);
    }
}
