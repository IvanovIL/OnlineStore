using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OnlineStore.Domain.Entities;


namespace OnlineStore.AppServices.Authentication.Services
{
	/// <summary>
	/// Cервис аутентификации
	/// </summary>
	public sealed class AuthenticationService : IAuthenticationService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

        /// <inheritdoc/>
        public AuthenticationService(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
             IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
			_signInManager = signInManager;
			_httpContextAccessor = httpContextAccessor;
        }


		/// <inheritdoc/>
		public  Task<IdentityResult> RegisterAsync(string email, string password, CancellationToken cancellation)
		{
			var user = new ApplicationUser
			{
				UserName = email,
				Email = email
			};
			
			return _userManager.CreateAsync(user, password);
        }


		/// <inheritdoc/>
		public async Task<bool> SignInAsync(string email, string password, CancellationToken cancellation)
		{
			var user = await _userManager.FindByEmailAsync(email)
				?? throw new UnauthorizedAccessException("Такого пользователя не существует");


			var isPasswordMatched = await _userManager.CheckPasswordAsync(user, password);

			if (isPasswordMatched)
			{
				await _signInManager.SignInAsync(user, isPersistent: true);
				return true;
			}
			else
			{
				//throw new UnauthorizedAccessException("Неправильный email или пароль");
				return false;
			}
		}


		/// <inheritdoc/>
		public Task SignOutAsync(CancellationToken cancellation)
		{
			return _signInManager.SignOutAsync();
		}
	}
}
