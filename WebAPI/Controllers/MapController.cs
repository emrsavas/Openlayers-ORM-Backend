using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "DB.txt");
        private readonly JsonSerializerOptions jsonOptions;

        public MapController()
        {
            jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new PointConverter());
        }

        [HttpPost("Add")]
        public IActionResult AddCoordinate([FromBody] MapModel model)
        {
            try
            {
                List<MapModel> mapList = ReadDataFromFile();

                mapList.Add(model);

                WriteDataToFile(mapList);

                return Ok("Veri başarıyla dosyaya kaydedildi.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Veri kaydetme hatası: {ex.Message}");
            }
        }

        [HttpGet("Get")]
        public IActionResult GetList()
        {
            try
            {
                List<MapModel> mapList = ReadDataFromFile();

                return Ok(mapList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Veri okuma hatası: {ex.Message}");
            }
        }

        [HttpPost("Delete")]
        public IActionResult DeleteCoordinate([FromBody] DeleteModel model)
        {
            try
            {
                List<MapModel> mapList = ReadDataFromFile();

                mapList.RemoveAll(item => item.Name == model.name && item.Number == model.number);

                WriteDataToFile(mapList);

                return Ok("Veri başarıyla silindi.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Veri silme hatası: {ex.Message}");
            }
        }


        private List<MapModel> ReadDataFromFile()
        {
            if (System.IO.File.Exists(filePath))
            {
                string jsonData = System.IO.File.ReadAllText(filePath);

                return JsonSerializer.Deserialize<List<MapModel>>(jsonData, jsonOptions) ?? new List<MapModel>();
            }
            else
            {
                return new List<MapModel>();
            }
        }

        private void WriteDataToFile(List<MapModel> mapList)
        {
            string jsonData = JsonSerializer.Serialize(mapList, jsonOptions);
            System.IO.File.WriteAllText(filePath, jsonData);
        }
    }
}