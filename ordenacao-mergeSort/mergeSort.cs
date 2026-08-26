using System;
using System.Diagnostics;

class mergeSort{
    static int[] numeros = new int[300000];

    static void Main(){
        GerarArray();

        Stopwatch tempoTotal = new Stopwatch();
        tempoTotal.Start();

        Stopwatch tempoOrdenacao = new Stopwatch();

        tempoOrdenacao.Start();
        OrdenarArray();
        tempoOrdenacao.Stop();

        Console.WriteLine("Ordenar: " + tempoOrdenacao.Elapsed.TotalMilliseconds);

        //reordenando
        tempoOrdenacao.Restart();
        OrdenarArray();
        tempoOrdenacao.Stop();

        Console.WriteLine("Reordenar: " + tempoOrdenacao.Elapsed.TotalMilliseconds);
    }

    public static void GerarArray(){
        Random random = new Random();

        for (int i = 0; i < numeros.Length; i++){
            numeros[i] = random.Next(0, 10001);
        }
    }

    static void OrdenarArray(){
        Sort(numeros, 0, numeros.Length - 1);
    }

    static void Sort(int[] vetor, int inicio, int fim){
        if (inicio < fim){
            int meio = (inicio + fim) / 2;

            Sort(vetor, inicio, meio);
            Sort(vetor, meio + 1, fim);

            Merge(vetor, inicio, meio, fim);
        }
    }

    static void Merge(int[] vetor, int inicio, int meio, int fim){
        int n1 = meio - inicio + 1;
        int n2 = fim - meio;

        int[] esquerda = new int[n1];
        int[] direita = new int[n2];

        Array.Copy(vetor, inicio, esquerda, 0, n1);
        Array.Copy(vetor, meio + 1, direita, 0, n2);

        int i = 0;
        int j = 0;
        int k = inicio;

        while (i < n1 && j < n2){
            if (esquerda[i] <= direita[j]){
                vetor[k] = esquerda[i];
                i++;
            }
            else{
                vetor[k] = direita[j];
                j++;
            }

            k++;
        }

        while (i < n1){
            vetor[k] = esquerda[i];
            i++;
            k++;
        }

        while (j < n2){
            vetor[k] = direita[j];
            j++;
            k++;
        }
    }
}