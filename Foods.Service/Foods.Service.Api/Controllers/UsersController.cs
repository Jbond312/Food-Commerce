using System.Threading.Tasks;
using AutoMapper;
using Common.Validation;
using Foods.Service.Repository.Users;
using Foods.Service.Repository.Users.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Service.Business.Entities;

namespace Foods.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserRepository<User> _repo;
        private readonly IEntityValidator<UserDto> _validator;
        private readonly IMapper _mapper;

        public UsersController(
            IMapper mapper, 
            IUserRepository<User> repo, 
            IEntityValidator<UserDto> validator
        )
        {
            _repo = repo;
            _validator = validator;
            _mapper = mapper;
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update([FromBody] UserDto userDto)
        {
            var userId = JwtMetaData.UserId;

            async Task<UserDto> UpdateUser()
            {
                userDto.Id = userId;
                await _validator.ValidateEntityAsync(userDto);

                var user = _mapper.Map<User>(userDto);
                return _mapper.Map<UserDto>(await _repo.Save(user));
            }
            return await Execute(UpdateUser);
        }


        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var userId = JwtMetaData.UserId;

            async Task<UserDto> GetUser()
            {
                var userdto = await _repo.Get(userId);
                return _mapper.Map<UserDto>(userdto);
            }

            return await Execute(GetUser);
        }

        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> Delete()
        {
            var userId = JwtMetaData.UserId;

            async Task DeleteUser()
            {
                await _repo.Delete(userId);

                //TODO delete an associated cook if there is one
            }

            return await Execute(DeleteUser);
        }
    }
}
