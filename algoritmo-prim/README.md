# Algoritmo de Prim
 
Implementação em **C#** do algoritmo de **Prim**, um algoritmo guloso para encontrar a Árvore Geradora Mínima (*Minimum Spanning Tree*) de um grafo conexo, não direcionado e ponderado.
 
Projeto desenvolvido para a disciplina de **Análise e Projeto de Algoritmos** (Computação — UNISINOS), como parte do Relatório de Implementação sobre algoritmos gulosos.
 
## Autoras
 
- Eduarda Maciel
- Luiza Klein

## Sobre o algoritmo
 
O algoritmo de Prim constrói a MST a partir de um vértice raiz, adicionando a cada passo a **aresta de menor peso** que conecta a árvore já construída a um vértice ainda fora dela. Essa escolha local ótima (estratégia gulosa) leva a uma solução globalmente ótima.
 
A implementação segue o pseudocódigo `MST-PRIM` apresentado em *Cormen et al., Introduction to Algorithms*:
 
```
MST-PRIM(G, w, r)
  para cada u em G.V
      u.chave = ∞
      u.π = NIL
  r.chave = 0
  Q = G.V
  enquanto Q ≠ ∅
      u = EXTRACT-MIN(Q)
      para cada v em G.Adj[u]
          se v ∈ Q e w(u, v) < v.chave
              v.π = u
              v.chave = w(u, v)
```
 
### Complexidade
 
| Estrutura para a fila de prioridades | Complexidade de tempo |
|--------------------------------------|-----------------------|
| Vetor / busca linear                 | O(V²)                 |
| Heap binário (min-heap)              | O(E log V)            |
 
> Ajuste a tabela de acordo com a estrutura utilizada na implementação.
 
## Estrutura do projeto
 
```text
algoritmo-prim/
├── Program.cs             # ponto de entrada: aquecimento (JIT) e execução dos três testes
├── Prim.cs                # implementação do MST-PRIM
├── Grafo.cs               # grafo não direcionado e ponderado (lista de adjacência)
├── ResultadoMst.cs        # resultado do algoritmo: vetor de pais e custo total
├── GeradorGrafo.cs        # gerador de grafos aleatórios e conexos para os testes
├── ExecutorTestes.cs      # testes de pequena, média e grande instância, com medição de tempo
├── VerificadorKruskal.cs  # verificador independente (Kruskal + union-find)
├── algoritmo-prim.csproj  # arquivo de projeto .NET
└── README.md
```
 
## Como executar
 
1. Instale o [.NET SDK 10](https://dotnet.microsoft.com/download).
2. Clone o repositório:
```bash
   git clone https://github.com/rkluiza/complexity-and-analysis-of-algorithms.git
   cd complexity-and-analysis-of-algorithms/algoritmo-prim
```
 
3. Compile e execute:
```bash
   dotnet build
   dotnet run
```
 
## Referência principal
 
- CORMEN, T. H.; LEISERSON, C. E.; RIVEST, R. L.; STEIN, C. *Algoritmos: Teoria e Prática*. Capítulo sobre Árvores Geradoras Mínimas.