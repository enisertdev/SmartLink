using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLink.Application.Repositories.Link;
using SmartLink.Application.Services;
using SmartLink.Domain.Entities;

namespace SmartLink.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly ILinkReadRepository _linkReadRepository;
        private readonly ILinkWriteRepository _linkWriteRepository;
        private readonly ILinkService _linkService;

        public LinkController(ILinkReadRepository linkReadRepository, ILinkWriteRepository linkWriteRepository, ILinkService linkService)
        {
            _linkReadRepository = linkReadRepository;
            _linkWriteRepository = linkWriteRepository;
            _linkService = linkService;
        }

        [HttpGet]
        public IActionResult GetLinks()
        {

            var links =  _linkReadRepository.GetAll(tracking:false).ToList();
            return Ok(links);
        }

        [HttpPost]
        public async Task<IActionResult> AddLink(string url, string title)
        {
            try
            {
                string sum = await _linkService.GetAiSummary(url);
                LinkEntity link = new LinkEntity
                    { Url = url, Title = title, Summary = sum };
                await _linkWriteRepository.AddAsync(link);
                await _linkWriteRepository.SaveChangesAsync();
                return Ok(link);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }
    }
}
