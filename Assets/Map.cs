using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    public int Width;
    public int Height;
    public static Map Singleton;
    private static Tilemap tilemap;
    private static Grid grid;
    private Node[,] nodes;
    private bool[,] startNodes;
    public TileBase Life;
    public TileBase Dead;
    public Text StepText;
    public int step;
    private bool play;
    private bool gridActive;
    public int fillPercent;
    public NodeList<Node> nodeToUpdate;
    public NodeList<Node> nodeToUpdate2;

    private void Awake()
    {
        this.nodeToUpdate = new NodeList<Node>();
        this.nodeToUpdate2 = new NodeList<Node>();
        this.nodes = new Node[this.Width, this.Height];
        this.startNodes = new bool[this.Width, this.Height];
        tilemap = GetComponent<Tilemap>();
        grid = GetComponentInParent<Grid>();
        this.gridActive = false;
        Singleton = this;
        GenerateMap();
        ResetMap();
    }

    public void GenerateMap()
    {
        for (int i = 0; i < this.Width; i++)
        {
            for (int j = 0; j < this.Height; j++)
            {
                bool alive = false;
                if (this.fillPercent != 0)
                {
                    alive = Random.Range(0, 100 / this.fillPercent) == 0;
                }

                tilemap.SetTile(new Vector3Int(i, j, 0), this.Life);
                tilemap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                this.nodes[i, j] = new Node(i, j, alive);
            }
        }
        for (int i = 0; i < this.Width; i++)
        {
            for (int j = 0; j < this.Height; j++)
            {
                this.startNodes[i, j] = this.nodes[i, j].Alive;
            }
        }
        for (int i = 0; i < this.Width; i++)
        {
            for (int j = 0; j < this.Height; j++)
            {
                this.nodes[i, j].SetNeighbor();
            }
        }
    }

    public void ResetMap()
    {
        this.play = false;
        this.step = 0;
        this.StepText.text = "Step: 0";
        for (int i = 0; i < this.Width; i++)
        {
            for (int j = 0; j < this.Height; j++)
            {
                this.nodes[i, j].Alive = this.startNodes[i, j];
                this.nodes[i, j].UpdateDisplay();
                this.nodeToUpdate.Add(this.nodes[i, j]);
            }
        }
    }

    public void StartPlay()
    {
        for (int i = 0; i < this.Width; i++)
        {
            for (int j = 0; j < this.Height; j++)
            {
                this.startNodes[i, j] = this.nodes[i, j].Alive;
            }
        }

        this.play = true;
    }

    private void Update()
    {
        if (this.play == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Node n = PixelToTile();
                if (n != null)
                {
                    n.Alive = !n.Alive;
                    n.UpdateDisplay();
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                MakeStep();
            }

            return;
        }

        MakeStep();
    }

    private void MakeStep()
    {
        NodeList<Node> temp = this.nodeToUpdate2;
        temp.Clear();
        this.nodeToUpdate2 = this.nodeToUpdate;
        this.nodeToUpdate = temp;
        for (int i = 0; i < this.nodeToUpdate2.Count(); i++)
        {
            this.nodeToUpdate2.array[i].Live();
        }
        for (int i = 0; i < this.nodeToUpdate2.Count(); i++)
        {
            this.nodeToUpdate2.array[i].UpdateMap();
        }

        /*
        for (int i = 0; i < this.Width; i++)
        {
            for (int j = 0; j < this.Height; j++)
            {
                this.nodes[i, j].Live();
            }
        }

        for (int i = 0; i < this.Width; i++)
        {
            for (int j = 0; j < this.Height; j++)
            {
                this.nodes[i, j].UpdateMap();
            }
        }
        */
        this.step++;
        this.StepText.text = "Step: " + this.step;
    }

    public Node[] GetAroundNodes(Node n)
    {
        Node[] neighbor = new Node[8];
        int a = 0;
        int b = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                if (IsInMap(n.X + i, n.Y + j))
                {
                    neighbor[a] = this.nodes[n.X + i, n.Y + j];
                    b++;
                }

                a++;
            }
        }
        Node[] result = new Node[b];
        int c = 0;
        for (int i = 0; i < neighbor.Length; i++)
        {
            if (neighbor[i] != null)
            {
                result[c] = neighbor[i];
                c++;
            }
        }
        return result;
    }

    private bool IsInMap(int x, int y)
    {
        if (x < 0 || x >= this.Width)
        {
            return false;
        }

        if (y < 0 || y >= this.Height)
        {
            return false;
        }

        return true;
    }

    public static void ShowNode(int x, int y)
    {
        tilemap.SetColor(new Vector3Int(x, y, 0), Color.white);
    }

    public static void HideNode(int x, int y)
    {
        tilemap.SetColor(new Vector3Int(x, y, 0), Color.black);
    }


    public static Node PixelToTile()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 campos = Camera.main.transform.position;
        Vector2 supposedTilePos = new Vector2(campos.x, campos.y);
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        Vector2 mouseDisplacementToCamera;


        float tempVar1 = 0; //nb of pixel from camera object (x)
        float tempVar2 = 0; //nb of pixel from camera object (y)
        if (mousePosition.x < screenWidth / 2f)
        {
            tempVar1 = -1 * (screenWidth / 2f - mousePosition.x);
        }
        else if (mousePosition.x > screenWidth / 2f)
        {
            tempVar1 = mousePosition.x - screenWidth / 2f;
        }
        else if (mousePosition.x == screenWidth / 2f)
        {
            tempVar1 = 0;
        }

        if (mousePosition.y < screenHeight / 2f)
        {
            tempVar2 = -1 * (screenHeight / 2f - mousePosition.y);
        }
        else if (mousePosition.y > screenHeight / 2f)
        {
            tempVar2 = mousePosition.y - screenHeight / 2f;
        }
        else if (mousePosition.y == screenHeight / 2f)
        {
            tempVar2 = 0;
        }

        mouseDisplacementToCamera.x = tempVar1;
        mouseDisplacementToCamera.y = tempVar2;

        float sizeOfTile = screenHeight / (Camera.main.orthographicSize * 2);

        float horizNbOfTile = screenWidth / sizeOfTile;
        float verticalNbOfTile = screenHeight / sizeOfTile;
        Vector2 tileGainedFromMouse; //can be + or -

        tileGainedFromMouse.x = mouseDisplacementToCamera.x / (screenWidth / 2f) * (horizNbOfTile / 2f);
        tileGainedFromMouse.y = mouseDisplacementToCamera.y / (screenHeight / 2f) * (verticalNbOfTile / 2f);
        supposedTilePos += tileGainedFromMouse;

        supposedTilePos.x = (float) Math.Truncate(supposedTilePos.x);
        supposedTilePos.y = (float) Math.Truncate(supposedTilePos.y);
        if (Singleton.IsInMap((int) supposedTilePos.x, (int) supposedTilePos.y))
        {
            return Singleton.nodes[(int) supposedTilePos.x, (int) supposedTilePos.y];
        }

        return null;
    }
}