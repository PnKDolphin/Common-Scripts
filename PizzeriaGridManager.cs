using UnityEngine;

public class BuildingDesigner : MonoBehaviour
{
    public bool blueprint_mode; 

    public float color_transition_time;
    public Color color_blueprint;
    public Color color_normal;

    public int grid_size_x = 20;
    public int grid_size_z = 20;
    public GameObject cell_prefab;
    public GameObject floor_prefab;
    public GameObject wall_prefab;

    private GameObject[,] grid;
    private bool[,] occupied_cells;
    public bool building_mode = false;

    public Camera cam;
    private void Start()
    {
        grid = new GameObject[grid_size_x, grid_size_z];
        occupied_cells = new bool[grid_size_x, grid_size_z];

        for (int x = 0; x < grid_size_x; x++)
        {
            for (int z = 0; z < grid_size_z; z++)
            {
                GameObject cell = Instantiate(cell_prefab, new Vector3(x, 0, z), Quaternion.identity, transform);
                cell.name = $"Cell_{x}_{z}";
                cell.SetActive(false);
                grid[x, z] = cell;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBuildingMode();
        }

        if (building_mode)
        {
            HandleCellToggling();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SubmitLayout();
        }
    }

    private void ToggleBuildingMode()
    {
        building_mode = !building_mode;

        foreach (GameObject cell in grid)
        {
            if (cell != null)
            {
                cell.SetActive(building_mode);
            }
        }
    }

    private void HandleCellToggling()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 hit_pos = hit.point;
                int x = Mathf.RoundToInt(hit_pos.x);
                int z = Mathf.RoundToInt(hit_pos.z);

                if (x >= 0 && x < grid_size_x && z >= 0 && z < grid_size_z)
                {
                    occupied_cells[x, z] = !occupied_cells[x, z];
                    UpdateCellAppearance(x, z);
                }
            }
        }
    }

    private void UpdateCellAppearance(int x, int z)
    {
        SpriteRenderer renderer = grid[x, z].GetComponent<SpriteRenderer>();
        if (occupied_cells[x, z])
        {
            renderer.color = Color.green; // Change to occupied color
        }
        else
        {
            renderer.color = Color.white; // Change to unoccupied color
        }
    }

    private void SubmitLayout()
    {
        for (int x = 0; x < grid_size_x; x++)
        {
            for (int z = 0; z < grid_size_z; z++)
            {
                if (occupied_cells[x, z])
                {
                    Instantiate(floor_prefab, new Vector3(x, 0, z), Quaternion.identity);
                }
            }
        }

        Debug.Log("Layout submitted.");
    }
//       HandleBlueprintToggle();
    /*void HandleBlueprintToggle()
    {
        if(Input.GetButtonDown("BlueprintMode")) blueprint_mode = !blueprint_mode;
        if(blueprint_mode) cam.backgroundColor = Color.Lerp(cam.backgroundColor, color_blueprint, color_transition_time * Time.deltaTime);
        else cam.backgroundColor = Color.Lerp(cam.backgroundColor, color_normal, color_transition_time * Time.deltaTime);
    }*/
}
