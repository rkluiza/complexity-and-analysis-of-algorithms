public class Program
{
    public static void Main()
    {
        // Grafo do relatório: A=0, B=1, C=2, D=3, E=4
        var g = new Grafo(5);
        g.AdicionarAresta(0, 1, 10); // A-B
        g.AdicionarAresta(0, 2, 20); // A-C
        g.AdicionarAresta(1, 3, 5);  // B-D
        g.AdicionarAresta(1, 2, 30); // B-C
        g.AdicionarAresta(2, 3, 15); // C-D
        g.AdicionarAresta(2, 4, 6);  // C-E
        g.AdicionarAresta(3, 4, 8);  // D-E

        var (pai, custo) = Prim.MstPrim(g, 0);
        char Nome(int i) => (char)('A' + i);

        Console.WriteLine("Arestas da MST:");
        for (int v = 0; v < g.NumVertices; v++)
            if (pai[v] != -1)
                Console.WriteLine($"{Nome(pai[v])} - {Nome(v)}");
        Console.WriteLine($"Custo total: {custo}");
    }
}