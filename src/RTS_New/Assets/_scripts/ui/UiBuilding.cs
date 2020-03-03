using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiBuilding : MonoBehaviour
{
    [SerializeField] private PlayerSelectionController _sc;
    [FormerlySerializedAs("_selectionUI")] 
    [SerializeField] private GameObject _buildingInfoGo;

    [SerializeField] private GameObject _buildingMenuGo;
    // Start is called before the first frame update
    [Header("****INFO PANEL****")]
    [Header("Basic Building Info")] 
    public Image Icon;

    public Image Health;

    [Header("Queue Info")] 
    public Image ProgressBar;
    public Button[] QueueButtons;
    
    [Header("****MENU****")]
    public Button[] Menubuttons;
    
    public MenuData DestroyData;
    
    //Keep a reference to the target such that we can remove listeners from its events once the selection changes
    private Building _target;

    private UiBuildingInfo _buildingInfoUi;

    void Start()
    {
        _buildingInfoUi = new UiBuildingInfo(this, _sc, _buildingInfoGo);
        _sc.SelectionUpdated += UpdateMenu;
    }

    private void UpdateMenu(List<Entity> entities)
    {
        if (entities.Count == 0)
        {
            _buildingMenuGo.SetActive(false);
            return; 
        }
        _target = entities[0] as Building;
        if (_target == null)
        {
            _buildingMenuGo.SetActive(false);
            return;
        }
        _buildingMenuGo.SetActive(true);
        for (int i = 0; i < Menubuttons.Length; i++)
        {
            Menubuttons[i].gameObject.SetActive(false);
            if (i == Menubuttons.Length - 1)
            {
                Menubuttons[i].onClick.RemoveAllListeners();
                Menubuttons[i].GetComponent<Image>().sprite = DestroyData.Icon;
                Menubuttons[i].onClick.AddListener(DestroySelectedBuilding);
                Menubuttons[i].gameObject.SetActive(true);
            }
        }

    }
    
    private void DestroySelectedBuilding()
    {    
        Destroy(_target.gameObject);
        _target = null;
    }

    /*
     * We need to get a list of actions from the building that take up to 15 slots
     * Slot 16 is to be reserved for destroying buildings
     */
    
    
    
//    private void SelectionUpdated(List<Entity> entities)
//    {
//        ClearOldTarget();
//        if (entities.Count == 0)
//        {            
//            _selectionUI.SetActive(false);
//            return;
//        }
//        if (entities[0] is Unit)
//        {
//            _selectionUI.SetActive(false);
//            return;
//        }
//        _selectionUI.SetActive(true);
//        _target = entities[0] as Building;
//        SetupTarget();
//    }
//
//    private void ClearOldTarget()
//    {
//        if (!_target) return;
//        ;
//        _target.GetComponent<BuildingHealth>().HealthChanged -= UpdateBuildingHealth;
//        var queue = _target.GetComponent<BuildingQueue>();
//        if (queue)
//        {
//            queue.QueueProcessing -= ProcessQueue;
//            queue.QueueChanged -= UpdateQueue;
//        }
//        _target = null;
//    }
//    
//    private void SetupTarget()
//    {
//        var data = _target.BuildingData;
//        Icon.sprite = data.Icon;
//        _target.GetComponent<BuildingHealth>().HealthChanged += UpdateBuildingHealth;
//        
//        var queue = _target.GetQueue();
//        if (!queue) return;
//        var buildingQueue = queue.Queue;
//        for (int i = 0; i < QueueButtons.Length; i++)
//        {
//            if (i < buildingQueue.Count)
//            {
//                var itemIcon = buildingQueue[i].Icon;
//                var index = i;
//                QueueButtons[i].onClick.RemoveAllListeners();
//                QueueButtons[i].GetComponent<Image>().sprite = itemIcon; 
//                QueueButtons[i].onClick.AddListener(delegate
//                {
//                    queue.RemoveFromQueue(index);
//                });
//                QueueButtons[i].gameObject.SetActive(true);
//            }
//            else
//            {
//                QueueButtons[i].gameObject.SetActive(false);
//            }
//        }
//        queue.QueueChanged += UpdateQueue;
//        queue.QueueProcessing += ProcessQueue;
//    }
//
//    private void UpdateBuildingHealth(float current, float max)
//    {
//        Health.fillAmount = (float)Math.Round(current / max, 2);
//    }
//
//    private void UpdateQueue()
//    {
//        var queue = _target.GetQueue();
//        if (!queue) return;
//        var buildingQueue = queue.Queue;
//        for (int i = 0; i < QueueButtons.Length; i++)
//        {
//            if (i < buildingQueue.Count)
//            {
//                var index = i;
//                var itemIcon = buildingQueue[i].Icon;
//                QueueButtons[i].onClick.RemoveAllListeners();
//                QueueButtons[i].GetComponent<Image>().sprite = itemIcon; 
//                QueueButtons[i].onClick.AddListener(delegate
//                {
//                    queue.RemoveFromQueue(index);
//                });
//                QueueButtons[i].gameObject.SetActive(true);
//            }
//            else
//            {
//                QueueButtons[i].gameObject.SetActive(false);
//            }
//        }
//    }
//    
//    private void ProcessQueue(double progress)
//    {
//        ProgressBar.fillAmount = (float) progress;
//    }
    
}