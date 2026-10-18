public class Grafo
{
    public int NumVertices { get; }
    // Adj[u] = lista de (vizinho v, peso w(u,v))
    public List<(int v, int peso)>[] Adj { get; }

    public Grafo(int numVertices)
    {
        NumVertices = numVertices;
        Adj = new List<(int, int)>[numVertices];
        for (int i = 0; i < numVertices; i++)
            Adj[i] = new List<(int, int)>();
    }

    // Grafo não direcionado: a aresta entra nas duas listas
    public void AdicionarAresta(int u, int v, int peso)
    {
        Adj[u].Add((v, peso));
        Adj[v].Add((u, peso));
    }
}
