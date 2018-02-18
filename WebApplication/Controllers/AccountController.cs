using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            if (Session["loggedUser"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.AdditionalCommunicat = "";
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            try
            {
                if (user.Login != null && user.Password != null)
                {
                    using (var db = new BoardGamesDBEntities())
                    {
                        Users usr = db.Users.FirstOrDefault(u => (user.Login == u.Login || user.Login == u.Email)
                            && user.Password == u.Password);
                        if (usr != null)
                        {
                            // User logged
                            SessionControl sessionControl = new SessionControl()
                            {
                                Id = usr.Id,
                                Email = usr.Email,
                                Login = usr.Login
                            };
                            Session["loggedUser"] = sessionControl;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            // User not recognized
                            ViewBag.AdditionalCommunicat = "Podano niepoprawny identyfikator lub hasło.";
                        }
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.AdditionalCommunicat = "Wystąpiły problemy techniczne. Spróbuj ponownie.";
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Settings()
        {
            if (Session["loggedUser"] != null)
            {
                SessionControl sessionControl = (SessionControl)Session["loggedUser"];
                List<PrivacyRules> userRulesList = new List<PrivacyRules>();
                UserAccount userAccount = null;

                using (var db = new BoardGamesDBEntities())
                {
                    Users user = db.Users.FirstOrDefault(u => u.Id == sessionControl.Id);
                    userAccount = new UserAccount()
                    {
                        Key = user.Id,
                        Email = user.Email,
                        Login = user.Login,     // do not pass password - for safety
                        Name = user.Name,
                        Surname = user.Surname,
                        Sex = user.Sex,
                        Birthday = user.Birthday,
                        Avatar = user.Avatar
                    };

                    foreach (UsersPrivacyPolicy rule in db.UsersPrivacyPolicy.Where(p => p.UserId == user.Id).ToList())
                    {
                        userRulesList.Add(
                            new PrivacyRules()
                            {
                                RuleDescription = db.UsersPrivacyPolicyList.FirstOrDefault(p => p.Id == rule.RuleId).Description,
                                RuleLevel = rule.RuleLevel
                            }
                            );
                    }
                }

                // Create final model
                UserProfileSettings userProfileSettings = new UserProfileSettings()
                {
                    UserInformation = userAccount,
                    UserPrivacyPolicy = userRulesList
                };

                return View(userProfileSettings);
            }

            // When user is not logged
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Registration()
        {
            if (Session["loggedUser"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Registration(UserAccount user)
        {
            try
            {
                if (user.Email != null && user.Password != null && user.Login != null
                    && user.Name != null && user.Surname != null && user.Birthday != null && user.RegulationsAreAccepted == true)
                {
                    using (var db = new BoardGamesDBEntities())
                    {
                        Users usr = null;
                        usr = db.Users.FirstOrDefault(u => user.Login == u.Login);
                        if (usr != null)
                        {
                            // Login is not available
                            ViewBag.AdditionalCommunicat = "Wybrana nazwa użytkownika jest zajęta.";
                            return View();
                        }
                        usr = db.Users.FirstOrDefault(u => user.Email == u.Email);
                        if (usr != null)
                        {
                            // Email is not available
                            ViewBag.AdditionalCommunicat = "Podany adres e-mail jest już używany przez kogoś innego.";
                            return View();
                        }

                        // Add new user
                        Users newUser = new Users()
                        {
                            Email = user.Email,
                            Login = user.Login,
                            Password = user.Password,
                            Name = user.Name,
                            Surname = user.Surname,
                            Sex = user.Sex,
                            Birthday = user.Birthday,
                            SecurityLevel = 1,
                            Avatar = null
                        };

                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                db.Users.Add(newUser);
                                db.SaveChanges();
                                transaction.Commit();
                            }
                            catch (Exception)
                            {

                                transaction.Rollback();
                                ViewBag.AdditionalCommunicat = "Wystąpiły problemy techniczne. Spróbuj ponownie.";
                            }
                        }

                        // Add default privacy policy for new user
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                usr = db.Users.FirstOrDefault(u => user.Login == u.Login);
                                if (usr != null)
                                {
                                    int privacyPolicyListCount = db.UsersPrivacyPolicyList.Count();
                                    for (int i = 1; i <= privacyPolicyListCount; i++)
                                    {
                                        UsersPrivacyPolicy defaultPolicy = new UsersPrivacyPolicy()
                                        {
                                            UserId = usr.Id,
                                            RuleId = i,
                                            RuleLevel = 2
                                        };

                                        db.UsersPrivacyPolicy.Add(defaultPolicy);
                                    }
                                    db.SaveChanges();
                                    transaction.Commit();
                                }

                                return View("~/Views/Account/RegistrationSuccess.cshtml");
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                ViewBag.AdditionalCommunicat = "Wystąpiły problemy techniczne. Spróbuj ponownie.";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.AdditionalCommunicat = "Wystąpiły problemy techniczne. Spróbuj ponownie.";
            }

            return View();
        }

        public ActionResult ChangePassword()
        {
            if (Session["loggedUser"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.AdditionalCommunicat = "";
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            if (Session["loggedUser"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                if (changePassword.CurrentPassword != null && changePassword.NewPassword != null && changePassword.NewPasswordCopy != null)
                {
                    if (changePassword.NewPassword == changePassword.NewPasswordCopy)
                    {
                        using (var db = new BoardGamesDBEntities())
                        {
                            SessionControl sessionControl = (SessionControl)Session["loggedUser"];
                            Users user = db.Users.FirstOrDefault(u => u.Id == sessionControl.Id);

                            if (changePassword.CurrentPassword == user.Password)
                            {
                                using (var transaction = db.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        Users currentUser = db.Users.FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);
                                        if (currentUser != null)
                                        {
                                            currentUser.Password = changePassword.NewPassword;
                                            db.SaveChanges();
                                            transaction.Commit();

                                            // Password has been changed
                                            Session.Clear();
                                            return View("~/Views/Account/ChangePasswordSuccess.cshtml");
                                        }
                                        else
                                        {
                                            ViewBag.AdditionalCommunicat = "Wystąpiły problemy techniczne. Spróbuj ponownie.";
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        transaction.Rollback();
                                        ViewBag.AdditionalCommunicat = "Wystąpiły problemy techniczne. Spróbuj ponownie.";
                                    }
                                }
                            }
                            else
                            {
                                ViewBag.AdditionalCommunicat = "Podano nieprawidłowe hasło.";
                            }
                        }
                    }
                    else
                    {
                        ViewBag.AdditionalCommunicat = "Powtórz poprawnie nowe hasło.";
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.AdditionalCommunicat = "Wystąpiły problemy techniczne. Spróbuj ponownie.";
            }

            return View();
        }

        public ActionResult Regulations()
        {
            return View();
        }
    }
}