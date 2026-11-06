namespace PrimMST;

/// <summary>
/// Resultado do algoritmo: vetor de predecessores (a árvore) e custo total.
/// </summary>
public class ResultadoMst
{
    public int[] Pai { get; }
    public long CustoTotal { get; }

    public ResultadoMst(int[] pai, long custoTotal)
    {
        Pai = pai;
        CustoTotal = custoTotal;
    }

    /// <summary>
    /// Número de arestas da árvore: vértices que possuem pai (todos, menos a raiz).
    /// </summary>
    public int NumArestas => Pai.Count(p => p != -1);
}