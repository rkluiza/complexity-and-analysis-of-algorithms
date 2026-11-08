using System.Diagnostics;

namespace PrimMST;
public static class ComparadorGulosoDP
{
    private static readonly int[] Sementes = { 42, 43, 44 };
    private static (ResultadoMst res, double medianaMs) Medir(Func<ResultadoMst> acao, int repeticoes)
    {
        var tempos = new double[repeticoes];
        ResultadoMst res = null!;
        for (int i = 0; i < repeticoes; i++)
        {
            var sw = Stopwatch.StartNew();
            res = acao();
            sw.Stop();
            tempos[i] = sw.Elapsed.TotalMilliseconds;
        }
        Array.Sort(tempos);
        return (res, tempos[repeticoes / 2]);
    }

    //Arestas da árvore como lista ordenada de pares (menor-maior), para comparar árvores.
    private static List<(int, int)> Arestas(ResultadoMst r)
    {
        var lista = new List<(int, int)>();
        for (int v = 0; v < r.Pai.Length; v++)
            if (r.Pai[v] != -1)
                lista.Add((Math.Min(v, r.Pai[v]), Math.Max(v, r.Pai[v])));
        lista.Sort();
        return lista;
    }

    private static string Formatar(List<(int, int)> arestas, Func<int, string> nome) =>
        string.Join(", ", arestas.Select(a => $"{nome(a.Item1)}-{nome(a.Item2)}"));

    // ---------- Teste 1: grafo pequeno ----------

    public static void TestePequeno()
    {
        Console.WriteLine("=== Comparação 1 - Pequena instância (grafo do relatório) ===");

        var g = new Grafo(5);
        g.AdicionarAresta(0, 1, 10); // A-B
        g.AdicionarAresta(0, 2, 20); // A-C
        g.AdicionarAresta(1, 3, 5);  // B-D
        g.AdicionarAresta(1, 2, 30); // B-C
        g.AdicionarAresta(2, 3, 15); // C-D
        g.AdicionarAresta(2, 4, 6);  // C-E
        g.AdicionarAresta(3, 4, 8);  // D-E

        string Nome(int i) => ((char)('A' + i)).ToString();

        var (prim, tPrim) = Medir(() => Prim.MstPrim(g, 0), 1000);
        var (dp, tDp) = Medir(() => MstDP.BottomUp(g, 0, out _, out _), 1000);

        var aPrim = Arestas(prim);
        var aDp = Arestas(dp);

        Console.WriteLine($"Prim (guloso): custo = {prim.CustoTotal} | arestas: {Formatar(aPrim, Nome)} | {tPrim:F5} ms");
        Console.WriteLine($"DP Bottom-Up : custo = {dp.CustoTotal} | arestas: {Formatar(aDp, Nome)} | {tDp:F5} ms");
        Console.WriteLine($"Custos iguais? {(prim.CustoTotal == dp.CustoTotal ? "SIM" : "NÃO")} | " +
                          $"Mesma árvore? {(aPrim.SequenceEqual(aDp) ? "SIM" : "NÃO")}");
    }

    // ---------- Testes 2 e 3: grafos aleatórios, mesmos grafos para os dois algoritmos ----------

    public static void TesteMedio() => ComparaAleatorio("Comparação 2 - Média instância", 15, 40);
    public static void TesteGrande() => ComparaAleatorio("Comparação 3 - Grande instância para a DP", 20, 60);

    private static void ComparaAleatorio(string titulo, int n, int m)
    {
        Console.WriteLine($"\n=== {titulo} ===");
        Console.WriteLine($"Cada grafo: {n} vértices, {m} arestas; 3 sementes; tempos = mediana de execuções");

        int iguais = 0;
        foreach (int semente in Sementes)
        {
            // mesma geração dos testes da DP (mesmos grafos e mesmas raízes)
            var rnd = new Random(semente);
            var g = GeradorGrafo.Gerar(n, m, 1000, rnd);
            int raiz = rnd.Next(n);

            var (prim, tPrim) = Medir(() => Prim.MstPrim(g, raiz), 200);
            var (dp, tDp) = Medir(() => MstDP.BottomUp(g, raiz, out _, out _), n >= 18 ? 3 : 15);

            bool mesmoCusto = prim.CustoTotal == dp.CustoTotal;
            bool mesmaArvore = Arestas(prim).SequenceEqual(Arestas(dp));
            if (mesmoCusto) iguais++;

            Console.WriteLine($"Semente {semente} (raiz {raiz}): custo Prim = {prim.CustoTotal}, custo DP = {dp.CustoTotal} " +
                              $"-> {(mesmoCusto ? "IGUAIS" : "DIFERENTES")} (mesma árvore: {(mesmaArvore ? "sim" : "não, empate de pesos")})");
            Console.WriteLine($"   Prim = {tPrim:F5} ms | DP = {tDp:F3} ms | DP é ~{tDp / Math.Max(tPrim, 1e-6):F0}x mais lenta");
        }
        Console.WriteLine($"Resumo: {iguais} de {Sementes.Length} grafos com custo igual nos dois paradigmas");
    }

    // ---------- Instâncias onde só o guloso consegue executar ----------

    public static void TesteSoGuloso()
    {
        Console.WriteLine("\n=== Instâncias grandes: apenas o Prim (a DP é inviável) ===");
        foreach (var (n, m) in new[] { (1_000, 5_000), (100_000, 500_000) })
        {
            var rnd = new Random(42);
            var g = GeradorGrafo.Gerar(n, m, 1000, rnd);
            int raiz = rnd.Next(n);

            var (prim, t) = Medir(() => Prim.MstPrim(g, raiz), 3);
            Console.WriteLine($"n = {n}, m = {m}: Prim custo = {prim.CustoTotal}, {t:F3} ms | " +
                              $"DP precisaria de 2^{n - 1} estados -> inviável");
        }
    }

    // ---------- Escalabilidade lado a lado ----------

    public static void TesteEscalabilidade()
    {
        Console.WriteLine("\n=== Escalabilidade: Prim x DP (arestas = 3n, semente 42, raiz 0) ===");
        Console.WriteLine($"{"n",3} | {"Estados",9} | {"Prim (ms)",10} | {"DP (ms)",10} | {"DP/Prim",9} | {"Mem. DP (KB)",12} | Custos");
        Console.WriteLine(new string('-', 78));

        foreach (int n in new[] { 8, 10, 12, 14, 16, 18, 20 })
        {
            var g = GeradorGrafo.Gerar(n, 3 * n, 1000, new Random(42));

            var (prim, tPrim) = Medir(() => Prim.MstPrim(g, 0), 500);
            var (dp, tDp) = Medir(() => MstDP.BottomUp(g, 0, out _, out _), n >= 18 ? 3 : 15);

            // tabela dp (int, 4 bytes) + tabela de decisões (sbyte, 1 byte) por posição (2^n posições)
            double memKb = 5.0 * (1L << n) / 1024.0;

            Console.WriteLine($"{n,3} | {1L << (n - 1),9} | {tPrim,10:F5} | {tDp,10:F3} | {tDp / Math.Max(tPrim, 1e-6),8:F0}x | " +
                              $"{memKb,12:F0} | {(prim.CustoTotal == dp.CustoTotal ? "iguais" : "DIFERENTES")}");
        }
    }

    public static void Executar()
    {
        TestePequeno();
        TesteMedio();
        TesteGrande();
        TesteSoGuloso();
        TesteEscalabilidade();
    }
}