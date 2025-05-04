using Microsoft.AspNetCore.Authorization;
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

            var links = _linkReadRepository.GetAll(tracking: false).ToList();
            return Ok(links);
        }

        [HttpGet("GetLinks/{username}")]
        [Authorize]
        public async Task<IActionResult> GetLinksForUser(string username)
        {
            var getUsername = User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            if (getUsername == null)
                return Unauthorized("Not authenticated.");
            var getUser = await _userReadRepository.GetSingleAsync(u => u.Username == getUsername);
            if (getUser == null)
                return Unauthorized("User not found");
            var links = _linkReadRepository.GetWhere(l => l.UserId == getUser.Id)
                .Select(l => new { l.Id,l.Title, l.Summary, }).ToList();
            return Ok(new
            {
                url = links
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddLink([FromBody] AddLinkRequest request)
        {
            var getUsername = User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            if (getUsername == null)
                return Unauthorized("Not authenticated.");
            var getUser = await _userReadRepository.GetSingleAsync(u => u.Username == getUsername);
            if (getUser == null)
                return Unauthorized("User not found");
            try
            {
                string sum = await _linkService.GetAiSummary(request.Url);
                //will be replaced with dto
                LinkEntity link = new LinkEntity
                { Url = request.Url, Summary = sum, UserId = getUser.Id };
                await _linkWriteRepository.AddAsync(link);
                await _linkWriteRepository.SaveChangesAsync();
                return Ok(new
                {
                    summary = link.Summary,
                    id = link.Id
                }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }
    }
}
