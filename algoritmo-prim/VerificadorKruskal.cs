namespace PrimMST;

/// <summary>
/// Verificador independente: calcula o custo da MST com o algoritmo de Kruskal
/// (ordenação das arestas + union-find). Serve só para conferir o resultado do Prim.
/// Como a MST pode não ser única, o que se compara é o custo total.
/// </summary>
public static class VerificadorKruskal
{
    public static long CustoMst(Grafo g)
    {
        var arestas = new List<(int u, int v, int peso)>();
        for (int u = 0; u < g.NumVertices; u++)
            foreach (var (v, peso) in g.Adj[u])
                if (u < v) arestas.Add((u, v, peso)); // cada aresta aparece duas vezes: guarda uma

        arestas.Sort((a, b) => a.peso.CompareTo(b.peso));

        int[] pai = new int[g.NumVertices];
        for (int i = 0; i < pai.Length; i++) pai[i] = i;

        int Encontrar(int x)
        {
            while (pai[x] != x)
            {
                pai[x] = pai[pai[x]]; // compressão de caminho
                x = pai[x];
            }
            return x;
        }

        long custo = 0;
        foreach (var (u, v, peso) in arestas)
        {
            int ru = Encontrar(u), rv = Encontrar(v);
            if (ru == rv) continue; // formaria ciclo
            pai[ru] = rv;
            custo += peso;
        }
        return custo;
    }
}