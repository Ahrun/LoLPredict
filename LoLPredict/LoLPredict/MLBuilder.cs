using DataScraper.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TensorFlowSharpCore;
namespace DataScraper
{
    public class MLBuilder
    {
        TFGraph graph = new TFGraph();
        public MLBuilder()
        {

        }

        public void NeuralNetTest(List<Match> matches, List<PlayerStats> playerStats)
        {
            graph.Seed = 100;

        }
        
    }
}
