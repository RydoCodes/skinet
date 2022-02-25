using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.IdentityDTOs.Dtos;
using API.Errors;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Dtos.IdentityDTOs;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Core.Interfaces.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using API.Extensions;
using AutoMapper;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenservice;
        private readonly AppIdentityDbContext _identitycontext;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenservice, AppIdentityDbContext identitycontext, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenservice = tokenservice;
            _identitycontext = identitycontext; // Not Required. Just Injecte for test
            _mapper = mapper;
        }

       // URL : /api/account will hit this
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            // Way 1
            Claim emalclaim = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            string email = emalclaim.Value;
            var user1 = await _userManager.FindByEmailAsync(email);

            // Way 2 IS ACTIVE  
            AppUser user = await _userManager.FindByEmailFromClaimsPrincipal(HttpContext.User); // Using ExtensionMethod : FindByEmailFromClaimsPrincipal

            // Common 
            return new UserDto
            {
                Email = user.Email,
                //Token = "This will be a token",
                Token = _tokenservice.CreateToken(user), // Token is generated for a user so we need to pass a user
                DisplayName = user.DisplayName
            };
        }

        [HttpGet("emailexists")] 
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null; 
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetAddress()
        {
            //Way 1 : FAIL
            ClaimsPrincipal loggedinuser = HttpContext.User;
            Claim emalclaim = loggedinuser?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            string email = emalclaim.Value;
            AppUser user1 = await _userManager.FindByEmailAsync(email);

            //Way 2
            AppUser user2 = await _userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email);
            Address useraddress2 = user2.Address;

            //Way 3
            AppUser user3 = await _identitycontext.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email);
            Address useraddress3 = user3.Address;

            // Way 4 is ACTIVE
            AppUser user4 = await _userManager.FindByEmailWithAddressAsync(HttpContext.User);
            Address useraddress4 = user4.Address;

            AddressDto addressdto = _mapper.Map<Address, AddressDto>(useraddress4);

            return addressdto;
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressdto)
        {
            AppUser user = await _userManager.FindByEmailWithAddressAsync(HttpContext.User);

            user.Address = _mapper.Map<AddressDto, Address>(addressdto);

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                AddressDto addressDto = _mapper.Map<Address, AddressDto>(user.Address);
                return Ok(addressDto);
            }
            else
            {
                return BadRequest("Probelem Updating the user");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto logindto)
        {
            AppUser user = await _userManager.FindByEmailAsync(logindto.Email);
            if (user == null)
            {
                return Unauthorized(new ApiResponse(401));
            }

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, logindto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new ApiResponse(401));
            }

            return new UserDto
            {
                Email = user.Email,
                //Token = "This will be a token",
                Token = _tokenservice.CreateToken(user), // Token is generated for a user so we need to pass a user
                DisplayName = user.DisplayName
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            bool response = CheckEmailExistsAsync(registerDto.Email).Result.Value;
            if(response)
            {
                var validationresponse = new ApiValidationErrorResponse
                {
                    Errors = new[]
                    { "Email Address is already in use"
                    }
                };

                //Way 1
               //return BadRequest(validationresponse);

                //Way 2
                return new BadRequestObjectResult(validationresponse);

                // Bad Request and BadRequestObjectResult produces same response.
            }

            AppUser user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded) // fails when user enters a weak password
            {
                return  BadRequest(new ApiResponse(400));
            }

            return new UserDto
            {
                Email = user.Email,
                //Token = "This will be a token",
                Token = _tokenservice.CreateToken(user),
                DisplayName = user.DisplayName
            };

            // Default accepts No Userame but fails in validation returns 400 Bad Request
            // Default does not accept No 
        }
    }
}