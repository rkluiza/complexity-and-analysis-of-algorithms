using System.Diagnostics;

namespace PrimMST;

/// <summary>
/// Executa os três testes pedidos no relatório: pequena, média e grande instância.
/// </summary>
public static class ExecutorTestes
{
    // Semente fixa: os mesmos grafos e a mesma raiz são gerados a cada execução (resultado reproduzível)
    private const int Semente = 42;

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
        Console.WriteLine("Decisões do algoritmo:");

        var sw = Stopwatch.StartNew();
        var res = Prim.MstPrim(g, 0, Console.WriteLine, Nome);
        sw.Stop();

        Console.WriteLine("Solução obtida (arestas da MST):");
        for (int v = 0; v < g.NumVertices; v++)
            if (res.Pai[v] != -1)
                Console.WriteLine($"   {Nome(res.Pai[v])}-{Nome(v)}");

        Imprimir(g, res, sw);
    }

    /// <summary>
    /// Teste 2 – Média instância: 1.000 vértices e 5.000 arestas aleatórias.
    /// </summary>
    public static void TesteMedio() => TesteAleatorio("Teste 2 - Média instância", 1_000, 5_000);

    /// <summary>
    /// Teste 3 – Grande instância: 100.000 vértices e 500.000 arestas aleatórias.
    /// </summary>
    public static void TesteGrande() => TesteAleatorio("Teste 3 - Grande instância", 100_000, 500_000);

    private static void TesteAleatorio(string titulo, int numVertices, int numArestas)
    {
        Console.WriteLine($"\n=== {titulo} ===");
        var rnd = new Random(Semente);

        var g = GeradorGrafo.Gerar(numVertices, numArestas, 1000, rnd);
        int raiz = rnd.Next(numVertices); // raiz escolhida aleatoriamente

        Console.WriteLine($"Entrada: {g.NumVertices} vértices, {g.NumArestas} arestas, pesos de 1 a 1000, raiz = {raiz}");

        var sw = Stopwatch.StartNew();
        var res = Prim.MstPrim(g, raiz);
        sw.Stop();

        Imprimir(g, res, sw);
    }

    /// <summary>
    /// Mostra custo, tempo e a conferência da solução (nº de arestas e comparação com o Kruskal).
    /// </summary>
    private static void Imprimir(Grafo g, ResultadoMst res, Stopwatch sw)
    {
        long custoKruskal = VerificadorKruskal.CustoMst(g);
        bool arestasOk = res.NumArestas == g.NumVertices - 1;
        bool custoOk = res.CustoTotal == custoKruskal;

        Console.WriteLine($"Custo total da MST (Prim): {res.CustoTotal}");
        Console.WriteLine($"Tempo de execução do Prim: {sw.Elapsed.TotalMilliseconds:F3} ms");
        Console.WriteLine($"Verificação: arestas = {res.NumArestas} (esperado {g.NumVertices - 1}) -> {(arestasOk ? "OK" : "ERRO")}");
        Console.WriteLine($"Verificação: custo do Kruskal = {custoKruskal} -> {(custoOk ? "OK, custos iguais" : "ERRO, custos diferentes")}");
    }
}