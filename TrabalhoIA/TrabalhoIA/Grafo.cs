using System;
using System.Collections.Generic;
using System.Linq;

namespace TrabalhoIA
{
   
    public abstract class GraphBase<VT, ST>
    {
        protected Dictionary<VT, List<ST>> VerticesAdjecentes = new Dictionary<VT, List<ST>>();

        public IEnumerable<VT> Vertices()
        {
            return VerticesAdjecentes.Keys;
        }

        public abstract IEnumerable<VT> VerticesAdjacentes(VT vertice);

        protected void AdicionarVertice(VT vertice)
        {
            VerticesAdjecentes.Add(vertice, new List<ST>());
        }

        public virtual void DFS(VT root, Action<VT> preAction, Action<VT> postAction)
        {
            DFS_Visit(root, preAction, postAction, new HashSet<VT>());
        }

        void DFS_Visit(VT vertice, Action<VT> preAction, Action<VT> postAction, HashSet<VT> verticesVisitados)
        {
            verticesVisitados.Add(vertice);

            if (preAction != null)
                preAction(vertice);

            foreach (var verticeAdjacente in VerticesAdjacentes(vertice))
                if (!verticesVisitados.Contains(verticeAdjacente))
                    DFS_Visit(verticeAdjacente, preAction, postAction, verticesVisitados);

            if (postAction != null)
                postAction(vertice);
        }
    }

    public abstract class GraphProtected<VT> : GraphBase<VT, VT>
    {
        public override IEnumerable<VT> VerticesAdjacentes(VT vertice)
        {
            return VerticesAdjecentes[vertice];
        }

        protected void AdicionarArestas(VT origem, VT destino)
        {
            VerticesAdjecentes[origem].Add(destino);
        }
    }

    public class Grafo<VT> : GraphProtected<VT>
    {
        public void AdicionarVerticeX(VT verticex)
        {
            AdicionarVertice(verticex);
        }

        public void AdicionarAresta(VT origem, VT destino)
        {
            AdicionarArestas(origem, destino);
        }
    }

    public class Grafo<VT, ET> : GraphBase<VT, Tuple<ET, VT>> where VT : class
    {
        public void AdicionarVetice(VT vertice)
        {
            AdicionarVertice(vertice);
        }

        public void AdicionarAresta(VT verticeOrigem, ET aresta, VT verticeDestino)
        {
            VerticesAdjecentes[verticeOrigem].Add(Tuple.Create(aresta, verticeDestino));
        }

        public override IEnumerable<VT> VerticesAdjacentes(VT vertice)
        {
            return VerticesAdjecentes[vertice].Select(verticeEAresta => verticeEAresta.Item2);
        }

        public IEnumerable<VT> PegarVertices()
        {
            return Vertices();
        }

        public ET PegarAresta(VT origem, VT destino)
        {
            return VerticesAdjecentes[origem].Single(aresta => aresta.Item2 == destino).Item1;
        }
    }
}