using System;
using System.Diagnostics;

public class shellSort
{

    static int[] numeros = new int[500000];
    public static void Main(string[] args)
    {
        GerarArray();

        Stopwatch tempoOrdenacao = new Stopwatch();

        tempoOrdenacao.Start();
        OrdenarArray(numeros, numeros.Length);
        tempoOrdenacao.Stop();

        Console.WriteLine("Ordenar:" + tempoOrdenacao.Elapsed.TotalMilliseconds);

        // Reordenando
        tempoOrdenacao.Restart();

        OrdenarArray(numeros, numeros.Length);
        tempoOrdenacao.Stop();

        Console.WriteLine("Reordenar:" + tempoOrdenacao.Elapsed.TotalMilliseconds);


    }

    static void GerarArray(){
       
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