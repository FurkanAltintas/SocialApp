using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerApp.DTOs;
using ServerApp.Filters;
using ServerApp.Models;
using ServerApp.Repositories;

namespace ServerApp.Controllers
{
    [ServiceFilter(typeof(LastActiveActionFilter))]
    [Authorize]
    [ApiController]
    [Route("api/[controller]/{userId}")]
    public class MessagesController : ControllerBase
    {
        private IRepository _repository;
        private IMapper _mapper;

        public MessagesController(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreateDTO messageForCreateDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageForCreateDTO.SenderId = userId;

            var recipient = await _repository.GetAsync<User>(messageForCreateDTO.RecipientId);

            if (recipient == null)
                return BadRequest("Mesaj göndermek istediğiniz kullanıcı bulunmamaktadır");

            var message = _mapper.Map<Message>(messageForCreateDTO);

            await _repository.AddAsync(message);

            if (await _repository.SaveChanges())
            {
                var messageDTO = _mapper.Map<MessageForCreateDTO>(message);
                return Ok(messageDTO);
            }

            return BadRequest();
        }
    }
}