using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerController
{
    private const int RIGHT_MOUSE_BUTTON = 1;

    [SerializeField] private CameraController cameraController;

    public ISelectionController SelectionController { get; private set; }
    public IActionController ActionController { get; private set; }

    public Player Player { get; private set; }
    public void Initialize(Player player)
    {
        Player = player;
        ActionController = new PlayerActionController();
    }

    private void Awake()
    {
        SelectionController = GetComponent<ISelectionController>();
        ActionController = new PlayerActionController();
        Player = new Player(Color.blue, false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(RIGHT_MOUSE_BUTTON) && hasSelection)
        {
            if (cameraController.Hit != null)
            {
                ActionController.AssignUnitActions(SelectionController.Selected, cameraController.Hit.Value);
                Debug.Log(cameraController.Hit.Value.point);
            }
        }
    }

    private bool hasSelection => SelectionController.Selected.Count > 0;
}