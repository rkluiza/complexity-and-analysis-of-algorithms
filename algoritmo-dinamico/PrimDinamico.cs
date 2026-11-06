namespace PrimMST;

/// <summary>Contadores da versão Top-Down, usados na análise e na visualização.</summary>
public class EstatisticasTopDown
{
    public long Chamadas;          // total de chamadas recursivas
    public long AcertosCache;      // vezes em que o cache evitou recálculo
    public long EstadosCalculados; // subconjuntos efetivamente calculados (armazenados)
}

/// <summary>
/// Árvore Geradora Mínima por Programação Dinâmica sobre subconjuntos de vértices.
///   Estado:      dp[S] = custo mínimo de uma árvore que conecta exatamente os vértices de S (raiz em S)
///   Decisão:     qual vértice v de S (v != raiz) foi a última folha adicionada
///   Recorrência: dp[S] = min_v ( dp[S \ {v}] + menor peso entre v e algum vértice de S \ {v} )
///   Caso base:   dp[{raiz}] = 0
///   Resultado:   dp[todos os vértices]
/// Exponencial no número de vértices: viável apenas para grafos pequenos (n ≤ ~22).
/// </summary>
public static class MstDP
{
    public const int Inf = 1_000_000_000;
    public const int MaxVertices = 24;

    // ======================= BOTTOM-UP (tabulação) =======================
    // Casos básicos -> preenchimento da tabela -> combinação -> solução final
    public static ResultadoMst BottomUp(Grafo g, int r, out int[] dp, out sbyte[] ultimo)
    {
        int n = g.NumVertices;
        ValidarTamanho(n);

        int[,] w = MatrizPesos(g);
        int bitRaiz = 1 << r;
        int cheio = (1 << n) - 1;

        dp = new int[1 << n];
        ultimo = new sbyte[1 << n];   // ultimo[S] = vértice removido na melhor decisão de S
        Array.Fill(dp, Inf);
        Array.Fill(ultimo, (sbyte)-1);

        dp[bitRaiz] = 0;              // caso base

        // Ordem crescente das máscaras garante que S\{v} (menor) já foi resolvido
        for (int mask = bitRaiz + 1; mask <= cheio; mask++)
        {
            if ((mask & bitRaiz) == 0) continue;   // só interessam conjuntos que contêm a raiz

            int melhor = Inf;
            sbyte melhorV = -1;

            for (int v = 0; v < n; v++)
            {
                if (v == r || (mask & (1 << v)) == 0) continue;

                int anterior = mask ^ (1 << v);            // subproblema menor: S \ {v}
                if (dp[anterior] >= Inf) continue;         // subconjunto desconexo

                int pesoLigacao = MenorLigacao(w, n, anterior, v, out _);
                if (pesoLigacao >= Inf) continue;          // v não se liga a S\{v}

                int candidato = dp[anterior] + pesoLigacao;
                if (candidato < melhor)
                {
                    melhor = candidato;
                    melhorV = (sbyte)v;
                }
            }

            dp[mask] = melhor;
            ultimo[mask] = melhorV;
        }

        return Reconstruir(w, n, r, cheio, dp[cheio], ultimo);
    }

    // ======================= TOP-DOWN (memoização) =======================
    // Problema principal -> chamadas recursivas -> consulta ao cache -> cálculo -> armazenamento -> retorno
    // Com usarCache = false vira a recursão simples (só para comparar o nº de chamadas, use n pequeno).
    public static ResultadoMst TopDown(Grafo g, int r, bool usarCache, out EstatisticasTopDown est)
    {
        int n = g.NumVertices;
        ValidarTamanho(n);

        var ctx = new Contexto
        {
            N = n,
            R = r,
            W = MatrizPesos(g),
            UsarCache = usarCache,
            Memo = new int[1 << n],
            Ultimo = new sbyte[1 << n],
            Est = new EstatisticasTopDown()
        };
        Array.Fill(ctx.Memo, -1);   // -1 = ainda não calculado
        Array.Fill(ctx.Ultimo, (sbyte)-1);

        int cheio = (1 << n) - 1;
        int custo = Resolver(ctx, cheio);

        est = ctx.Est;
        return Reconstruir(ctx.W, n, r, cheio, custo, ctx.Ultimo);
    }

    private class Contexto
    {
        public int N, R;
        public int[,] W = null!;
        public bool UsarCache;
        public int[] Memo = null!;
        public sbyte[] Ultimo = null!;
        public EstatisticasTopDown Est = null!;
    }

