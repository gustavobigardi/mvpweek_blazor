using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MVPWeek.Server.Data;
using MVPWeek.Server.Models;

namespace MVPWeek.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParticipantController : ControllerBase
    {
        private readonly ILogger<ParticipantController> _logger;
        private readonly DemoDbContext _context;

        private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

        public ParticipantController(ILogger<ParticipantController> logger,
            DemoDbContext context,
            IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            _logger = logger;
            _context = context;
            _apiBehaviorOptions = apiBehaviorOptions;
        }

        [HttpPost("add")]
        public async Task<IActionResult> CreateParticipant([FromBody] Participant participant)
        {
            if (_context.Participants.Any(p => p.Email.Equals(participant.Email)))
            {
                ModelState.AddModelError("Email", "This email is already registered!");
                return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }

            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("raffle")]
        public async Task<IActionResult> Raffle([FromServices] IConfiguration configuration, [FromBody] Raffle raffle)
        {
            if (!raffle.Password.Equals(configuration.GetValue<string>("RafflePassword")))
            {
                ModelState.AddModelError("Password", "Wrong password for raffle!");
                return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }   

            using(var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var participant = await _context.Participants
                        .Where(p => !p.Selected)
                        .OrderBy(p => new Guid())
                        .FirstOrDefaultAsync();

                    if (participant != null) {
                        participant.Selected = true;
                        _context.Participants.Update(participant);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        return Ok(participant);
                    } 

                    await transaction.RollbackAsync();
                    return NoContent();
                }
                catch (System.Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}