using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerController
{
    private const int RIGHT_MOUSE_BUTTON = 1;

    [SerializeField] private CameraController cameraController;

    public SelectionController SelectionController { get; private set; }
    public IActionController ActionController { get; private set; }

    public Player Player { get; private set; }
    public void Initialize(Player player)
    {
        Player = player;
        ActionController = new PlayerActionController();
    }

    private void Awake()
    {
        SelectionController = GetComponentInChildren<SelectionController>();
        ActionController = new PlayerActionController();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(RIGHT_MOUSE_BUTTON) && hasSelection)
        {
            if (cameraController.Hit != null)
            {
                ActionController.AssignUnitActions(SelectionController.Selected, cameraController.Hit.Value);
            }
        }
    }

    private bool hasSelection => SelectionController.Selected.Count > 0;
}