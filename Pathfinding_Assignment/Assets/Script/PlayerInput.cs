using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Transform node;
    [SerializeField] private Transform startNode;
    [SerializeField] private Transform endNode;
    [SerializeField] private List<Transform> blockPath = new List<Transform>();
    [SerializeField] private List<Transform> selectedNode = new List<Transform>();

    [SerializeField] private Transform selection;

    private void Start()
    {
    
    }

    void Update()
    {
        MouseInput();
    
        this.ColorBlockPath();
        this.UpdateNodeColor();
    }

    private void MouseInput()
    {
        if (!UIHoverListener.isUIOverride)
        {
            if (Input.GetButton("Fire1"))
            {
             
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Node")
                {
                    
                    node = hit.transform;
                    Node n = node.GetComponent<Node>();
                    Transform nt = node.GetComponent<Transform>();

                    if (node != null)
                    {
                    
                        if (!n.IsSelected())
                        {
                            

                            Transform tempSelect = Instantiate(selection, node.transform.position, Quaternion.identity);
                            tempSelect.parent = n.transform;
                            selectedNode.Add(nt);
                            n.SetSelected(true);
                        }
                    }

                    
                }
            }
            else if (Input.GetButton("Fire2"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Node")
                {
                    
                    node = hit.transform;
                    Node n = node.GetComponent<Node>();
                    Transform nt = node.GetComponent<Transform>();

                    if (node != null)
                    {
                       
                        if (n.IsSelected())
                        {
                            SelectionComponent tempSelect = n.GetComponentInChildren<SelectionComponent>();
                            Destroy(tempSelect.gameObject);
                            selectedNode.Remove(nt);
                            n.SetSelected(false);
                        }
                        node = null;
                    }
                }
            }
        }
    }

    public void BtnStartNode()
    {
        
        foreach (Transform sn in selectedNode)
        {
            Node n = sn.GetComponent<Node>();
            if (!selectedNode.Contains(startNode))
            {
                if (n.IsWalkable() && sn != endNode && sn != startNode)
                {
                    if (startNode == null)
                    {
                        SpriteRenderer sRend = sn.GetComponent<SpriteRenderer>();
                        sRend.material.color = Color.blue;
                    }
                    else
                    {
                       
                        SpriteRenderer sRend = startNode.GetComponent<SpriteRenderer>();
                        sRend.material.color = Color.white;

                      
                        sRend = sn.GetComponent<SpriteRenderer>();
                        sRend.material.color = Color.blue;
                    }

                    startNode = sn;
                }
            }
            SelectionComponent tempSelect = sn.GetComponentInChildren<SelectionComponent>();
            Destroy(tempSelect.gameObject);

            n.SetSelected(false);
        }

        node = null;

        selectedNode.Clear();
    }

    public void QuitExe()
    {
        Application.Quit();
    }

    public void BtnEndNode()
    {
        foreach (Transform sn in selectedNode)
        {
            Node n = sn.GetComponent<Node>();
            if (!selectedNode.Contains(endNode))
            {
                if (n.IsWalkable() && sn != startNode && sn != endNode)
                {
                    
                    if (endNode == null)
                    {
                        SpriteRenderer sRend = sn.GetComponent<SpriteRenderer>();
                        sRend.material.color = Color.cyan;
                    }
                    else
                    {
                        
                        SpriteRenderer sRend = endNode.GetComponent<SpriteRenderer>();
                        sRend.material.color = Color.white;

                  
                        sRend = sn.GetComponent<SpriteRenderer>();
                        sRend.material.color = Color.cyan;
                    }

                    endNode = sn;
                }
            }
            SelectionComponent tempSelect = sn.GetComponentInChildren<SelectionComponent>();
            Destroy(tempSelect.gameObject);

            n.SetSelected(false);
        }

        node = null;

        selectedNode.Clear();
    }

    public void BtnFindPath()
    {
       
        if (startNode != null && endNode != null)
        {
         
            ShortestPath finder = gameObject.GetComponent<ShortestPath>();

            finder.ResetAnimation();

        
            GenerateGridManager gridManager = gameObject.GetComponent<GenerateGridManager>();
            for (int i = 0; i < gridManager.grid.Count; ++i)
            {
                Node currentNode = gridManager.grid[i].GetComponent<Node>();
                SpriteRenderer sRend = currentNode.GetComponent<SpriteRenderer>();

                if (currentNode != startNode || currentNode != endNode || currentNode.IsWalkable())
                {
                    currentNode.SetWeight(int.MaxValue);
                    sRend.material.color = Color.white;
                }
            }

            finder.FindShortestPath(startNode, endNode);

            finder.StartExploreCount(true);

       
        }
    }

    public void BtnBlockPath()
    {
       
        foreach (Transform sn in selectedNode)
        {
            Node n = sn.GetComponent<Node>();

            if (!blockPath.Contains(sn))
            {
                blockPath.Add(sn);
            }

            if (sn == startNode)
            {
                startNode = null;
            }
            if (sn == endNode)
            {
                endNode = null;
            }

            SpriteRenderer sRend = sn.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.black;

            if (sn != startNode && sn != endNode)
            {
                n.SetWalkable(false);
            }

            SelectionComponent tempSelect = sn.GetComponentInChildren<SelectionComponent>();
            Destroy(tempSelect.gameObject);

            n.SetSelected(false);
        }

        node = null;

        selectedNode.Clear();
    }

    public void BtnUnblockPath()
    {
        foreach (Transform sn in selectedNode)
        {
            Node n = sn.GetComponent<Node>();

            SpriteRenderer sRend = sn.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.white;

            n.SetWalkable(true);

            if (blockPath.Contains(sn))
            {
                blockPath.Remove(sn);
            }

            if (node == startNode)
            {
                startNode = null;
            }

            if (node == endNode)
            {
                endNode = null;
            }

            SelectionComponent tempSelect = sn.GetComponentInChildren<SelectionComponent>();
            Destroy(tempSelect.gameObject);

            n.SetSelected(false);
        }

        node = null;

        selectedNode.Clear();
    }

    public void BtnClearBlock()
    {
      
        foreach (Transform path in blockPath)
        {
         
            Node n = path.GetComponent<Node>();
            n.SetWalkable(true);

          
            SpriteRenderer sRend = path.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.white;

        }
      
        blockPath.Clear();
    }

    public void BtnRestart()
    {
        Time.timeScale = 1;

        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    public void BtnWeight(InputField _value)
    {
        float result = Mathf.Abs(float.Parse(_value.text));

        ShortestPath finder = GetComponent<ShortestPath>();
        finder.SetWeight(result);
    }

    public void BtnDiagonal(Toggle value)
    {
        GenerateGridManager generate = gameObject.GetComponent<GenerateGridManager>();

        generate.SetDiagonal(value.isOn);
    }

    public void BtnPathSpeed(Slider _value)
    {
        Time.timeScale = _value.value;
    }

    public void AlgoType(Dropdown _value)
    {
        ShortestPath path = gameObject.GetComponent<ShortestPath>();

        path.algorithm = (ShortestPath.AlgoType)_value.value;
    }

    private void ColorBlockPath()
    {
        foreach (Transform block in blockPath)
        {
            SpriteRenderer sRend = block.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.black;
        }
    }

    private void UpdateNodeColor()
    {
        if (startNode != null)
        {
            SpriteRenderer sRend = startNode.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.blue;
        }

        if (endNode != null)
        {
            SpriteRenderer sRend = endNode.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.cyan;
        }
    }
}
