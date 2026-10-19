using System.Diagnostics;

namespace PrimMST;

/// <summary>
/// Executa os três testes pedidos no relatório: pequena, média e grande instância.
/// Nos testes aleatórios, cada tamanho é testado com várias sementes (grafos diferentes),
/// e cada grafo é executado algumas vezes para estabilizar a medição de tempo.
/// </summary>
public static class ExecutorTestes
{
    // Cada semente gera um grafo diferente (outras conexões, outros pesos e outra raiz)
    private static readonly int[] Sementes = { 42, 43, 44 };

    // Quantas vezes o Prim é executado sobre o mesmo grafo (para calcular a mediana do tempo)
    private const int Repeticoes = 3;

    /// <summary>
    /// Teste 1 – Pequena instância: grafo do relatório (A a E), com as decisões passo a passo.
    /// </summary>
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

        Console.WriteLine($"Entrada: {g.NumVertices} vértices, {g.NumArestas} arestas, raiz = A");

        // Execução demonstrativa (com log, fora da medição de tempo, pois imprimir na tela é lento)
        Console.WriteLine("Decisões do algoritmo:");
        var demo = Prim.MstPrim(g, 0, Console.WriteLine, Nome);

        Console.WriteLine("Solução obtida (arestas da MST):");
        for (int v = 0; v < g.NumVertices; v++)
            if (demo.Pai[v] != -1)
                Console.WriteLine($"   {Nome(demo.Pai[v])}-{Nome(v)}");

        ExecutarRepetidas(g, 0, "Grafo do relatório");
    }

    /// <summary>
    /// Teste 2 – Média instância: 1.000 vértices e 5.000 arestas aleatórias.
    /// </summary>
    public static void TesteMedio() => TesteAleatorio("Teste 2 - Média instância", 1_000, 5_000);

    /// <summary>
    /// Teste 3 – Grande instância: 100.000 vértices e 500.000 arestas aleatórias.
    /// </summary>
    public static void TesteGrande() => TesteAleatorio("Teste 3 - Grande instância", 100_000, 500_000);

    /// <summary>
    /// Gera um grafo diferente para cada semente, executa o Prim e confere cada um com o Kruskal.
    /// </summary>
    private static void TesteAleatorio(string titulo, int numVertices, int numArestas)
    {
        Console.WriteLine($"\n=== {titulo} ===");
        Console.WriteLine($"Cada grafo: {numVertices} vértices, {numArestas} arestas, pesos de 1 a 1000; " +
                          $"{Sementes.Length} sementes, {Repeticoes} execuções por grafo");

        int grafosOk = 0;
        foreach (int semente in Sementes)
        {
            var rnd = new Random(semente);
            var g = GeradorGrafo.Gerar(numVertices, numArestas, 1000, rnd);
            int raiz = rnd.Next(numVertices); // raiz escolhida aleatoriamente

            bool ok = ExecutarRepetidas(g, raiz, $"Semente {semente} (raiz {raiz})");
            if (ok) grafosOk++;
        }

        Console.WriteLine($"Resumo: {grafosOk} de {Sementes.Length} grafos conferem com o Kruskal");
    }

    /// <summary>
    /// Executa o Prim várias vezes sobre o mesmo grafo, mede o tempo de cada execução
    /// e confere o resultado (nº de arestas e comparação com o Kruskal). Retorna true se tudo conferiu.
    /// </summary>
    private static bool ExecutarRepetidas(Grafo g, int raiz, string rotulo)
    {
        long custoKruskal = VerificadorKruskal.CustoMst(g); // calculado uma vez por grafo
        var tempos = new double[Repeticoes];
        bool tudoOk = true;
        long custoPrim = 0;

        for (int i = 0; i < Repeticoes; i++)
        {
            var sw = Stopwatch.StartNew();
            var res = Prim.MstPrim(g, raiz);
            sw.Stop();

            tempos[i] = sw.Elapsed.TotalMilliseconds;
            custoPrim = res.CustoTotal;
            tudoOk &= res.NumArestas == g.NumVertices - 1 && res.CustoTotal == custoKruskal;
        }

        string lista = string.Join(" / ", tempos.Select(t => t.ToString("F3")));
        Array.Sort(tempos);

        Console.WriteLine($"{rotulo}: custo Prim = {custoPrim}, custo Kruskal = {custoKruskal}, " +
                          $"tempos = {lista} ms, mediana = {tempos[Repeticoes / 2]:F3} ms -> {(tudoOk ? "OK" : "ERRO")}");
        return tudoOk;
    }
}