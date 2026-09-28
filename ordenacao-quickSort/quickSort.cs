using System;
using System.Diagnostics;

class quickSort{
    static int[] numeros = new int[500000];

    static void Main()
    {
        numeros = GerarVetorParticaoEquilibrada(numeros.Length);

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

    static int[] GerarVetorParticaoEquilibrada(int tamanho)
    {
        int[] vetor = new int[tamanho];

        int meio = tamanho / 2;

        // Coloca valores menores que o pivô
        for (int i = 0; i < meio; i++)
        {
            vetor[i] = i + 1;
        }

        // Coloca valores maiores que o pivô
        for (int i = meio; i < tamanho - 1; i++)
        {
            vetor[i] = i + 2;
        }

        // O pivô fica por último
        vetor[tamanho - 1] = meio + 1;

        return vetor;
    }

    static void OrdenarArray()
        {
            QuickSort(numeros, 0, numeros.Length - 1);
        }

        static void QuickSort(int[] vetor, int inicio, int fim)
        {
            if (inicio < fim)
            {
                int posicaoPivo = Particionar(vetor, inicio, fim);

                QuickSort(vetor, inicio, posicaoPivo - 1);
                QuickSort(vetor, posicaoPivo + 1, fim);
            }
        }

        static int Particionar(int[] vetor, int inicio, int fim)
        {
            int pivo = vetor[fim];
            int i = inicio - 1;

            for (int j = inicio; j < fim; j++)
            {
                if (vetor[j] < pivo)
                {
                    i++;

                    int aux = vetor[i];
                    vetor[i] = vetor[j];
                    vetor[j] = aux;
                }
            }

            int aux2 = vetor[i + 1];
            vetor[i + 1] = vetor[fim];
            vetor[fim] = aux2;

            return i + 1;
        }
    }