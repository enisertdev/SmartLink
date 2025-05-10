using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartLink.Application.Repositories.Link;
using SmartLink.Application.Repositories.User;
using SmartLink.Application.Services;
using SmartLink.Domain.Entities;
using SmartLink.Domain.Entities.Identity;

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
        private readonly UserManager<AppUser> _userManager;

        public class AddLinkRequest
        {
            public string Url { get; set; }
        }

        public LinkController(ILinkReadRepository linkReadRepository, ILinkWriteRepository linkWriteRepository, ILinkService linkService, IUserReadRepository userReadRepository, UserManager<AppUser> userManager)
        {
            _linkReadRepository = linkReadRepository;
            _linkWriteRepository = linkWriteRepository;
            _linkService = linkService;
            _userReadRepository = userReadRepository;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLink(string id)
        {
            var link = await _linkReadRepository.GetByIdAsync(Guid.Parse(id));
            if (link == null)
                return BadRequest(new { message = "Link could not be found" });
            return Ok(new {summary = link.Summary, title = link.Title});
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetLinks()
        {

            var links = _linkReadRepository.GetAll(tracking: false).ToList();
            return Ok(links);
        }

        [Authorize]
        [HttpGet("GetLinks")]
        public async Task<IActionResult> GetLinksForUser()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("Not authenticated.");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized("User not found");
            var links = _linkReadRepository.GetWhere(l => l.UserId == user.Id)
                .Select(l => new { l.Id, l.Title, l.Summary, })
                .ToList();
            return Ok(new
            {
                url = links
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddLink([FromBody] AddLinkRequest request)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("Not authenticated.");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized("User not found");
            try
            {
                string title = await _linkService.CreateSummaryTitle(request.Url);
                string sum = await _linkService.GetAiSummary(request.Url);

                //will be replaced with dto
                LinkEntity link = new LinkEntity
                { Url = request.Url, Summary = sum, UserId = user.Id, Title = title };
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

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteLink(string id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();
            var link = await _linkReadRepository.GetByIdAsync(Guid.Parse(id));
            if (link.UserId != userId)
                return Unauthorized();
            _linkWriteRepository.RemoveAsync(link);
            await _linkWriteRepository.SaveChangesAsync();
            return Ok("Link has been removed.");


        }
    }
}
