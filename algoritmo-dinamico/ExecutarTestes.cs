using System.Diagnostics;

namespace PrimMST;

//Testes da Programação Dinâmica Bottom-Up (pequena, média e grande instância).
//Os tamanhos são limitados pela natureza exponencial da DP (2^(n-1) estados).
public static class ExecutorTestesDP
{
    private static readonly int[] Sementes = { 42, 43, 44 };
    private const int Repeticoes = 3; // execuções por grafo (usa-se a mediana do tempo)

    //Teste 1 – grafo do relatório (A a E), com tabela, decisões e estatísticas
    public static void TestePequeno()
    {
        Console.WriteLine("=== Teste 1 - Pequena instância (grafo do relatório) ===");

        var g = new Grafo(5);
        g.AdicionarAresta(0, 1, 10); // A-B
        g.AdicionarAresta(0, 2, 20); // A-C
        g.AdicionarAresta(1, 3, 5);  // B-D
        g.AdicionarAresta(1, 2, 30); // B-C
        g.AdicionarAresta(2, 3, 15); // C-D
        g.AdicionarAresta(2, 4, 6);  // C-E
        g.AdicionarAresta(3, 4, 8);  // D-E

        string Nome(int i) => ((char)('A' + i)).ToString();
        int raiz = 0;
        const long custoEsperado = 29; // B-D(5) + C-E(6) + D-E(8) + A-B(10), calculado à mão

        Console.WriteLine($"Entrada: {g.NumVertices} vértices, {g.NumArestas} arestas, raiz = A");

        var res = MstDP.BottomUp(g, raiz, out int[] dp, out sbyte[] ultimo, out EstatisticasDP est);
        MstDP.ImprimirTabela(g, raiz, dp, ultimo, Nome);

        Console.WriteLine($"\nSolução DP Bottom-Up: custo = {res.CustoTotal}");
        Console.WriteLine("Arestas da MST:");
        for (int v = 0; v < g.NumVertices; v++)
            if (res.Pai[v] != -1)
                Console.WriteLine($"   {Nome(res.Pai[v])}-{Nome(v)}");

        long distintos = est.EstadosCalculados; // estados distintos consultados = estados calculados
        Console.WriteLine("\nSubproblemas sobrepostos (reaproveitamento da tabela):");
        Console.WriteLine($"   Estados calculados (cada um uma única vez) : {est.EstadosCalculados}");
        Console.WriteLine($"   Consultas à tabela                         : {est.ConsultasTabela}");
        Console.WriteLine($"   Resultados reaproveitados                  : {est.ConsultasTabela - distintos}");
        Console.WriteLine($"   Chamadas que a recursão simples faria      : {MstDP.ChamadasRecursaoSimples(g.NumVertices):N0}");

        bool ok = res.CustoTotal == custoEsperado && ArvoreValida(g, raiz, res);
        Console.WriteLine($"Custo igual ao esperado ({custoEsperado}) e árvore válida? {(ok ? "OK" : "ERRO")}");
    }

    //Teste 2 – média instância: 15 vértices, 40 arestas (2^14 = 16.384 estados)
    public static void TesteMedio() => TesteAleatorio("Teste 2 - Média instância", 15, 40);

    //Teste 3 – grande instância para a DP: 20 vértices, 60 arestas (2^19 = 524.288 estados)
    public static void TesteGrande() => TesteAleatorio("Teste 3 - Grande instância (limite prático da DP)", 20, 60);

