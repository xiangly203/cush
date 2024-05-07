using AutoMapper;
using cush.DTO;
using cush.Models;
using cush.Service;
using cush.Config;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;
namespace cush.Controllers;



[ApiController]
[Route("api/[controller]")]
public class CushController: ControllerBase
{
    private readonly CushService _cushService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    private readonly IDatabase _cache;

    public CushController(CushService cushService, IMapper mapper, ILogger<CushController> logger, IDatabase cache)
    {
        _cushService = cushService;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
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
    public async Task<ActionResult<ResDto>> GetTransactionsInTimeRange(string StartDateStr, string endDateStr)
    {
        _logger.LogInformation("Getting transactions in time range: {StartDateStr} - {endDateStr}", StartDateStr, endDateStr);
        string key = RedisPrefix.CUSH + $"{StartDateStr}-{endDateStr}";
        var result = _cache.StringGet(key);
        if (result.HasValue){
            return Ok(JsonSerializer.Deserialize<ResDto>(result));
        }
        // _cache.Subscribe(key).OnMessage(channelMessage => {
        //     return Ok(channelMessage);
        // });
        var start = DateTime.ParseExact(StartDateStr, "yyyy-MM-dd", null);
        var end = DateTime.ParseExact(endDateStr + " 23:59:59", "yyyy-MM-dd HH:mm:ss", null);
        TimeZoneInfo chinaStandardTime = TimeZoneInfo.FindSystemTimeZoneById("Asia/Shanghai");
        DateTime startDateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(start,chinaStandardTime);
        DateTime endDateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(end,chinaStandardTime);
        try
        {
            var cushes = await _cushService.GetCushesInTimeRange(startDateTimeUtc, endDateTimeUtc);
            // _cache.Publish(key, JsonSerializer.Serialize(cushes));
            _cache.StringSet(key, JsonSerializer.Serialize(cushes),TimeSpan.FromMinutes(5));
            return Ok(cushes);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}

