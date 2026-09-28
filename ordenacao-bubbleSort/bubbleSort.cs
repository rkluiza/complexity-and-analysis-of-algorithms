using System;
using System.Diagnostics;

class bubbleSort{
    static int[] numeros = new int[300000];

    static void Main()
    {
        Stopwatch tempoOrdenacao = new Stopwatch();

        // Melhor caso: vetor já ordenado (nenhuma troca)
        numeros = GerarCrescente(numeros.Length);

        tempoOrdenacao.Start();
        OrdenarArray();
        tempoOrdenacao.Stop();
        Console.WriteLine("Melhor caso:" + tempoOrdenacao.Elapsed.TotalMilliseconds);

        numeros = GerarDecrescente(numeros.Length);

        tempoOrdenacao.Restart();
        OrdenarArray();
        tempoOrdenacao.Stop();
        Console.WriteLine("Pior caso:" + tempoOrdenacao.Elapsed.TotalMilliseconds);
    }

    public static void GerarArray()
    {
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

    static int[] GerarDecrescente(int tamanho)
    {
        int[] vetor = new int[tamanho];

        for (int i = 0; i < tamanho; i++)
        {
            vetor[i] = tamanho - i;
        }

        return vetor;
    }

    static void OrdenarArray()
    {
        for (int i = 0; i < numeros.Length - 1; i++){
            for (int j = 0; j < numeros.Length - i - 1; j++){
                if (numeros[j] > numeros[j + 1]){
                    int aux = numeros[j];
                    numeros[j] = numeros[j + 1];
                    numeros[j + 1] = aux;
                }
            }
        }
    }
}