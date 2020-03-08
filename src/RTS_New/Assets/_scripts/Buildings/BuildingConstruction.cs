using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingConstruction : MonoBehaviour
{
    private const int MODEL_STATES = 3;
    
    private void Start()
    {
        GetComponent<BuildingHealth>().HealthChanged += ConstructionProgress;
        var player = GetComponent<Building>().Player;
        for (int i = 0; i < MODEL_STATES; i++)
        {
            var model = transform.GetChild(i);
            var meshRenderer = model.GetComponent<MeshRenderer>();
            meshRenderer.material = player.EntityMaterial;
            if (i != 0)
                model.gameObject.SetActive(false);
        }
    }

    private void ConstructionProgress(float current, float final)
    {
        var progress = Math.Round(current / final, 1);
        UpdateModel(progress);
    }

    private void UpdateModel(double progress)
    {
        if (progress >= 0.5f && progress < 1f)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (progress == 1f)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            GetComponent<BuildingHealth>().HealthChanged -= ConstructionProgress;
            gameObject.GetComponent<Building>().Queue.enabled = true;
            Destroy(this);
        }
    }
    
}
