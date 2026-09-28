using System;
using System.Diagnostics;

class mergeSort{
    static int[] numeros = new int[300000];

    static void Main(){
        Stopwatch tempoOrdenacao = new Stopwatch();

        // Aquecimento (fora do cronômetro): evita que a compilação do JIT
        // entre na primeira medição e distorça a comparação entre os casos
        numeros = GerarPiorCaso(numeros.Length);
        OrdenarArray();

        // Melhor caso: vetor já ordenado (cada merge esgota a metade esquerda logo)
        numeros = GerarCrescente(numeros.Length);

        tempoOrdenacao.Start();
        OrdenarArray();
        tempoOrdenacao.Stop();

        Console.WriteLine("Melhor caso: " + tempoOrdenacao.Elapsed.TotalMilliseconds);

        // Pior caso: cada merge alterna entre as duas metades até o fim
        numeros = GerarPiorCaso(numeros.Length);

        tempoOrdenacao.Restart();
        OrdenarArray();
        tempoOrdenacao.Stop();

        Console.WriteLine("Pior caso: " + tempoOrdenacao.Elapsed.TotalMilliseconds);

    }

    public static void GerarArray(){
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
        int[] vetor = GerarCrescente(tamanho);

        PreencherPiorCaso(vetor, 0, tamanho - 1);

        return vetor;
    }

    static void PreencherPiorCaso(int[] vetor, int inicio, int fim)
    {
        if (inicio >= fim)
        {
            return;
        }

        int tamanho = fim - inicio + 1;
        int qtdEsquerda = (tamanho + 1) / 2;
        int[] aux = new int[tamanho];

        // Posições pares vão para a esquerda e ímpares para a direita
        for (int i = 0; i < tamanho; i++)
        {
            if (i % 2 == 0)
            {
                aux[i / 2] = vetor[inicio + i];
            }
            else
            {
                aux[qtdEsquerda + i / 2] = vetor[inicio + i];
            }
        }

        Array.Copy(aux, 0, vetor, inicio, tamanho);

        // Mesma divisão usada no Sort
        int meio = (inicio + fim) / 2;

        PreencherPiorCaso(vetor, inicio, meio);
        PreencherPiorCaso(vetor, meio + 1, fim);
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

        for (int e = 0; e < n1; e++){
            esquerda[e] = vetor[inicio + e];
        }

        for (int d = 0; d < n2; d++){
            direita[d] = vetor[meio + 1 + d];
        }

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