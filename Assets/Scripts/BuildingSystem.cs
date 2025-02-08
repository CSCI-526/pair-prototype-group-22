using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance;   // Singleton for easy access in Node scripts

    [Header("Setup")]
    public GameObject prefabToBuild; // The real prefab to place

    public GameObject option1;
    public GameObject option2;

    public Material PreviewMaterial1;
    public Material PreviewMaterial2;
    
    public Material previewMaterial;         // Material for the preview object
    public float exitDistanceThreshold = 10f;
    public Button startPlacingButton;
    public GameObject player;
    public string targetSceneName = "RacingScene";
    
    [Header("Mouse-Follow Settings")]
    public float previewDistanceFromCamera = 5f;  // How far in front of the camera the preview sits when no node is hovered

    public bool IsPlacing { get; private set; }
    public GameObject PreviewObject { get; private set; }

    // Track the current hovered node
    private Node currentHoveredNode = null;

    private void Awake()
    {
        // Simple singleton pattern
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        player.GetComponent<PlayerSimpleMovement>().enabled = false;
    }

    private void Update()
    {
        // If we’re not placing, or no preview spawned, do nothing.
        if (!IsPlacing || PreviewObject == null)
            return;

        // If we're NOT hovering a node, let the preview follow the mouse in 3D space
        if (currentHoveredNode == null)
        {
            MovePreviewWithMouse();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentHoveredNode.OnMouseClick();
            }
            
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = previewDistanceFromCamera; // distance from camera

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            float dist = Vector3.Distance(worldPos, currentHoveredNode.transform.position);
           // Debug.Log(dist);
            if (dist > exitDistanceThreshold)
            {
                OnNodeHoverExit(currentHoveredNode);
            }
        }
    }

    /// <summary>
    /// Called by a UI button or another script to start building mode.
    /// </summary>
    public void StartPlacing()
    {
        prefabToBuild = option1;
        previewMaterial = PreviewMaterial1;

        // Destroy an old preview if it exists
        if (PreviewObject != null)
            Destroy(PreviewObject);

        // 1) Spawn the preview object
        PreviewObject = Instantiate(prefabToBuild);
        PreviewObject.tag = "Preview";
        ApplyPreviewMaterial(PreviewObject);
        

        // 2) Set build mode active
        IsPlacing = true;
    }
    
    public void StartPlacing2()
    {
        prefabToBuild = option2;
        previewMaterial = PreviewMaterial2;

        // Destroy an old preview if it exists
        if (PreviewObject != null)
            Destroy(PreviewObject);

        // 1) Spawn the preview object
        PreviewObject = Instantiate(prefabToBuild);
        PreviewObject.tag = "Preview";
        ApplyPreviewMaterial(PreviewObject);
        

        // 2) Set build mode active
        IsPlacing = true;
    }
    
    public void FinishPlacing()
    {
        // 2) Set build mode active
        IsPlacing = false;
        startPlacingButton.interactable = false;
        switchScene();
    }

    private void switchScene()
    {
        DontDestroyOnLoad(player);
        player.GetComponent<PlayerSimpleMovement>().enabled = true;
        player.GetComponent<Rigidbody>().isKinematic = false;
        Scene targetScene = SceneManager.GetSceneByName(targetSceneName);
       // Debug.Log("scene");
        if (targetScene.IsValid() || !targetScene.isLoaded)
        {
            SceneManager.LoadScene(targetSceneName, LoadSceneMode.Single);
        }
    }

    /// <summary>
    /// Called by Node.OnMouseDown to finalize placement at a specific node.
    /// </summary>
    public void PlaceObjectAtNode(Node node)
    {
        if (PreviewObject == null || prefabToBuild == null) return;

        // The preview should already be snapped to this node, so use its position/rotation
        Vector3 finalPos = PreviewObject.transform.position;
        Quaternion finalRot = PreviewObject.transform.rotation;
        
        // Debug.Log("plave");
        node.occupy = true;

        // Instantiate the "real" object
        GameObject a = Instantiate(prefabToBuild, finalPos, finalRot);
        
        // set as child of player 
        a.transform.SetParent(player.transform, true);
        
        a.GetComponent<BuildPartController>().OnAttachUpdate();
        
        //Debug.Log(a.name);

        // Destroy the preview and exit build mode (or keep building—your choice)
        Destroy(PreviewObject);
        PreviewObject = null;
        IsPlacing = false;
    }

    /// <summary>
    /// Called from Node.OnMouseEnter when the mouse hovers that node's collider.
    /// </summary>
    public void OnNodeHoverEnter(Node node)
    {
        if (!IsPlacing || PreviewObject == null) return;

        currentHoveredNode = node;

        // Snap preview to the node’s transform
        PreviewObject.transform.position = node.transform.position + node.transform.forward * 0.5f;
        PreviewObject.transform.rotation = node.transform.rotation;

        // (Optionally) if your node or surface has a "normal," and you want to orient:
        // PreviewObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, node.surfaceNormal);
        // Or if you have a child "attachPoint" in the Node, use that instead of node.transform.
    }

    /// <summary>
    /// Called from Node.OnMouseExit when the mouse leaves that node's collider.
    /// </summary>
    public void OnNodeHoverExit(Node node)
    {
        // Only clear if we're exiting the currently hovered node
        if (currentHoveredNode == node)
        {
            currentHoveredNode = null;
        }
    }

    /// <summary>
    /// Makes the entire spawned object use a "preview" material (optional).
    /// </summary>
    private void ApplyPreviewMaterial(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.material = previewMaterial;
        }
    }

    /// <summary>
    /// Moves the preview object in front of the camera where the mouse is, 
    /// without using a direct raycast to the scene.
    /// </summary>
    private void MovePreviewWithMouse()
    {
        // We'll place the preview at a fixed distance in front of the camera 
        // based on the mouse position in screen space.
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = previewDistanceFromCamera; // distance from camera

        Camera cam = Camera.main; // or your chosen camera
        if (cam != null)
        {
            Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
            PreviewObject.transform.position = worldPos;
        }
    }
}
