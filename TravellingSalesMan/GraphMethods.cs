using System;

namespace TSP
{
    public static class GraphMethods
    {
        public static Graph CreateFullGraph(int size, int minDisctance, int maxDistance, double pheromoneValue)
        {
            Graph graph = new Graph();
            Random rand = new Random();

            for (int i = 0; i < size; i++)
            {
                Vertex vertex = new Vertex();
                vertex.index = i;                
                graph.vertices.Add(vertex);
            }
            
            for(int i=0;i<size;i++)
            {
                for(int j=i+1;j<size;j++)
                {
                    Edge edge = new Edge();
                    edge.distance = rand.Next(minDisctance, maxDistance);                    
                    edge.pheromoneValue = pheromoneValue;
                    edge.vertex1 = graph.vertices[i];
                    edge.vertex2 = graph.vertices[j];
                    
                    graph.vertices[i].neighbors.Add(edge);
                    
                    Edge edge2 = new Edge();
                    edge2.distance = edge.distance;
                    edge2.pheromoneValue = pheromoneValue;
                    edge2.vertex1 = graph.vertices[j];
                    edge2.vertex2 = graph.vertices[i];
                    
                    graph.vertices[j].neighbors.Add(edge2);
                    
                    graph.edges.Add(edge);
                    graph.edges.Add(edge2);
                }
            }

            return graph;
        }


        public static Graph CreateGraphFromStringList(List<string> data)
        //    public static Graph CreateGraphFromXAndYCoordinates(int size, int minDisctance, int maxDistance, double pheromoneValue)
        {

            double pheromoneValue = 0.001;

            Graph graph = new Graph();
            Random rand = new Random();

            //Format name,X,Y

            for (int i = 0; i < data.Count; i++)
            {
                string[] vertexData = data[i].Split(',');
                Vertex vertex = new Vertex();
                vertex.index = i;
                vertex.name = vertexData[0];
                vertex.xCoordinate = float.Parse(vertexData[1], System.Globalization.CultureInfo.InvariantCulture); 
                vertex.yCoordinate = float.Parse(vertexData[2], System.Globalization.CultureInfo.InvariantCulture);
                graph.vertices.Add(vertex);
            }

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = i + 1; j < data.Count; j++)
                {
                    Edge edge = new Edge();
                    edge.pheromoneValue = pheromoneValue;
                    edge.vertex1 = graph.vertices[i];
                    edge.vertex2 = graph.vertices[j];
                    edge.distance = CalculateDistance(edge.vertex1.xCoordinate, edge.vertex1.yCoordinate, edge.vertex2.xCoordinate, edge.vertex2.yCoordinate);

                    graph.vertices[i].neighbors.Add(edge);

                    Edge edge2 = new Edge();
                    edge2.distance = edge.distance;
                    edge2.pheromoneValue = pheromoneValue;
                    edge2.vertex1 = graph.vertices[j];
                    edge2.vertex2 = graph.vertices[i];

                    graph.vertices[j].neighbors.Add(edge2);

                    graph.edges.Add(edge);
                    graph.edges.Add(edge2);
                }
            }

            return graph;
        }

        private static float CalculateDistance(float x1, float y1, float x2, float y2)
        {
            //return Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
            return ((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
    }
}