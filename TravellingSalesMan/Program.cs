using System;

namespace TSP
{
    public static class Program
    {
        public static void Main(string[] args)
        {


            //var source = Enumerable.Range(100, 20000);

            //IEnumerable<int> parallelQuery3 = new List<int>() { 1 };

            //parallelQuery3 =
            //source.AsParallel()
            //    .Where(n => n % 10 == 0)
            //    .Select(n => n);

            Graph graph = new Graph();

            string fileName = "data.csv";
            try
            {
                if (File.Exists(fileName))
                {

                    //Format name,X,Y
                    Console.WriteLine("Load graph from file");
                    List<string> input = new List<string>();
                    using (var reader = new StreamReader(fileName))
                    {
                        
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            input.Add(line);
                        }
                    }
                    graph = GraphMethods.CreateGraphFromStringList(input);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading file");
            }

            //****************** GRAPH INTIALIZE *****************************
            if (graph.vertices.Count == 0)
            {
                Console.WriteLine("Generate default graph");

                graph = GraphMethods.CreateFullGraph(10, 1, 1000, 0.001);


            }

            Console.WriteLine("Graph details: ");
            foreach (var vertex in graph.vertices)
            {

                Console.WriteLine($"Vertex {vertex.index.ToString()} Data: {vertex.data.ToString()} ");
                {
                    foreach (var neighbor in vertex.neighbors)
                    {
                        Console.WriteLine($" \t Index = {neighbor.vertex2.index} Neighbor = {neighbor.vertex2.name}, Distance = {neighbor.distance.ToString()})");
                    }

                }
                Console.WriteLine(" ");
            }



            //******************** BRUTE FORCE *******************************
            Hamilton h = new Hamilton();
             h.graph = graph;                         
            
             Console.WriteLine("Brute Force");

            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            foreach (Vertex v in h.ShortestHamiltonCycle())
             {
                 Console.Write(v.index+" ");
             }

            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

            Console.WriteLine();
             Console.WriteLine("Distance: "+h.minDistance);
             Console.WriteLine();


            //******************** Parallel BRUTE FORCE *******************************
            h = new Hamilton();
            h.graph = graph;

            Console.WriteLine("Parallel Brute Force - Disordered");

            watch.Restart();

            Parallel.ForEach (h.ShortestHamiltonCycle(), v => 
            {
                Console.Write(v.index + " ");
            });

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

            //foreach (Vertex v in h.ShortestHamiltonCycle())
            //{
            //    Console.Write(v.index + " ");
            //}
            Console.WriteLine();
            Console.WriteLine("Distance: " + h.minDistance);
            Console.WriteLine();






            //******************** ANT ***************************************
            //Doesn't like float distances
            AntOptimization ant = new AntOptimization();
            ant.graph = graph;
            ant.alpha = 1;
            ant.beta = 5;
            ant.ro = 0.5;

            Console.WriteLine("Ant");
            foreach (Vertex v in ant.AntColonyOptimization(100, 100))
            {
                Console.Write(v.index + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Distance: " + ant.minDistance);
            Console.WriteLine();
            //**************** GENETIC ***************************************
            Genetic genetic = new Genetic();
            genetic.graph = graph;  
            
            genetic.GenerateChromosomes(100);
            
            genetic.mutationRate = (int)0.047*genetic.chromosomes.Count;
            genetic.crossingRate = genetic.chromosomes.Count;
            
            Console.WriteLine("Genetic");
            foreach(Vertex v in genetic.GeneticOptimization(100))
            {
                Console.Write(v.index+" ");
            }
            Console.Write(genetic.chromosomes[0].genes[0].index);
            Console.WriteLine();
            Console.WriteLine("Distance: "+genetic.chromosomes[0].rating); 
            Console.WriteLine();     
            //************ NEAREST NEIGHBOUR *********************************
            NearestNeighbour nn = new NearestNeighbour();
            nn.graph = graph;
                        
            Console.WriteLine("Nearest Neighbour");
            foreach(Vertex v in nn.NearestNeighbourOptimization())
            {
                Console.Write(v.index+" ");
            }  
            Console.WriteLine();                                       
            Console.WriteLine("Distance: "+nn.minDistance);
            Console.WriteLine();
        }
    }
}
