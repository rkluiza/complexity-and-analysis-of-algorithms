namespace PrimMST;

/// <summary>
/// Implementação do algoritmo de Prim, seguindo o pseudocódigo MST-PRIM(G, w, r)
/// de Cormen et al. A fila de prioridade (PriorityQueue) é só a estrutura auxiliar
/// que faz o papel de Q: toda a lógica do algoritmo é escrita aqui.
/// </summary>
public static class Prim
{
    /// <param name="g">Grafo conexo, não direcionado e ponderado.</param>
    /// <param name="r">Vértice raiz.</param>
    /// <param name="log">Se informado, recebe a descrição das decisões tomadas a cada passo.</param>
    /// <param name="nome">Converte o índice do vértice em texto para o log (ex.: 0 vira "A").</param>
    public static ResultadoMst MstPrim(Grafo g, int r, Action<string>? log = null, Func<int, string>? nome = null)
    {
        nome ??= i => i.ToString();
        int n = g.NumVertices;
        int[] chave = new int[n];
        int[] pai = new int[n];
        bool[] naFila = new bool[n];

        // Linhas 1-3: u.chave = infinito e u.pi = NIL para todo vértice
        for (int u = 0; u < n; u++)
        {
            chave[u] = int.MaxValue;
            pai[u] = -1;
        }

        // Linha 4: r.chave = 0
        chave[r] = 0;

        // Linha 5: Q = V[G]
        var q = new PriorityQueue<int, int>();
        for (int u = 0; u < n; u++)
        {
            naFila[u] = true;
            q.Enqueue(u, chave[u]);
        }

        log?.Invoke($"Raiz = {nome(r)} (chave 0). Todos os outros começam com chave infinita.");

        // Linha 6: while Q != vazio
        while (q.Count > 0)
        {
            // Linha 7: u = EXTRACT-MIN(Q)
            q.TryDequeue(out int u, out int chaveNaFila);

            // Entradas antigas (vértice já processado ou chave desatualizada) são ignoradas
            if (!naFila[u] || chaveNaFila != chave[u]) continue;
            naFila[u] = false;

            if (pai[u] == -1)
                log?.Invoke($"Extrai {nome(u)} (chave {chave[u]}): é a raiz.");
            else
                log?.Invoke($"Extrai {nome(u)} (chave {chave[u]}): aresta {nome(pai[u])}-{nome(u)} entra na árvore.");

            // Linha 8: for cada v em G.Adj[u]
            foreach (var (v, peso) in g.Adj[u])
            {
                // Linha 9: if v em Q e w(u,v) < v.chave
                if (naFila[v] && peso < chave[v])
                {
                    log?.Invoke($"   Vizinho {nome(v)}: peso {peso} < chave atual " +
                                $"({(chave[v] == int.MaxValue ? "infinito" : chave[v].ToString())}) -> pai = {nome(u)}, chave = {peso}");
                    pai[v] = u;          // Linha 10
                    chave[v] = peso;     // Linha 11
                    q.Enqueue(v, peso);  // a PriorityQueue não tem DECREASE-KEY: insere de novo
                }
            }
        }

        long custoTotal = 0;
        for (int i = 0; i < n; i++)
            if (pai[i] != -1) custoTotal += chave[i];

        return new ResultadoMst(pai, custoTotal);
    }
}