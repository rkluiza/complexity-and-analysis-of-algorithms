namespace PrimMST;

/// <summary>
/// Gera grafos aleatórios, conexos, não direcionados e ponderados para os testes.
/// </summary>
public static class GeradorGrafo
{
    /// <summary>
    /// Primeiro constrói uma árvore aleatória que liga todos os vértices (garante a conexidade).
    /// Depois acrescenta arestas aleatórias, sem repetir arestas, até chegar em numArestas.
    /// </summary>
    public static Grafo Gerar(int numVertices, int numArestas, int pesoMax, Random rnd)
    {
        long maxArestas = (long)numVertices * (numVertices - 1) / 2;
        numArestas = (int)Math.Min(Math.Max(numArestas, numVertices - 1), maxArestas);

        var g = new Grafo(numVertices);
        var usadas = new HashSet<long>();

        // 1) Árvore aleatória: cada vértice i liga-se a um vértice anterior qualquer
        for (int i = 1; i < numVertices; i++)
        {
            int j = rnd.Next(i);
            g.AdicionarAresta(i, j, rnd.Next(1, pesoMax + 1));
            usadas.Add(Chave(i, j, numVertices));
        }

        // 2) Arestas extras aleatórias
        while (g.NumArestas < numArestas)
        {
            int u = rnd.Next(numVertices);
            int v = rnd.Next(numVertices);
            if (u == v) continue;
            if (!usadas.Add(Chave(u, v, numVertices))) continue; // aresta já existe
            g.AdicionarAresta(u, v, rnd.Next(1, pesoMax + 1));
        }

        return g;
    }

    // Identifica a aresta {u,v} independentemente da ordem
    private static long Chave(int u, int v, int n)
    {
        int a = Math.Min(u, v), b = Math.Max(u, v);
        return (long)a * n + b;
    }
}