using System.Collections.Generic;
using UnityEngine;

public class ShortestPath : MonoBehaviour
{
    private GameObject[] nodes;

    [SerializeField] private bool startExploreCount = false;
    [SerializeField] private bool startPathCount = false;

    [SerializeField] private float counter;

    [SerializeField] private float exploreInterval = 0.2f;
    [SerializeField] private float pathInterval = 0.2f;

    [SerializeField] private int exploreCounter;
    [SerializeField] private int resultCounter;

    [SerializeField] private List<Transform> explored;
    [SerializeField] private List<Transform> result;

    [SerializeField] private float weight = 1.0f;

    public enum AlgoType
    {
        Dijkstra = 0,
        AStar = 1
    }

    public AlgoType algorithm;

    public void SetWeight(float value)
    {
        this.weight = value;
    }

   
    public void ResetAnimation()
    {
        this.startExploreCount = false;
        this.startPathCount = false;

        this.explored.Clear();
        this.result.Clear();

        this.counter = 0.0f;

        this.exploreCounter = 0;
        this.resultCounter = 0;
    }

    public void StartExploreCount(bool value)
    {
        this.startExploreCount = value;
    }

    public void StartPathCount(bool value)
    {
        this.startPathCount = value;
    }

    private void Update()
    {
        if (startExploreCount)
        {
            PlayExplorePath();
        }
        if (startPathCount)
        {
            PlayWalkPath();
        }
    }

    private void PlayExplorePath()
    {
        counter += Time.deltaTime;
        if (counter > exploreInterval && exploreCounter < explored.Count)
        {
            counter = 0.0f;

            SpriteRenderer sRend = explored[exploreCounter].GetComponent<SpriteRenderer>();
            sRend.material.color = Color.yellow;

            ++exploreCounter;
        }
        else if (counter > exploreInterval && exploreCounter == explored.Count)
        {
            counter = 0.0f;
            exploreCounter = 0;

            explored.Clear();

            startExploreCount = false;
            startPathCount = true;
        }
    }

    private void PlayWalkPath()
    {
        counter += Time.deltaTime;
        if (counter > pathInterval && resultCounter < result.Count)
        {
            counter = 0.0f;

            SpriteRenderer sRend = result[resultCounter].GetComponent<SpriteRenderer>();
            sRend.material.color = Color.red;

            ++resultCounter;
        }
        else if (counter > pathInterval && resultCounter == result.Count)
        {
            counter = 0.0f;
            resultCounter = 0;

            result.Clear();

            startPathCount = false;
        }
    }

    public List<Transform> FindShortestPath(Transform start, Transform end)
    {
        nodes = GameObject.FindGameObjectsWithTag("Node");

        result = new List<Transform>();
        Transform node = null;

        if (algorithm == AlgoType.Dijkstra)
        {
            node = DijkstrasAlgo(start, end);
        }
        else if (algorithm == AlgoType.AStar)
        {
            node = AStarAlgo(start, end);
        }


        while (node != null)
        {
            result.Add(node);
            Node currentNode = node.GetComponent<Node>();
            node = currentNode.GetParentNode();
        }

        
        result.Reverse();
        return result;
    }

    private Transform DijkstrasAlgo(Transform start, Transform end)
    {
        double startTime = Time.realtimeSinceStartup;

      
        List<Transform> unexplored = new List<Transform>();

        
        foreach (GameObject obj in nodes)
        {
            Node n = obj.GetComponent<Node>();
            if (n.IsWalkable())
            {
                n.ResetNode();
                unexplored.Add(obj.transform);
            }
        }

       
        Node startNode = start.GetComponent<Node>();
        startNode.SetWeight(0);

       

        while (unexplored.Count > 0)
        {
            
            unexplored.Sort((x, y) => x.GetComponent<Node>().GetWeight().CompareTo(y.GetComponent<Node>().GetWeight()));

            
            Transform current = unexplored[0];

           
            if (current == end)
            {
                return end;
            }

            
            unexplored.Remove(current);

            
            explored.Add(current);

            Node currentNode = current.GetComponent<Node>();
            List<Transform> neighbours = currentNode.GetNeighbourNode();
            foreach (Transform neighNode in neighbours)
            {
                counter = 0;
                Node node = neighNode.GetComponent<Node>();

                
                if (unexplored.Contains(neighNode) && node.IsWalkable())
                {

                    float distance = CalculateDistance(neighNode.position, current.position) * weight;
                    distance = currentNode.GetWeight() + distance;

                   
                    if (distance < node.GetWeight())
                    {
                      
                        node.SetWeight(distance);
                        node.SetParentNode(current);
                    }
                }
            }
        }

        double endTime = (Time.realtimeSinceStartup - startTime);
        print("Compute time: " + endTime);

        print("Path completed!");

        return end;
    }

    private Transform AStarAlgo(Transform start, Transform end)
    {
        double startTime = Time.realtimeSinceStartup;

       
        List<Transform> unexplored = new List<Transform>();

       
        foreach (GameObject obj in nodes)
        {
            Node n = obj.GetComponent<Node>();
            if (n.IsWalkable())
            {
                n.ResetNode();
                unexplored.Add(obj.transform);
            }
        }

      
        Node startNode = start.GetComponent<Node>();
        startNode.SetWeight(0);


        while (unexplored.Count > 0)
        {
            
            unexplored.Sort((x, y) => x.GetComponent<Node>().GetWeight().CompareTo(y.GetComponent<Node>().GetWeight()));

           
            Transform current = unexplored[0];

           
            if (current == end)
            {
                return end;
            }

            
            unexplored.Remove(current);

            
            explored.Add(current);

            Node currentNode = current.GetComponent<Node>();
            List<Transform> neighbours = currentNode.GetNeighbourNode();
            foreach (Transform neighNode in neighbours)
            {
                Node node = neighNode.GetComponent<Node>();

                
                if (unexplored.Contains(neighNode) && node.IsWalkable())
                {
                   
                    float distance = CalculateDistance(current.position, neighNode.position);
                    float hCost = CalculateDistance(neighNode.position, end.position) * weight;
                    float gCost = CalculateDistance(neighNode.position, start.position) * weight;
                    float fCost = hCost + gCost;


                    node.SetHCost(hCost);
                    node.SetGCost(gCost);

                    distance = currentNode.GetWeight() + node.GetFCost + distance;

              
                    if (distance < node.GetWeight())
                    {
                      
                        node.SetWeight(hCost);
                        node.SetParentNode(current);
                    }
                }
            }
        }

        double endTime = (Time.realtimeSinceStartup - startTime);
        print("Compute time: " + endTime);

        print("Path completed!");

        return end;
    }

    public float CalculateDistance(Vector2 originNode, Vector2 targetNode)
    {
        GenerateGridManager grid = GetComponent<GenerateGridManager>();
        float h;

        if (!grid.GetDiagonal)
        {
            h = Mathf.Abs(originNode.x - targetNode.x) + Mathf.Abs(originNode.y - targetNode.y);
        }
        else
        {
            h = Mathf.Max(Mathf.Abs(originNode.x - targetNode.x), Mathf.Abs(originNode.y - targetNode.y));
        }
        return h;
    }
}
