using System;
using System.Diagnostics;

class Program
{
    static int[] numeros = new int[10000];

    static void Main()
    {
    /*    int opcao;

        do
        {
            Console.Clear();
            Console.WriteLine("===== MENU =====");
            Console.WriteLine("1 - Gerar array aleatório");
            Console.WriteLine("2 - Ordenar array");
            Console.WriteLine("3 - Mostrar array");
            Console.WriteLine("4 - Mostrar números primos");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha uma opção: ");

            int.TryParse(Console.ReadLine(), out opcao);

            switch (opcao)
            {
                case 1:
                    GerarArray();
                    break;

                case 2:
                    OrdenarArray();
                    break;

                case 3:
                    MostrarArray();
                    break;

                case 4:
                    MostrarPrimos();
                    break;

                case 0:
                    Console.WriteLine("Encerrando...");
                    break;

                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

            if (opcao != 0)
            {
                Console.WriteLine("\nPressione qualquer tecla...");
                Console.ReadKey();
            }

        } while (opcao != 0);
    */

        Stopwatch tempoTotal = new Stopwatch();
        tempoTotal.Start();

        Stopwatch tempoGeracao = new Stopwatch();
        tempoGeracao.Start();
        GerarArray();
        tempoGeracao.Stop();
        Console.WriteLine($"{tempoGeracao.Elapsed.TotalMilliseconds}");

        Stopwatch tempoOrdenacao = new Stopwatch();
        tempoOrdenacao.Start();
        OrdenarArray();
        tempoOrdenacao.Stop();
        Console.WriteLine($"{tempoOrdenacao.Elapsed.TotalMilliseconds}");

        Stopwatch tempoPrimos = new Stopwatch();
        tempoPrimos.Start();
        MostrarPrimos();
        tempoPrimos.Stop();
        Console.WriteLine($"{tempoPrimos.Elapsed.TotalMilliseconds}");

// reordenando e gerando os primos de novo 
        
        tempoOrdenacao.Restart();
        OrdenarArray();
        tempoOrdenacao.Stop();
        Console.WriteLine($"{tempoOrdenacao.Elapsed.TotalMilliseconds}");

        tempoPrimos.Restart();
        MostrarPrimos();
        tempoPrimos.Stop();
        Console.WriteLine($"{tempoPrimos.Elapsed.TotalMilliseconds}");

        tempoTotal.Stop();
        Console.WriteLine($"{tempoTotal.Elapsed.TotalMilliseconds}");
    }

    static void GerarArray()
    {
        Random random = new Random();

        for (int i = 0; i < numeros.Length; i++)
        {
            numeros[i] = random.Next(0, 15001);
        }

    //    Console.WriteLine("Array gerado com sucesso!");
    }

    static void OrdenarArray()
    {

        for (int i = 0; i < numeros.Length; i++)
        {
            for (int j = i + 1; j < numeros.Length; j++)
            {
                if (numeros[i] > numeros[j])
                {
                    int aux = numeros[i];
                    numeros[i] = numeros[j];
                    numeros[j] = aux;
                }
            }
        }

    //    Console.WriteLine("Array ordenado!");
    }

    static void MostrarArray()
    {
        for (int i = 0; i < numeros.Length - 1; i++)
        {
    //            Console.Write(numeros[i] + " ");
        }        

    }

    static void MostrarPrimos()
    {

    //        Console.WriteLine("Números primos encontrados:\n");

        foreach (int numero in numeros)
        {
            if (EhPrimo(numero))
            {
    //            Console.Write(numero + " ");
            }
        }

    //    Console.WriteLine();
    }

    static bool EhPrimo(int numero)
    {
        if (numero < 2)
            return false;

        if (numero == 2)
            return true;

        if (numero % 2 == 0)
            return false;

        for (int i = 3; i * i <= numero; i += 2)
        {
            if (numero % i == 0)
                return false;
        }

        return true;
    }
}