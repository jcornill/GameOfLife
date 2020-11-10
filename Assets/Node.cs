public class Node
{
    public int X;
    public int Y;

    public bool Alive;
    public bool NewAlive;

    private Node[] neighbor;

    public Node(int x, int y, bool alive)
    {
        this.X = x;
        this.Y = y;
        this.Alive = alive;
        UpdateDisplay();
    }

    public void SetNeighbor()
    {
        this.neighbor = Map.Singleton.GetAroundNodes(this);
    }

    public void UpdateDisplay()
    {
        if (this.Alive)
        {
            Map.ShowNode(this.X, this.Y);
        }
        else
        {
            Map.HideNode(this.X, this.Y);
        }
    }

    public void Live()
    {
        this.NewAlive = IsAlive();
    }

    public void UpdateMap()
    {
        if (this.Alive != this.NewAlive)
        {
            this.Alive = this.NewAlive;
            UpdateDisplay();
            Map.Singleton.nodeToUpdate.Add(this);
            foreach (Node n in this.neighbor)
            {
                Map.Singleton.nodeToUpdate.Add(n);
            }
        }
    }

    public bool IsAlive()
    {
        int numberAlive = 0;
        for (int i = 0; i < this.neighbor.Length; i++)
        {
            if (this.neighbor[i].Alive)
            {
                numberAlive++;
                if (numberAlive > 3)
                {
                    break;
                }
            }
        }
        if (!this.Alive && numberAlive == 3)
        {
            return true;
        }

        if (this.Alive)
        {
            if (numberAlive == 2 || numberAlive == 3)
            {
                return true;
            }
        }

        return false;
    }
}
