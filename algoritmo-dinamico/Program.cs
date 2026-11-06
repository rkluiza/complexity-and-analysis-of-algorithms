namespace PrimMST;

public class Program
{
    public static void Main()
    {
        // Aquecimento (JIT) para o primeiro tempo não incluir a compilação
        var aquecimento = GeradorGrafo.Gerar(10, 30, 100, new Random(1));
        MstDP.BottomUp(aquecimento, 0, out _, out _);
        MstDP.TopDown(aquecimento, 0, true, out _);

        ExecutorTestesDP.TestePequeno();
        ExecutorTestesDP.TesteMedio();
        ExecutorTestesDP.TesteGrande();
        ExecutorTestesDP.TesteEscalabilidade();
    }
}