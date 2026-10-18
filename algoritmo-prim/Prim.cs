public static class Prim
{
    // MST-PRIM(G, w, r)
    public static (int[] pai, int custoTotal) MstPrim(Grafo g, int r)
    {
        int n = g.NumVertices;
        int[] chave = new int[n];
        int[] pai = new int[n];
        bool[] naFila = new bool[n];

        // Linhas 1-3: chave = infinito, pai = NIL
        for (int u = 0; u < n; u++)
        {
            chave[u] = int.MaxValue;
            pai[u] = -1;
        }

        // Linha 4: chave da raiz = 0
        chave[r] = 0;

        // Linha 5: Q = V[G]
        // Fila de prioridade com (chave, vértice); ordena pela menor chave
        var q = new PriorityQueue<int, int>();
        for (int u = 0; u < n; u++)
        {
            naFila[u] = true;
            q.Enqueue(u, chave[u]);
        }

        // Linha 6: while Q != vazio
        while (q.Count > 0)
        {
            // Linha 7: u = EXTRACT-MIN(Q)
            q.TryDequeue(out int u, out int chaveNaFila);

            // Entradas obsoletas (já processadas ou com chave antiga) são ignoradas
            if (!naFila[u] || chaveNaFila != chave[u]) continue;
            naFila[u] = false;

            // Linha 8: for cada v em G.Adj[u]
            foreach (var (v, peso) in g.Adj[u])
            {
                // Linha 9: if v em Q e w(u,v) < v.chave
                if (naFila[v] && peso < chave[v])
                {
                    pai[v] = u;          // Linha 10
                    chave[v] = peso;     // Linha 11
                    q.Enqueue(v, peso);  // "DECREASE-KEY" (ver explicação)
                }
            }
        }

        int custoTotal = 0;
        for (int i = 0; i < n; i++)
            if (pai[i] != -1) custoTotal += chave[i];

        return (pai, custoTotal);
    }
}

