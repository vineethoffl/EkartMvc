using EkartMvc.Models.Models;
using EkartMvc.Models.ViewModels;
using EkartMvc.Repositories.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EkartMvc.Controllers
{
    // UserController.cs
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the username is already taken
                    if (_userRepository.IsUsernameTaken(user.Username))
                    {
                        ModelState.AddModelError("Username", "Username is already taken.");
                        return View(user);
                    }

                    // Check if the email is already registered
                    if (_userRepository.IsEmailTaken(user.Email))
                    {
                        ModelState.AddModelError("Email", "Email is already registered.");
                        return View(user);
                    }
                    //user.Password = HashPassword(user.Password);
                    _userRepository.RegisterUser(user);
                    // You may want to add additional logic, like redirecting to a login page
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                }
            }

            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult UserDashboard()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Authenticate user (you may need to implement this logic based on your requirements)
                User authenticatedUser = _userRepository.AuthenticateUser(model.Username, model.Password);

                if (authenticatedUser != null)
                {
                    // Redirect to the home page or some other page upon successful login
                    //return View(authenticatedUser);
                    return RedirectToAction("UserDashboard", "User");
                }
                else
                {
                    // Add a model error for unsuccessful login attempt
                    ModelState.AddModelError(string.Empty, "Invalid username or password");
                }
            }

            // If the model is not valid or login is unsuccessful, return the view with errors
            return View(model);
        }

        //    // Implement other actions as needed
        //}

        //public class UserController : Controller
        //{
        //    private readonly IUserRepository userRepository;
        //    private readonly IAuthenticationService authenticationService;

        //    public UserController(IUserRepository userRepository, IAuthenticationService authenticationService)
        //    {
        //        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        //        //this.authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        //    }

        //    public IActionResult Register()
        //    {
        //        return View();
        //    }

        //    //[HttpPost]
        //    //[ValidateAntiForgeryToken]
        //    //public IActionResult Register(UserViewModel model)
        //    //{
        //    //    try
        //    //    {
        //    //        if (ModelState.IsValid)
        //    //        {
        //    //            // Check if the username is already taken
        //    //            if (userRepository.IsUsernameTaken(model.Username))
        //    //            {
        //    //                ModelState.AddModelError("Username", "Username is already taken.");
        //    //                return View(model);
        //    //            }

        //    //            // Check if the email is already registered
        //    //            if (userRepository.IsEmailTaken(model.Email))
        //    //            {
        //    //                ModelState.AddModelError("Email", "Email is already registered.");
        //    //                return View(model);
        //    //            }

        //    //            //// Hash the password before storing it
        //    //            string hashedPassword = HashPassword(model.Password);

        //    //            EkartMvc.Models.Models.User user = new EkartMvc.Models.Models.User
        //    //            {
        //    //                Username = model.Username,
        //    //                Password = hashedPassword,
        //    //                Email = model.Email
        //    //            };

        //    //            userRepository.RegisterUser(user);

        //    //            // Log the user in after successful registration
        //    //            authenticationService.SignIn(user);

        //    //            // Redirect to the home page or a welcome page
        //    //            return RedirectToAction("Index", "Home");
        //    //        }
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        // Log the exception or handle it appropriately
        //    //        ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
        //    //    }

        //    //    return View(model);
        //    //}

        //    public IActionResult Login()
        //    {
        //        return View();
        //    }

        //    //[HttpPost]
        //    //[ValidateAntiForgeryToken]
        //    //public IActionResult Login(LoginViewModel model)
        //    //{
        //    //    try
        //    //    {
        //    //        if (ModelState.IsValid)
        //    //        {
        //    //            // Validate user credentials
        //    //            EkartMvc.Models.Models.User user = userRepository.AuthenticateUser(model.Username, model.Password);

        //    //            if (user != null)
        //    //            {
        //    //                // Log the user in
        //    //                authenticationService.SignIn(user);

        //    //                // Redirect to the home page or a dashboard
        //    //                return RedirectToAction("Index", "Home");
        //    //            }

        //    //            // Invalid credentials
        //    //            ModelState.AddModelError(string.Empty, "Invalid username or password.");
        //    //        }
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        // Log the exception or handle it appropriately
        //    //        ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
        //    //    }

        //    //    return View(model);
        //    //}

        //    //public IActionResult Logout()
        //    //{
        //    //    authenticationService.SignOut();

        //    //    // Redirect to the home page or a login page
        //    //    return RedirectToAction("Index", "Home");
        //    //}

        //    public IActionResult Profile()
        //    {
        //        // Get the currently logged-in user
        //        EkartMvc.Models.Models.User user = authenticationService.GetAuthenticatedUser();

        //        if (user != null)
        //        {
        //            // Display user profile information
        //            return View(user);
        //        }

        //        // Redirect to the login page if the user is not authenticated
        //        return RedirectToAction("Login");
        //    }

        //    private string HashPassword(string password)
        //    {
        //        // Use a secure password hashing algorithm like BCrypt or Argon2
        //        // For simplicity, this example uses a basic hash function
        //        using (SHA256 sha256 = SHA256.Create())
        //        {
        //            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        //            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        //        }
        //    }

        //    // Other actions for login, logout, profile, etc.

        //    // Implement other actions as needed
    }
}
