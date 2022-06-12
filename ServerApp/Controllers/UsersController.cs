using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.DTOs;
using ServerApp.Filters;
using ServerApp.Models;
using ServerApp.Repositories;

namespace ServerApp.Controllers
{
    [ServiceFilter(typeof(LastActiveActionFilter))]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IRepository _repository;
        private IMapper _mapper;


        public UsersController(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserQueryParams userQueryParams)
        {
            await Task.Delay(2500);

            userQueryParams.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var query = _repository.Query<User>().Where(u => u.Id != userQueryParams.UserId).AsQueryable();

            if (userQueryParams.Gender != null)
            {
                if (userQueryParams.Gender == GenderEnum.male)
                {
                    query = query.Where(u => u.Gender == GenderEnum.male.ToString());
                }
                else
                {
                    query = query.Where(u => u.Gender == GenderEnum.female.ToString());
                }
            }

            if (userQueryParams.minAge != 18 || userQueryParams.maxAge != 100)
            {
                DateTime today = DateTime.Now;
                DateTime min = today.AddYears(-(userQueryParams.maxAge + 1));
                DateTime max = today.AddYears(-userQueryParams.minAge);

                query = query.Where(u => u.DateOfBirth >= min && u.DateOfBirth <= max);
            }

            if (!string.IsNullOrEmpty(userQueryParams.Country))
            {
                query = query.Where(u => u.Country.ToLower() == userQueryParams.Country.ToLower());
            }

            if (!string.IsNullOrEmpty(userQueryParams.City))
            {
                query = query.Where(u => u.City.ToLower() == userQueryParams.City.ToLower());
            }

            if (userQueryParams.OrderBy != null)
            {
                if (userQueryParams.OrderBy == OrderByEnum.Age)
                {
                    query = query.OrderBy(u => u.DateOfBirth);
                }
                else if (userQueryParams.OrderBy == OrderByEnum.Created)
                {
                    query = query.OrderByDescending(u => u.Created);
                }
                else if (userQueryParams.OrderBy == OrderByEnum.Name)
                {
                    query = query.OrderBy(u => u.Name);
                }
                else if (userQueryParams.OrderBy == OrderByEnum.LastActive)
                {
                    query = query.OrderByDescending(u => u.LastActive);
                }
            }

            var users = query.Include(u => u.Images).ToList();

            var result = _mapper.Map<List<UserForListDTO>>(users);

            return Ok(result);
        }

        [HttpGet("follows")]
        public async Task<IActionResult> Follows([FromQuery] UserQueryParams userQueryParams)
        {
            userQueryParams.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var users = _repository.Query<User>()
                .Where(u => u.Id == userQueryParams.UserId)
                .Include(u => u.Images)
                .Include(u => u.Followers)
                .Include(u => u.Followings)
                .AsQueryable();

            if (userQueryParams.Followers)
            {
                // takip edenler
                var result = await _repository.GetFollows(userQueryParams.UserId, false);
                users = users.Where(u => result.Contains(u.Id));
            }

            if (userQueryParams.Followings)
            {
                // takip edilenler
                var result = await _repository.GetFollows(userQueryParams.UserId, true);
                users = users.Where(u => result.Contains(u.Id));
            }

            return Ok(users.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _repository.GetAsync<User>(u => u.Id == id, u => u.Images);

            var result = _mapper.Map<UserForDetailsDTO>(user);

            return Ok(result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDTO userForUpdateDTO)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) // Kullanıcının id bilgisini almış olduk
                return BadRequest("Not Valid Request");


            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var user = await _repository.GetAsync<User>(id);

            _mapper.Map(userForUpdateDTO, user);

            if (await _repository.SaveChanges())
                return Ok();


            throw new Exception("Güncelleme sırasında bir hata oluştu.");
        }


        [HttpPost("{followerUserId}/follow/{userId}")]
        public async Task<IActionResult> FollowUser(int followerUserId, int userId)
        {
            if (followerUserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            if (followerUserId == userId)
            {
                return BadRequest("Kendinizi takip edemezsiniz");
            }

            var isAlreadyFollowed = await _repository
                .AnyAsync<UserToUser>(u => u.FollowerId == followerUserId && u.UserId == userId);

            if (isAlreadyFollowed)
            {
                return BadRequest("Zaten kullanıcıyı takip ediyorsunuz");
            }

            if (await _repository.GetAsync<User>(userId) == null)
            {
                return NotFound();
            }

            var follow = new UserToUser()
            {
                UserId = userId,
                FollowerId = followerUserId
            };

            await _repository.AddAsync(follow);
            await _repository.SaveChanges();
            return Ok();         
        }


        [HttpGet("isFollow/{userId}")]
        public async Task<IActionResult> AnyUser(int userId)
        {
            var key = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (key <= 0)
            {
                return Unauthorized();
            }

            var data = _repository.GetAsync<UserToUser>(
                u => u.FollowerId == key &&
                u.UserId == userId);

            return Ok(data != null ? true : false);
        }
    }
}