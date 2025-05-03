using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLink.Application.Repositories.Link;
using SmartLink.Application.Repositories.User;
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
        private readonly IUserReadRepository _userReadRepository;

        public class AddLinkRequest
        {
            public string Url { get; set; }
            public string Title { get; set; }
            public Guid UserId { get; set; }
        }

        public LinkController(ILinkReadRepository linkReadRepository, ILinkWriteRepository linkWriteRepository, ILinkService linkService, IUserReadRepository userReadRepository)
        {
            _linkReadRepository = linkReadRepository;
            _linkWriteRepository = linkWriteRepository;
            _linkService = linkService;
            _userReadRepository = userReadRepository;
        }

        [HttpGet]
        public IActionResult GetLinks()
        {

            var links =  _linkReadRepository.GetAll(tracking:false).ToList();
            return Ok(links);
        }

        [HttpPost]
        public async Task<IActionResult> AddLink([FromBody] AddLinkRequest request)
        {
            try
            {
                string sum = await _linkService.GetAiSummary(request.Url);
                //will be replaced with dto
                LinkEntity link = new LinkEntity
                    { Url = request.Url, Title = request.Title, Summary = sum, UserId = request.UserId };
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
