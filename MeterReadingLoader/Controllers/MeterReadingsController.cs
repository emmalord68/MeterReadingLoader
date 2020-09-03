using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using MeterReading.DataService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MeterReadingLoader.Controllers
{
    [ApiController]
    [Route("")]
    public class MeterReadingsController : ControllerBase
    {
        private readonly ILogger<MeterReadingsController> _logger;
        private IReadingService _readingService;

        public MeterReadingsController(ILogger<MeterReadingsController> logger, IReadingService readingService)
        {
            _logger = logger;
            _readingService = readingService;
        }

        [HttpPost("meter-reading-uploads")]
        public ActionResult Upload(IFormFile file)
        {
            int successfulReadings = 0;
            int failedReadings = 0;

            try
            {
                if (file.Length > 0)
                {
                    //Save file to temporary location

                    var filePath = Path.Combine(Environment.CurrentDirectory, "Temp", Guid.NewGuid().ToString());
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    // process file
                    string[] lines = System.IO.File.ReadAllLines(filePath);

                    // Don't process the first line - it contains headers.
                    for (int i = 1; i < lines.Length; i++)
                    {
                        try
                        {
                            string[] columns = lines[i].Split(',');
                            _readingService.CreateReading(columns[0], columns[1], columns[2]);
                            successfulReadings++;
                        }
                        catch (Exception ex)
                        {
                            failedReadings++;
                            // Log error
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                // Log error
                return BadRequest();
            }

            return Ok(new { Successful = successfulReadings, Failed = failedReadings  });
        }
    }
}
