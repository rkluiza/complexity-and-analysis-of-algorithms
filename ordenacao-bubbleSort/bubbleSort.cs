using System;
using System.Diagnostics;

class bubbleSort{
    static int[] numeros = new int[300000];

    static void Main()
    {
        GerarVetorInverso(numeros.Length);

        Stopwatch tempoOrdenacao = new Stopwatch();

        tempoOrdenacao.Start();
        OrdenarArray();
        tempoOrdenacao.Stop();
        Console.WriteLine("Ordenar:" + tempoOrdenacao.Elapsed.TotalMilliseconds);

    }

    public static void GerarArray()
    {
        Random random = new Random();

        for (int i = 0; i < numeros.Length; i++){
            numeros[i] = random.Next(0, 10001);
        }
    }

    static int[] GerarVetorInverso(int tamanho)
    {
        Random random = new Random();

        int[] vetor = new int[tamanho];

        // Gera números aleatórios de 0 a 10.000
        for (int i = 0; i < tamanho; i++)
        {
            vetor[i] = random.Next(0, 10001);
        }

        // Ordena o vetor
        Array.Sort(vetor);

        // Inverte o vetor
        Array.Reverse(vetor);

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