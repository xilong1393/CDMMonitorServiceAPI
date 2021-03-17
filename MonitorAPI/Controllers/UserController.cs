using MonitorAPI.Model;
using MonitorAPI.Models;
using MonitorAPI.Service;
using MonitorAPI.Util;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace MonitorAPI.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetUserLogin(UserLoginForm userLoginForm) {
            try
            {
                AuthService.AuthServiceSoapClient client = new AuthService.AuthServiceSoapClient();
                string response = client.AuthenticateEMPLID(userLoginForm.UserName, userLoginForm.Password, "winformDesktopApp");
                if (Helper.IsNumeric(response))
                {
                    UserService service = ServiceFactory.UserService;
                    LogService logSservice = ServiceFactory.LogService;
                    User user = service.UserLogin(userLoginForm.UserName);
                    if (user != null)
                    {
                        UserLoginLog userLoginLog = new UserLoginLog();
                        userLoginLog.SessionID = userLoginForm.SessionID;
                        userLoginLog.UserID = user.UserID;
                        userLoginLog.LoginTime = DateTime.Now;
                        userLoginLog.LoginIP = userLoginForm.LoginIP;
                        logSservice.LogUserLogin(userLoginLog);
                        return Ok(user);
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                LogHelper.GetLogger().Error(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }
    }
}