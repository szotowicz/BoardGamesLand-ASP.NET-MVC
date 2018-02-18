using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class UserController : Controller
    {
        // GET: User

        [Route("User/{userNick}")]
        public ActionResult UserProfile(string userNick)
        {
            try
            {
                UserProfile userProfile = new UserProfile();
                using (var db = new BoardGamesDBEntities())
                {
                    // set user whom someone visits
                    Users user = db.Users.FirstOrDefault(u => u.Login == userNick);
                    if (user == null)
                    {
                        // When user is not exist
                        return RedirectToAction("Index", "Home");
                    }

                    userProfile.UserWhomSomeoneVisits = new UserAccount()
                    {
                        Key = user.Id,
                        Email = user.Email,
                        Login = user.Login,
                        Name = user.Name,
                        Surname = user.Surname,
                        Sex = user.Sex,
                        Birthday = user.Birthday,
                        Avatar = user.Avatar
                    };

                    // set privacy policy
                    List<PrivacyRules> userRulesList = new List<PrivacyRules>();
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
                    userProfile.UserPrivacyPolicy = userRulesList;

                    // set friend list
                    List<int> userFriendsIndex = new List<int>();
                    foreach (Friendship friend in db.Friendship.Where(f => f.Friend1 == user.Id || f.Friend2 == user.Id).ToList())
                    {
                        if (friend.Friend1 != user.Id)
                        {
                            userFriendsIndex.Add(friend.Friend1);
                        }
                        else
                        {
                            userFriendsIndex.Add(friend.Friend2);
                        }
                    }
                    userProfile.UserFriendsIndex = userFriendsIndex;
                }

                if (userProfile != null)
                {
                    return View(userProfile);
                }
            }
            catch(Exception)
            { }

            return RedirectToAction("Index", "Home");
        }
    }
}