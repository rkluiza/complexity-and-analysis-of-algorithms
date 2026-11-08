namespace PrimMST;

// Resultado do algoritmo: vetor de predecessores (a árvore) e custo total.
public class ResultadoMst
{
    public int[] Pai { get; }
    public long CustoTotal { get; }

    public ResultadoMst(int[] pai, long custoTotal)
    {
        Pai = pai;
        CustoTotal = custoTotal;
    }

    // Número de arestas da árvore: vértices que possuem pai (todos, menos a raiz).
    public int NumArestas => Pai.Count(p => p != -1);
}