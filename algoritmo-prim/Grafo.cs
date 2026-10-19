namespace PrimMST;

/// <summary>
/// Grafo não direcionado e ponderado, representado por lista de adjacência.
/// Adj[u] guarda os pares (vizinho v, peso w(u,v)) de cada vértice u.
/// </summary>
public class Grafo
{
    public int NumVertices { get; }
    public int NumArestas { get; private set; }
    public List<(int v, int peso)>[] Adj { get; }

    public Grafo(int numVertices)
    {
        NumVertices = numVertices;
        Adj = new List<(int, int)>[numVertices];
        for (int i = 0; i < numVertices; i++)
            Adj[i] = new List<(int, int)>();
    }

    /// <summary>
    /// Como o grafo é não direcionado, a aresta é inserida nas listas de u e de v.
    /// </summary>
    public void AdicionarAresta(int u, int v, int peso)
    {
        Adj[u].Add((v, peso));
        Adj[v].Add((u, peso));
        NumArestas++;
    }
}