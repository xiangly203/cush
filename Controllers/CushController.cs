using AutoMapper;
using cush.DTO;
using cush.Models;
using cush.Service;
using Microsoft.AspNetCore.Mvc;

namespace cush.Controllers;



[ApiController]
[Route("api/[controller]")]
public class CushController: ControllerBase
{
    private readonly CushService _cushService;
    private readonly IMapper _mapper;

    public CushController(CushService cushService, IMapper mapper)
    {
        _cushService = cushService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<bool>> CreateTransaction(CushDTO cushDTO)
    {
        var isSuc = await _cushService.CreateCushesAsync(cushDTO);
        if (isSuc)
        {
            return Ok("ok");
        }
        return BadRequest();
    }

    [HttpGet("range")]
    public async Task<ActionResult<List<CushDTO>>> GetTransactionsInTimeRange(string StartDateStr, string endDateStr)
    {
        var start = DateTime.ParseExact(StartDateStr, "yyyy-MM-dd", null);
        var end = DateTime.ParseExact(endDateStr + " 23:59:59", "yyyy-MM-dd HH:mm:ss", null);
        TimeZoneInfo chinaStandardTime = TimeZoneInfo.FindSystemTimeZoneById("Asia/Shanghai");
        DateTime startDateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(start,chinaStandardTime);
        DateTime endDateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(end,chinaStandardTime);
        try
        {
            var cushes = await _cushService.GetCushesInTimeRange(startDateTimeUtc, endDateTimeUtc);
            return Ok(cushes);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}

