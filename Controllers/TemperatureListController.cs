using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace task1.Controllers
{
    [Route("api")]
    [ApiController]
    public class TemperatureListController : ControllerBase
    {
        private readonly List<TemperatureList> list;
        public TemperatureListController(List<TemperatureList> list)
        {
            this.list = list;
        }
        private List<TemperatureList> FindRange(string dateMin = "", string dateMax = "", bool delete = false)
        {
            List<TemperatureList> outputList = new List<TemperatureList>();
            DateTime dateMinC;
            DateTime dateMaxC;
            if (dateMin == "") dateMinC = DateTime.MinValue;
            else dateMinC = Convert.ToDateTime(dateMin);
            if (dateMax == "") dateMaxC = DateTime.MaxValue;
            else dateMaxC = Convert.ToDateTime(dateMax);
            for (int i = 0; i < list.Count; i++)
                if (list[i].date >= dateMinC && list[i].date <= dateMaxC)
                {
                    if (!delete) outputList.Add(list[i]);
                    else list.RemoveAt(i--);
                }
            return outputList;
        }

        [HttpGet("list")]
        public IEnumerable<string> List(string dateMin = "", string dateMax = "")
        {
            List<TemperatureList> outputList = FindRange(dateMin, dateMax);
            return Enumerable.Range(0, outputList.Count()).Select(index => outputList[index].date + " " + outputList[index].temp);
        }

        [HttpPost("save")]
        public IActionResult Save([FromQuery] string date, float temp)
        {
            list.Add(new TemperatureList { date = Convert.ToDateTime(date), temp = temp });
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] string dateMin = "", string dateMax = "")
        {
            FindRange(dateMin, dateMax, true);
            return Ok();
        }

        [HttpPut("changes")]
        public IActionResult changes([FromQuery] string date, float temp)
        {
            list[list.FindIndex(index => index.date == Convert.ToDateTime(date))].temp = temp;
            return Ok();
        }

    }
}
