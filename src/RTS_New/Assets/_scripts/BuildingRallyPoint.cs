using UnityEngine;

public class BuildingRallyPoint : MonoBehaviour
{
    public void SetRallyPoint(Vector3 pos)
    {
        if (gameObject && !gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        transform.position = pos;
    }

    public void RallyPointSet()
    {
        if(gameObject)
            gameObject.SetActive(false);
    }
}
