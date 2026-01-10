using AutoMapper;
using Core.Identity;
using Core.Interfaces;
using E_commerce.Api.Dtos;
using E_commerce.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,ITokenService tokenService,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized();
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if(!result.Succeeded) return Unauthorized();
            return new UserDto
            {
                Email=user.Email,
                Token=_tokenService.CreateToken(user),
                DisplayName=user.DisplayName
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                DisplayName = registerDto.Displayname,
                Email=registerDto.Email,
                UserName=registerDto.Email
            
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return BadRequest();
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };

        }

        [Authorize]
        [HttpPost("getCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimPrincipal(User);
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };
        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> checkMailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserByClimsProncipleWithAddress(User);
            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FindUserByClimsProncipleWithAddress(User);
            user.Address = _mapper.Map<AddressDto, Address>(address);
            var resuilt = await _userManager.UpdateAsync(user);
            if (resuilt.Succeeded)
                return Ok(_mapper.Map<Address, AddressDto>(user.Address));

            return BadRequest("Problem updating the user address");
        }
    }
}
