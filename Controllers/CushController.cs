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
    public async Task<ActionResult<List<CushDTO>>> GetTransactionsInTimeRange(DateTime startDate, DateTime endDate)
    {
        try
        {
            var cushes = await _cushService.GetCushesInTimeRange(startDate, endDate);
            return Ok(cushes);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}

