using System.Collections.Generic;
using UnityEngine;

public class MinHeap : MonoBehaviour
{
    public class BinaryNode
    {
        Transform node;

        public BinaryNode(Transform node)
        {
            this.node = node;
        }

        public Transform GetNode()
        {
            Transform result = this.node;
            return result;
        }

        public float GetWeight()
        {
            Node n = node.GetComponent<Node>();
            float result = n.GetWeight();
            return result;
        }
    }

    private List<BinaryNode> heap;

 
    public void CreateHeap(Transform node)
    {
     
        heap = new List<BinaryNode>();

    
        heap.Add(new BinaryNode(node));
    }

    public void Insert(Transform node)
    {
        
        BinaryNode bNode = new BinaryNode(node);

      
        heap.Add(bNode);

       
        this.BubbleUp(heap.Count - 1);
    }

    public Transform Extract()
    {
        
        BinaryNode temp = heap[heap.Count - 1];
        heap[heap.Count - 1] = heap[0];
        heap[0] = temp;

       
        Transform result = heap[heap.Count - 1].GetNode();
        heap.RemoveAt(heap.Count - 1);

      
        this.Heapify(0);

     
        return result;
    }

    public bool IsEmpty()
    {
        return heap.Count == 0;
    }

    private void BubbleUp(int index)
    {

        if (index <= 0)
        {
            return;
        }

        int position = index % 2;

        int parent;
      
        if (position == 0)
        {
            parent = Mathf.FloorToInt((index / 2) - 1);
        }

        else
        {
            parent = Mathf.FloorToInt((index / 2));
        }

      
        BinaryNode parentNode = heap[parent];
        BinaryNode node = heap[index];
        if (parentNode.GetWeight() > node.GetWeight())
        {
            BinaryNode temp = heap[index];
            heap[index] = parentNode;
            heap[parent] = temp;

            this.BubbleUp(parent); 

        }
    }

    private void Heapify(int index)
    {
      
        int leftIndex = (2 * index) + 1;
        int rightIndex = (2 * index) + 2;
        int smallest = index;

      
        if (leftIndex <= heap.Count - 1 && heap[leftIndex].GetWeight() <= heap[smallest].GetWeight())
        {
            smallest = leftIndex;
        }

        if (rightIndex <= heap.Count - 1 && heap[rightIndex].GetWeight() <= heap[smallest].GetWeight())
        {
            smallest = rightIndex;
        }

      
        if (smallest != index)
        {
            BinaryNode temp = heap[index];
            heap[index] = heap[smallest];
            heap[smallest] = temp;

            this.Heapify(smallest);
        }
    }

    public void displayHeap()
    {
        print("|||Heap|||");
        int counter = 0;
        foreach (BinaryNode bNode in heap)
        {
            print("index " + counter + " : " + bNode.GetNode().name + " (Weight: " + bNode.GetWeight() + ")");
            counter++;
        }
        print("|||Heap|||");
    }

    public Transform root()
    {
        Transform result = heap[0].GetNode();
        return result;
    }
}