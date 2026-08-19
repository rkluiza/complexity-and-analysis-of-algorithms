using System;
using System.Diagnostics;

class bubbleSort{
    static int[] numeros = new int[1000];

    static void Main()
    {
        GerarArray();

        Stopwatch tempoTotal = new Stopwatch();
        tempoTotal.Start();

        Stopwatch tempoOrdenacao = new Stopwatch();

        tempoOrdenacao.Start();
        OrdenarArray();
        tempoOrdenacao.Stop();
        Console.WriteLine("Ordenar:" + tempoOrdenacao.Elapsed.TotalMilliseconds);

        // Reordenando
        tempoOrdenacao.Restart();
        OrdenarArray();
        tempoOrdenacao.Stop();
        Console.WriteLine("Reordenar:" + tempoOrdenacao.Elapsed.TotalMilliseconds);

        tempoTotal.Stop();
        Console.WriteLine("Tempo total:" + tempoTotal.Elapsed.TotalMilliseconds);
    }

    public static void GerarArray()
    {
        Random random = new Random();

        for (int i = 0; i < numeros.Length; i++){
            numeros[i] = random.Next(0, 10001);
        }
    }

    static void OrdenarArray()
    {
        for (int i = 0; i < numeros.Length - 1; i++){
            for (int j = 0; j < numeros.Length - 1 - i; j++){
                if (numeros[j] > numeros[j + 1]){
                    int aux = numeros[j];
                    numeros[j] = numeros[j + 1];
                    numeros[j + 1] = aux;
                }
            }
        }
    }
}