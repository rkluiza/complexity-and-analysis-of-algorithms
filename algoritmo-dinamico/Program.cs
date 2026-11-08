namespace PrimMST;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Aquecimento (JIT) para o primeiro tempo não incluir a compilação
        var aquecimento = GeradorGrafo.Gerar(10, 30, 100, new Random(1));
        Prim.MstPrim(aquecimento, 0);
        MstDP.BottomUp(aquecimento, 0, out _, out _);

        string modo = args.Length > 0 ? args[0] : "tudo";

        if (modo == "dp" || modo == "tudo")
        {
            Console.WriteLine("################ RESULTADOS DA PROGRAMAÇÃO DINÂMICA ################\n");
            ExecutorTestesDP.TestePequeno();
            ExecutorTestesDP.TesteMedio();
            ExecutorTestesDP.TesteGrande();
            ExecutorTestesDP.TesteEscalabilidade();
        }

        if (modo == "comparar" || modo == "tudo")
        {
            Console.WriteLine("\n\n################ COMPARAÇÃO: PRIM (GULOSO) x DP ################\n");
            ComparadorGulosoDP.Executar();
        }
    }
}