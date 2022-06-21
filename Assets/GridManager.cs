using UnityEngine;
using System.Collections;
//using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class GridManager : MonoBehaviour {

    public enum TileType {
        Plain,
        Wall
    }
    TileType[] world = null;
    int n_x;
    int n_z;

    static Vector3[] corners = { new Vector3(1, -0, 0), new Vector3(0, 0, 1) };

    GameObject player;

    Vector3 locateToCenter(Vector3 pos)
    {
        return pos + new Vector3(0.5f, 0, 0.5f);
    }

    public void SetAsWall(Vector3 pos, TileType type = TileType.Wall)
    {
        world[pos2Cell(pos)] = type;
    }

    public void BuildWorld(int n_rows, int n_cols)
    {
        int max_tiles = n_rows * n_cols;

        n_x = n_rows;
        n_z = n_cols;

        if (player == null) player = GameObject.Find("Player");

        // set up the player's position to the center of the grid.
        player.transform.position = locateToCenter(new Vector3(n_x / 2, 0, n_z / 2)); // place the player to the center.
        int player_cell = pos2Cell(player.transform.position);

        // construct a game world and assign walls.
        world = new TileType[max_tiles];
        for (int i = 0; i < max_tiles; i++)
        {
            world[i] = TileType.Plain;
            if (i == player_cell) continue; // we assign the player's location as a plain grid cell.

        }

        for (int i = 0; i < max_tiles; i++) drawRect(i, world[i] == TileType.Wall ? new Color(1f, 1f, 1f, 0f) : new Color(1f, 1f, 1f, 0f));

        transform.position = player.transform.position + new Vector3(0, n_z*Mathf.Sqrt(2), 0); // locate camera properly.
        transform.LookAt(player.transform);
        transform.RotateAround( player.transform.position, new Vector3(-1, 0, 0), 50);
        transform.RotateAround( player.transform.position, new Vector3(0, -1, 0), 52);
    }

    void Start ()
    {
        player = GameObject.Find("Player");
    }
        
    public int pos2Cell(Vector3 pos)
    {
        return ((int)pos.z) * n_x + (int)pos.x;
    }

    public Vector3 cell2Pos(int cellno)
    {
        return new Vector3(cellno % n_x, 0, cellno / n_x);
    }

    public Vector3 pos2center(Vector3 pos)
    {

        return locateToCenter(cell2Pos(pos2Cell(pos)));
    }

    void drawRect(int cellno, Color c, float duration=10000.0f)
    {
        Vector3 correction = new Vector3(0, -0.5f, 0);
        Vector3 lb = cell2Pos(cellno) + correction;
        

        Debug.DrawLine(lb, lb + corners[0], c, duration);
        Debug.DrawLine(lb, lb + corners[1], c, duration);
        Vector3 rt = lb + corners[0] + corners[1] + correction;
        Debug.DrawLine(rt, rt - corners[0], c, duration);
        Debug.DrawLine(rt, rt - corners[1], c, duration);
    }

    int[] findNeighbors(int cellno, TileType[] world)
    {
        List<int> neighbors = new List<int> { -1, 1, -n_x, n_x, -n_x - 1, -n_x + 1, n_x - 1, n_x + 1 };

        if (cellno % n_x == 0)          neighbors.RemoveAll( (no) => no == -1   || no == -1 - n_x || no == -1 + n_x );
        if (cellno % n_x == n_x - 1)    neighbors.RemoveAll( (no) => { return no ==  1   || no ==  1 - n_x || no ==  1 + n_x; } );
        if (cellno / n_x == 0)          neighbors.RemoveAll( (no) => { return no == -n_x || no == -n_x - 1 || no == -n_x + 1; } );
        if (cellno / n_x == n_z - 1)    neighbors.RemoveAll( (no) => { return no ==  n_x || no ==  n_x - 1 || no ==  n_x + 1; } );
        
        for (int i=0; i< neighbors.Count; )
        {
            neighbors[i] += cellno;
            if (neighbors[i] < 0 || neighbors[i] >= n_x * n_z || world[neighbors[i]] == TileType.Wall ) neighbors.RemoveAt(i);
            else i++; /* increase unless removing */
        }

        // remove crossing-neighbors if they are blocked by two adjacent walls. See ppt page 41.
        Vector3 X = cell2Pos(cellno);
        for (int i=0; i < neighbors.Count; )
        {
            Vector3 Xp = cell2Pos(neighbors[i]);
            if ( (X.x - Xp.x) * (X.z - Xp.z) != 0)
            {
                if ( world[ pos2Cell(new Vector3(Xp.x, 0, X.z))] == TileType.Wall 
                    && world[ pos2Cell(new Vector3(X.x, 0, Xp.z))] == TileType.Wall )
                {
                    neighbors.RemoveAt(i);
                    continue;
                }
            }
            i++;
        }
        return neighbors.ToArray();
    }

    int[] buildPath(int[] parents, int from, int to)
    {
        if (parents == null) return null;

        List<int> path = new List<int>();
        int current = to;
        while ( current != from )
        {
            path.Add(current);
            current = parents[current];
        }
        path.Add(from); // to -> ... -> ... -> from

        path.Reverse(); // from -> ... -> ... -> to
        return path.ToArray();
    }

    void drawPath(int[] path)
    {
        if (path == null) return;
        Vector3 correction = new Vector3(0, -0.5f, 0);

        for (int i=0; i< path.Length-1; i++)
        {
            Debug.DrawLine(locateToCenter(cell2Pos(path[i]))+correction , locateToCenter(cell2Pos(path[i + 1]))+correction, new Color(1f, 1f, 1f, 0f), 5.0f);
        }
    }

    class Node
    {
        public int      no;
        public float    f; // final cost = global cost + heuristic cost
        public float    g; // global cost
    }

    public enum MethodType {  BFS, AStar, DFS };

    float computeDistance(int cell1, int cell2)
    {
        return Vector3.Distance(cell2Pos(cell1), cell2Pos(cell2));
    }

    public MethodType method = MethodType.AStar;

   
    int[] findAstarPath(int from, int to, TileType[] world)
    {
        print("A*");
        int max_tiles = n_x * n_z;

        if (from < 0 || from >= max_tiles || to < 0 || to >= max_tiles) return null;

        // initialize the parents of all tiles to negative value, implying no tile number associated.
        int[] parents = new int[max_tiles];       

        for (int i = 0; i < parents.Length; i++) parents[i] = -1;

        List<Node> closed = new List<Node>();
        List<Node> open = new List<Node>() { new Node { no = from, f = 0f, g = 0f}  };
        int nIterations = 0;
        while (open.Count > 0) 
        {
            var lowScore = open.Min(node => node.f);
            var current = open.First(n => n.f == lowScore);
            open.Remove(current);
            closed.Add(current);
            int[] neighbors = findNeighbors(current.no, world);            
            nIterations++;
            foreach (var neighbor in neighbors)
            {
                if (neighbor == to) 
                { 
                    // found the destination
                    parents[neighbor] = current.no;
                    return buildPath(parents, from, to); // read parents array and construct the shoretest path by traversal
                }

                if (closed.FirstOrDefault(n => n.no == neighbor) != null) continue;

                // computeDistance(current.no, neighbor) : global cost 계산시 인접 노드간 거리를 고려. 사선인 경우, sqrt(2)가 반환
                var g = current.g + computeDistance(current.no, neighbor);
                var h = computeDistance(neighbor, to); // computeDistance(neighbor, to) : heuristic cost 계산
                var nodeInOpen = open.FirstOrDefault(n => n.no == neighbor);
                if (nodeInOpen == null)
                {
                    parents[neighbor] = current.no;
                    
                    open.Insert(0, new Node { no = neighbor, f = g + h, g = g });
                    continue;
                }
                if ( g + h < nodeInOpen.f)
                {
                    nodeInOpen.f = g + h;
                    nodeInOpen.g = g;
                    parents[neighbor] = current.no;
                }
            }
        }
        return null; // I cannot find any path from source to destination        
    }

    Coroutine move_coroutine = null;

    public void Move(GameObject player, Vector3 destination, bool immediate = false)
    {
        if (immediate == true)
        {
            player.transform.position = destination;
            return;
        }
        if (move_coroutine != null)
        {
            StopCoroutine(move_coroutine);
            move_coroutine = null;
        }
        move_coroutine = StartCoroutine(move(player, destination));
    }

    private IEnumerator move(GameObject player, Vector3 destination)
    {
        int start = pos2Cell(player.transform.position);
        int end = pos2Cell(destination);
        int[] path = null;

            path = findAstarPath(start, end, world);

        if (path == null) yield break;

        // path should start from "source" to "destination".
                
        drawPath(path);
        List<int> remaining = new List<int>(path); // convert int array to List
        remaining.RemoveAt(0); // we don't need the first one, since the first element should be same as that of source.
        while (remaining.Count > 0)
        {   
            int to = remaining[0]; remaining.RemoveAt(0);
    
            Vector3 toLoc = locateToCenter(cell2Pos(to));
            while (player.transform.position != toLoc)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, toLoc, 3f * Time.deltaTime);
                
                drawRect(pos2Cell(player.transform.position), new Color(1f, 1f, 1f, 0f), Time.deltaTime);
                yield return null;
            }
        }    
    }
}
