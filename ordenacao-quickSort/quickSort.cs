using System;
using System.Diagnostics;
using System.Threading;

class quickSort{
    static int[] numeros = new int[500000];

    // No pior caso a recursão chega a profundidade n, o que estoura a pilha
    // padrão (1 MB). Por isso a ordenação roda em uma thread com pilha de 1 GB.
    const int TamanhoPilha = 1024 * 1024 * 1024;

    static void Main()
    {
        Stopwatch tempoOrdenacao = new Stopwatch();

        // Melhor caso: o pivô é sempre a mediana do subvetor
        numeros = GerarMelhorCaso(numeros.Length);

        tempoOrdenacao.Start();
        OrdenarComPilhaGrande();
        tempoOrdenacao.Stop();
        Console.WriteLine("Melhor caso:" + tempoOrdenacao.Elapsed.TotalMilliseconds);

        // Pior caso: vetor já ordenado (o pivô é sempre o maior elemento)
        numeros = GerarCrescente(numeros.Length);

        tempoOrdenacao.Restart();
        OrdenarComPilhaGrande();
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

    static int[] GerarMelhorCaso(int tamanho)
    {
        int[] vetor = new int[tamanho];

        PreencherMelhorCaso(vetor, 0, tamanho - 1, 1);

        return vetor;
    }

    static void PreencherMelhorCaso(int[] vetor, int inicio, int fim, int menorValor)
    {
        int tamanho = fim - inicio + 1;

        if (tamanho <= 0)
        {
            return;
        }

        // Quantos elementos ficam à esquerda do pivô (a mediana)
        int qtdMenores = (tamanho - 1) / 2;
        int qtdMaiores = tamanho - qtdMenores - 1;

        // Menores vêm primeiro, já no formato de melhor caso
        PreencherMelhorCaso(vetor, inicio, inicio + qtdMenores - 1, menorValor);

        if (qtdMaiores > 0)
        {
            int inicioMaiores = inicio + qtdMenores;

            PreencherMelhorCaso(vetor, inicioMaiores, fim - 1, menorValor + qtdMenores + 1);

            // A partição move o 1º maior para o fim do subvetor; aqui fazemos a rotação inversa
            int ultimo = vetor[fim - 1];
            Array.Copy(vetor, inicioMaiores, vetor, inicioMaiores + 1, qtdMaiores - 1);
            vetor[inicioMaiores] = ultimo;
        }

        // O pivô (último elemento) é a mediana
        vetor[fim] = menorValor + qtdMenores;
    }

    static void OrdenarComPilhaGrande()
    {
        Thread thread = new Thread(OrdenarArray, TamanhoPilha);
        thread.Start();
        thread.Join();
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