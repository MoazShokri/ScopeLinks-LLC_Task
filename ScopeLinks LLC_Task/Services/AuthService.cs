

namespace ScopeLinks_LLC_Task.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly JWT _jwt;

		public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
		{
			this._userManager = userManager;
			this._roleManager = roleManager;
			this._jwt = jwt.Value;
		}
		public async Task<AuthModel> RegisterAsync(RegisterModel model)
		{
			if (await _userManager.FindByEmailAsync(model.Email) is not null)
				return new AuthModel { Message = "Email is already registered!" };

			if (await _userManager.FindByNameAsync(model.UserName) is not null)
				return new AuthModel { Message = "Username is already registered!" };

			if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == model.PhoneNumber))
				return new AuthModel { Message = "Phone number is already registered!" };

			byte[] profilePictureData = null; // Initialize as null

			if (model.ProfilePicture != null)
			{
				// Process and store the uploaded profile picture data
				using (var memoryStream = new MemoryStream())
				{
					await model.ProfilePicture.CopyToAsync(memoryStream);
					profilePictureData = memoryStream.ToArray();
				}
			}
			var user = new ApplicationUser
			{
				UserName = model.UserName,
				Email = model.Email,
				FristName = model.FristName,
				LastName = model.LastName,
				PhoneNumber= model.PhoneNumber,
				ProfilePicture = profilePictureData

			};
			

			var result = await _userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
			{
				var errors = string.Empty;

				foreach (var error in result.Errors)
					errors += $"{error.Description},";

				return new AuthModel { Message = errors };
			}

			await _userManager.AddToRoleAsync(user, Roles.User);
			var jwtSecurityToken = await CreateJwtToken(user);

			return new AuthModel
			{
				Email = user.Email,
				ExpiresOn = jwtSecurityToken.ValidTo,
				IsAuthenticated = true,
				Roles = new List<string> { Roles.User },
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
				UserName = user.UserName,
				ProfilePicture = user.ProfilePicture
				
			};
		}
		public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
		{
			var authModel = new AuthModel();

			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
			{
				authModel.Message = "Email or Password is incorrect!";
				return authModel;
			}

			var jwtSecurityToken = await CreateJwtToken(user);
			var rolesList = await _userManager.GetRolesAsync(user);

			authModel.IsAuthenticated = true;
			authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
			authModel.Email = user.Email;
			authModel.UserName = user.UserName;
			authModel.ExpiresOn = jwtSecurityToken.ValidTo;
			authModel.Roles = rolesList.ToList();
			authModel.ProfilePicture = user.ProfilePicture;

			return authModel;
		}

		public async Task<string> AddRoleAsync(AddRoleModel model)
		{
			var user = await _userManager.FindByIdAsync(model.UserId);

			if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
				return "Invalid user ID or Role";

			if (await _userManager.IsInRoleAsync(user, model.Role))
				return "User already assigned to this role";

			var result = await _userManager.AddToRoleAsync(user, model.Role);

			return result.Succeeded ? string.Empty : "Sonething went wrong";
		}

		private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = new List<Claim>();

			foreach (var role in roles)
				roleClaims.Add(new Claim("roles", role));

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("uid", user.Id)
			}
			.Union(userClaims)
			.Union(roleClaims);

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
			var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
			var jwtSecurityToken = new JwtSecurityToken(
				issuer: _jwt.Issuer,
				audience: _jwt.Audience,
				claims: claims,
				expires: DateTime.Now.AddDays(_jwt.DurationInDays),
				signingCredentials: signingCredentials);

			return jwtSecurityToken;
		}

		public string GetUserIdFromRequestHeader(HttpContext httpContext)
		{
			try
			{
				if (httpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
				{
					var token = authorizationHeader.ToString();
					var bearerPrefix = "Bearer ";

					if (token.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
					{
						token = token.Substring(bearerPrefix.Length);
						var jwtHandler = new JwtSecurityTokenHandler();

						var validationParameters = new TokenValidationParameters
						{
							ValidateIssuer = true,
							ValidIssuer = _jwt.Issuer,

							ValidateAudience = true,
							ValidAudience = _jwt.Audience,

							ValidateIssuerSigningKey = true,
							IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)),

							RequireExpirationTime = true
						};

						SecurityToken validatedToken;
						var claimsPrincipal = jwtHandler.ValidateToken(token, validationParameters, out validatedToken);

						if (claimsPrincipal == null)
						{
							return null;
						}

						var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "uid");

						if (userIdClaim == null)
						{
							return null;
						}

						var userId = userIdClaim.Value;

						return userId;
					}
				}
				return null;
			}
			catch (Exception)
			{
				return null;
			}
		}
		public string GetUserIdFromToken(HttpContext httpContext, out string CustomerId)
		{
			try
			{
				CustomerId = null;
				string token = string.Empty;
				if (httpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
				{
					token = authorizationHeader.ToString();
					var bearerPrefix = "Bearer ";

					if (token.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
					{
						token = token.Substring(bearerPrefix.Length);
					}
				}


				var jwtHandler = new JwtSecurityTokenHandler();

				var validationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = _jwt.Issuer,

					ValidateAudience = true,
					ValidAudience = _jwt.Audience,

					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)),

					RequireExpirationTime = true
				};

				SecurityToken validatedToken;
				var claimsPrincipal = jwtHandler.ValidateToken(token, validationParameters, out validatedToken);

				if (claimsPrincipal == null)
				{
					return null;
				}

				var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "uid");

				if (userIdClaim == null)
				{
					return null;
				}
				CustomerId = (claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "PCid")).Value;
				var userId = userIdClaim.Value;

				return userId;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
	
}
