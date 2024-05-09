
using Microsoft.AspNetCore.Mvc;

namespace cush.Controllers;

[ApiController]
[Route("/")]
public class MainController:ControllerBase{
    [HttpGet]
    public ActionResult<string> Index(){
        return Ok("ok");
    }
}