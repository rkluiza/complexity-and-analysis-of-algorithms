using System;
using System.Diagnostics;

class selectionSort {

    static int[] numeros = new int[300000];

    static void Main() {

    Stopwatch cronometro = new Stopwatch();
    Stopwatch cronometro2 = new Stopwatch();
    Stopwatch tempoTotal = new Stopwatch();

        tempoTotal.Start();

        GerarArray();

        cronometro.Start();

        OrdenarArray(numeros);

        cronometro.Stop();

        cronometro2.Start();

        OrdenarArray(numeros);

        cronometro2.Stop();

        tempoTotal.Stop();

        Console.WriteLine("Ordenar desdordenado: " + cronometro.Elapsed.TotalMilliseconds);
        Console.WriteLine("Ordenar ordenado: " + cronometro2.Elapsed.TotalMilliseconds);
        Console.WriteLine("Tempo total: " + tempoTotal.Elapsed.TotalMilliseconds);
   
    }

    static void GerarArray(){
       
        Random random = new Random();

        for (int i = 0; i < numeros.Length; i++)
        {
            numeros[i] = random.Next(0, 10001);
        }
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