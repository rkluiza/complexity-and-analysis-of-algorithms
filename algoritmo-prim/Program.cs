namespace PrimMST;

public class Program
{
    public static void Main()
    {
        // Aquecimento (JIT) para o tempo do primeiro teste não incluir a compilação do código
        Prim.MstPrim(GeradorGrafo.Gerar(100, 300, 100, new Random(1)), 0);

        ExecutorTestes.TestePequeno();
        ExecutorTestes.TesteMedio();
        ExecutorTestes.TesteGrande();
    }
}