    private static void TesteAleatorio(string titulo, int numVertices, int numArestas)
    {
        Console.WriteLine($"\n=== {titulo} ===");
        Console.WriteLine($"Cada grafo: {numVertices} vértices, {numArestas} arestas, pesos de 1 a 1000; " +
                          $"{Sementes.Length} sementes, {Repeticoes} execuções por grafo (mediana)");
        Console.WriteLine($"Recursão simples faria ~{MstDP.ChamadasRecursaoSimples(numVertices):E2} chamadas; " +
                          $"a tabela tem {1L << (numVertices - 1):N0} estados");

        int ok = 0;
        foreach (int semente in Sementes)
        {
            var rnd = new Random(semente);
            var g = GeradorGrafo.Gerar(numVertices, numArestas, 1000, rnd);
            int raiz = rnd.Next(numVertices);
            if (Executar($"Semente {semente} (raiz {raiz})", g, raiz)) ok++;
        }
        Console.WriteLine($"Resumo: {ok} de {Sementes.Length} grafos com árvore válida");
    }

    //Executa o Bottom-Up várias vezes no mesmo grafo, valida a árvore e mede o tempo
    private static bool Executar(string rotulo, Grafo g, int raiz)
    {
        var tempos = new double[Repeticoes];
        ResultadoMst res = null!;
        EstatisticasDP est = null!;

        for (int i = 0; i < Repeticoes; i++)
        {
            var sw = Stopwatch.StartNew();
            res = MstDP.BottomUp(g, raiz, out _, out _, out est);
            sw.Stop();
            tempos[i] = sw.Elapsed.TotalMilliseconds;
        }

        string lista = string.Join(" / ", tempos.Select(t => t.ToString("F3")));
        Array.Sort(tempos);
        bool ok = ArvoreValida(g, raiz, res);

        Console.WriteLine($"{rotulo}: custo = {res.CustoTotal} -> {(ok ? "OK" : "ERRO")}");
        Console.WriteLine($"   tempos = {lista} ms, mediana = {tempos[Repeticoes / 2]:F3} ms | " +
                          $"estados: {est.EstadosCalculados:N0}, consultas: {est.ConsultasTabela:N0}, " +
                          $"reaproveitados: {est.ConsultasTabela - est.EstadosCalculados:N0}");
        return ok;
    }

    // Mostra o crescimento do tempo da DP (≈ dobra a cada vértice) conforme n aumenta
    public static void TesteEscalabilidade()
    {
        Console.WriteLine("\n=== Escalabilidade da DP Bottom-Up (arestas = 3n, semente 42) ===");
        Console.WriteLine($"{"n",4} | {"Estados 2^(n-1)",16} | {"Tempo (ms)",12} | {"Custo",8}");
        Console.WriteLine(new string('-', 52));

        foreach (int n in new[] { 8, 10, 12, 14, 16, 18, 20 })
        {
            var rnd = new Random(42);
            var g = GeradorGrafo.Gerar(n, 3 * n, 1000, rnd);

            var sw = Stopwatch.StartNew();
            var res = MstDP.BottomUp(g, 0, out _, out _);
            sw.Stop();

            Console.WriteLine($"{n,4} | {1L << (n - 1),16} | {sw.Elapsed.TotalMilliseconds,12:F3} | {res.CustoTotal,8}");
        }
    }

    
    /* Validação estrutural da árvore: n-1 arestas, todos os vértices chegam à raiz seguindo o vetor
    de pais (sem ciclos) e o custo recalculado a partir do grafo é igual ao custo informado.*/
    private static bool ArvoreValida(Grafo g, int raiz, ResultadoMst res)
    {
        int n = g.NumVertices;
        if (res.NumArestas != n - 1 || res.Pai[raiz] != -1) return false;

        long custo = 0;
        for (int v = 0; v < n; v++)
        {
            // segue os pais até a raiz; se passar de n passos, há ciclo
            int atual = v, passos = 0;
            while (atual != raiz)
            {
                atual = res.Pai[atual];
                if (atual == -1 || ++passos > n) return false;
            }

            if (v == raiz) continue;

            // recalcula o peso da aresta (pai[v], v) direto do grafo
            int melhor = int.MaxValue;
            foreach (var (viz, peso) in g.Adj[v])
                if (viz == res.Pai[v] && peso < melhor) melhor = peso;
            if (melhor == int.MaxValue) return false; // aresta inexistente
            custo += melhor;
        }
        return custo == res.CustoTotal;
    }
}