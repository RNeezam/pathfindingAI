using System.Collections.Generic;
using UnityEngine;

public class GenerateGridManager : MonoBehaviour
{

    public int row = 5;
    public int column = 5;
    public float padding = 3f;
    public Transform nodePrefab;
    [SerializeField] private bool enableDiagonal = false;

    public List<Transform> grid = new List<Transform>();

    public static GenerateGridManager instance;

   

    void Start()
    {
        

        GridInit();
    }

    public void SetDiagonal(bool value)
    {
        this.enableDiagonal = value;
        this.GenerateNeighbours();
    }

    public bool GetDiagonal
    {
        get
        {
            return enableDiagonal;
        }
    }

    private void GridInit()
    {
        this.GenerateGrid();
        this.GenerateNeighbours();
    }

    private void GenerateGrid()
    {
        int counter = 0;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
               
                Transform node = Instantiate(nodePrefab, new Vector2(j * padding, i * padding), Quaternion.identity);
                node.parent = transform;
                node.name = "node (" + counter + ")";
                grid.Add(node);
                counter++;
            }
        }
    }

    private void GenerateNeighbours()
    {
        for (int i = 0; i < grid.Count; i++)
        {
            Node currentNode = grid[i].GetComponent<Node>();
            currentNode.GetNeighbourNode().Clear();
            int index = i + 1;

     
            if (index % column == 1)
            {
              
                if (i + column < column * row)
                {
                    currentNode.AddNeighbourNode(grid[i + column]);        
                }
                if (i - column >= 0)
                {
                    currentNode.AddNeighbourNode(grid[i - column]);         
                }

                if (enableDiagonal)
                {
                    if (i + column + 1 < column * row)
                    {
                        currentNode.AddNeighbourNode(grid[i + column + 1]);     
                    }
                    if (i - column + 1 >= 0)
                    {
                        currentNode.AddNeighbourNode(grid[i - column + 1]);     
                    }
                }

                currentNode.AddNeighbourNode(grid[i + 1]);               
            }
            
            else if (index % column == 0)
            {
               
                if (i + column < column * row)
                {
                    currentNode.AddNeighbourNode(grid[i + column]);      
                }
                if (i - column >= 0)
                {
                    currentNode.AddNeighbourNode(grid[i - column]);        
                }

                if (enableDiagonal)
                {
                    if (i + column - 1 < column * row)
                    {
                        currentNode.AddNeighbourNode(grid[i + column - 1]);    
                    }
                    if (i - column - 1 >= 0)
                    {
                        currentNode.AddNeighbourNode(grid[i - column - 1]);     
                    }
                }

                currentNode.AddNeighbourNode(grid[i - 1]);                 
            }
            else
            {
                
                if (i + column < column * row)
                {
                    currentNode.AddNeighbourNode(grid[i + column]);     
                }
                if (i - column >= 0)
                {
                    currentNode.AddNeighbourNode(grid[i - column]);         
                }

                if (enableDiagonal)
                {
                    if (i + column + 1 < column * row)
                    {
                        currentNode.AddNeighbourNode(grid[i + column + 1]);     
                    }
                    if (i + column - 1 < column * row)
                    {
                        currentNode.AddNeighbourNode(grid[i + column - 1]);     
                    }
                    if (i - column + 1 >= 0)
                    {
                        currentNode.AddNeighbourNode(grid[i - column + 1]);     
                    }
                    if (i - column - 1 >= 0)
                    {
                        currentNode.AddNeighbourNode(grid[i - column - 1]);     
                    }
                }

                currentNode.AddNeighbourNode(grid[i + 1]);                  
                currentNode.AddNeighbourNode(grid[i - 1]);                  
            }
        }
    }
}
