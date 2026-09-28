using System;
using System.Diagnostics;

class selectionSort {

    static int[] numeros = new int[300000];

    static void Main() {

    Stopwatch cronometro = new Stopwatch();

        // Melhor caso: vetor já ordenado (o índice do menor nunca é atualizado)
        numeros = GerarCrescente(numeros.Length);

        cronometro.Start();

        OrdenarArray(numeros);

        cronometro.Stop();

        Console.WriteLine("Melhor caso: " + cronometro.Elapsed.TotalMilliseconds);

        // Pior caso: vetor decrescente (o índice do menor é atualizado ~n²/4 vezes)
        numeros = GerarDecrescente(numeros.Length);

        cronometro.Restart();

        OrdenarArray(numeros);

        cronometro.Stop();

        Console.WriteLine("Pior caso: " + cronometro.Elapsed.TotalMilliseconds);

    }

    static void GerarArray(){

        Random random = new Random();

        for (int i = 0; i < numeros.Length; i++)
        {
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

    static void OrdenarArray(int[] numeros){

        // é um algoritmo simples que divide a lista em duas partes: os elementos já ordenados à esquerda e os não ordenados à direita.
        // baseado em comparação, seleciona o menor elemento
        // da parte não ordenada e troca com o primeiro elemento nao ordenado

        // Encontre o menor elemento e troque-o com o primeiro elemento. Dessa forma, o menor elemento ficará na posição correta.
        // Em seguida, encontre o menor elemento entre os restantes (ou o segundo menor) e troque-o com o segundo elemento.
        // Continuamos fazendo isso até que todos os elementos estejam na posição correta.

        int n = numeros.Length;

        for(int i= 0; i < n -1; i++){

            int menorIndiceNaoOrdenado = i;

            for(int j = i + 1; j < n; j++){
                
                if (numeros[j] < numeros[menorIndiceNaoOrdenado]){

                    menorIndiceNaoOrdenado = j;
                }
            }
            int aux = numeros[i];
            numeros[i] = numeros[menorIndiceNaoOrdenado];
            numeros[menorIndiceNaoOrdenado] = aux;
        }
    }

}