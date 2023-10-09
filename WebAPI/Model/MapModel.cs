using System.Drawing;
using System.Text.Json.Serialization;

namespace WebAPI.Model
{
    public class MapModel
    {
        public string? Name { get; set; }
        public int Number { get; set; }
        public int EdgeCount { get; set; }

        public List<List<double>> Coordinates { get; set; }
    }

   



}
