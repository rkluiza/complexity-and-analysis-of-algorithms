using System;
using System.Diagnostics;

public class shellSort
{

    static int[] numeros = new int[300000];
    public static void Main(string[] args)
    {
        Stopwatch tempoOrdenacao = new Stopwatch();

        // Melhor caso: vetor já ordenado (nenhum deslocamento)
        numeros = GerarCrescente(numeros.Length);

        tempoOrdenacao.Start();
        OrdenarArray(numeros, numeros.Length);
        tempoOrdenacao.Stop();

        Console.WriteLine("Melhor caso:" + tempoOrdenacao.Elapsed.TotalMilliseconds);

        // Pior caso: menores nas posições ímpares e maiores nas pares
        numeros = GerarPiorCaso(numeros.Length);

        tempoOrdenacao.Restart();
        OrdenarArray(numeros, numeros.Length);
        tempoOrdenacao.Stop();

        Console.WriteLine("Pior caso:" + tempoOrdenacao.Elapsed.TotalMilliseconds);


    }

    static void GerarArray(){

        Random random = new Random();

        for (int i = 0; i < numeros.Length; i++){
            numeros[i] = random.Next(0, 10001);
        }

    }

    static int[] GerarCrescente(int tamanho)
    {
        int[] vetor = new int[tamanho];

        for (int i = 0; i < tamanho; i++)
        {
            vetor[i] = i + 1;
        }

        return vetor;
    }

    static int[] GerarPiorCaso(int tamanho)
    {
        int[] vetor = new int[tamanho];
        int metade = tamanho / 2;

        // Todos os intervalos > 1 são pares e nunca comparam posições pares com ímpares,
        // então todo o trabalho fica para o intervalo 1 (Θ(n²))
        for (int i = 0; i < metade; i++)
        {
            vetor[2 * i + 1] = i + 1;
            vetor[2 * i] = metade + i + 1;
        }

        return vetor;
    }

    static void OrdenarArray(int[] numeros, int n){

        for (int intervalo = n / 2; intervalo > 0; intervalo /= 2) {
            for (int i = intervalo; i < n; i += 1) {

                int temp = numeros[i];
                int j;

                for (j = i; j >= intervalo && numeros[j - intervalo] > temp; j -= intervalo) {
                    numeros[j] = numeros[j - intervalo];
                }

                numeros[j] = temp;
            }
        }
    }
}