    private static int Resolver(Contexto c, int mask)
    {
        c.Est.Chamadas++;

        if (mask == (1 << c.R)) return 0;                 // caso base: só a raiz

        if (c.UsarCache && c.Memo[mask] != -1)            // consulta ao cache
        {
            c.Est.AcertosCache++;
            return c.Memo[mask];
        }

        int melhor = Inf;
        for (int v = 0; v < c.N; v++)
        {
            if (v == c.R || (mask & (1 << v)) == 0) continue;

            int anterior = mask ^ (1 << v);
            int sub = Resolver(c, anterior);              // chamada recursiva (subproblema menor)
            if (sub >= Inf) continue;

            int pesoLigacao = MenorLigacao(c.W, c.N, anterior, v, out _);
            if (pesoLigacao >= Inf) continue;

            if (sub + pesoLigacao < melhor)
            {
                melhor = sub + pesoLigacao;
                c.Ultimo[mask] = (sbyte)v;
            }
        }

        if (c.UsarCache)
        {
            c.Memo[mask] = melhor;                        // armazenamento
            c.Est.EstadosCalculados++;
        }
        return melhor;                                    // retorno
    }

    // ======================= AUXILIARES =======================

    /// <summary>Menor peso de aresta ligando v a algum vértice do conjunto. Retorna Inf se não houver.</summary>
    private static int MenorLigacao(int[,] w, int n, int conjunto, int v, out int origem)
    {
        int melhor = Inf;
        origem = -1;
        for (int u = 0; u < n; u++)
        {
            if ((conjunto & (1 << u)) == 0) continue;
            if (w[u, v] < melhor)
            {
                melhor = w[u, v];
                origem = u;
            }
        }
        return melhor;
    }

    /// <summary>Converte a lista de adjacência em matriz de pesos (Inf = sem aresta).</summary>
    private static int[,] MatrizPesos(Grafo g)
    {
        int n = g.NumVertices;
        var w = new int[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                w[i, j] = Inf;

        for (int u = 0; u < n; u++)
            foreach (var (v, peso) in g.Adj[u])
                if (peso < w[u, v]) w[u, v] = peso;
        return w;
    }

    /// <summary>
    /// Volta pela tabela de decisões: de dp[V] até dp[{raiz}], recuperando a aresta de cada folha removida.
    /// </summary>
    private static ResultadoMst Reconstruir(int[,] w, int n, int r, int cheio, int custoDp, sbyte[] ultimo)
    {
        if (custoDp >= Inf)
            throw new InvalidOperationException("O grafo é desconexo: não existe árvore geradora.");

        int[] pai = new int[n];
        Array.Fill(pai, -1);

        int mask = cheio;
        long custo = 0;
        int bitRaiz = 1 << r;

        while (mask != bitRaiz)
        {
            int v = ultimo[mask];
            int anterior = mask ^ (1 << v);
            int peso = MenorLigacao(w, n, anterior, v, out int u);
            pai[v] = u;
            custo += peso;
            mask = anterior;
        }
        return new ResultadoMst(pai, custo);
    }

    private static void ValidarTamanho(int n)
    {
        if (n > MaxVertices)
            throw new ArgumentException($"DP por subconjuntos é exponencial: use no máximo {MaxVertices} vértices.");
    }

    // ======================= VISUALIZAÇÃO (Etapa 6) =======================

    /// <summary>
    /// Imprime a tabela dp para todos os subconjuntos que contêm a raiz, mostrando
    /// qual subproblema menor foi reutilizado em cada decisão.
    /// </summary>
    public static void ImprimirTabela(Grafo g, int r, int[] dp, sbyte[] ultimo, Func<int, string> nome)
    {
        int n = g.NumVertices;
        int[,] w = MatrizPesos(g);
        int bitRaiz = 1 << r;

        string Conjunto(int mask)
        {
            var itens = new List<string>();
            for (int i = 0; i < n; i++)
                if ((mask & (1 << i)) != 0) itens.Add(nome(i));
            return "{" + string.Join(",", itens) + "}";
        }

        Console.WriteLine("\nTabela de Programação Dinâmica (subconjuntos que contêm a raiz):");
        Console.WriteLine($"{"S",-16}| {"dp[S]",-6}| Recorrência usada (dp[S\\{{v}}] + w)");
        Console.WriteLine(new string('-', 70));

        for (int mask = bitRaiz; mask < (1 << n); mask++)
        {
            if ((mask & bitRaiz) == 0) continue;

            string valor = dp[mask] >= Inf ? "∞" : dp[mask].ToString();
            string origem;

            if (mask == bitRaiz)
            {
                origem = "caso base";
            }
            else if (ultimo[mask] < 0)
            {
                origem = "subconjunto desconexo";
            }
            else
            {
                int v = ultimo[mask];
                int anterior = mask ^ (1 << v);
                int peso = MenorLigacao(w, n, anterior, v, out int u);
                origem = $"dp[{Conjunto(anterior)}] + w({nome(u)}-{nome(v)}) = {dp[anterior]} + {peso}";
            }

            Console.WriteLine($"{Conjunto(mask),-16}| {valor,-6}| {origem}");
        }
    }
}