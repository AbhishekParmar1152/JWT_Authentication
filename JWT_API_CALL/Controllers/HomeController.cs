using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using JWT_Authentication.Controllers;
using JWT_Authentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace JWT_API_CALL.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(UserRegistrationModel registration)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("https://localhost:44386/api/Account/register", content))
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        var userregistration = JsonConvert.DeserializeObject<Response>(apiResponse);
                        if (userregistration.Status == "Success")
                        {
                            TempData["Message"] = "Registration successfully..!";
                            return RedirectToAction("Login");
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message;
                return RedirectToAction("Login");
            }
            TempData["Message"] = "Some Problem with Registration";
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.alertmsgregistration = TempData["Message"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel login)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("https://localhost:44386/api/Account/login", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var userlogin = JsonConvert.DeserializeObject<Response>(apiResponse);
                        if (userlogin.Status == "Success")
                        {
                            HttpContext.Session.SetString("Token", userlogin.Token);
                            TempData["Message"] = "Login Successfully!";
                            UserRegistrationModel dTO = new UserRegistrationModel();
                            foreach (var item in (dynamic)userlogin.Data)
                            {
                                dTO = JsonConvert.DeserializeObject<UserRegistrationModel>(userlogin.Data.ToString());
                            }
                            return RedirectToAction("Dashboard", dTO);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message;
            }
            TempData["Message"] = "Some Problem with login..";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult RegistrationAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationAdmin(UserRegistrationModel registration)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:44386/api/Account/register-admin", content))
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        var userregistration = JsonConvert.DeserializeObject<Response>(apiResponse);
                        if (userregistration.Status == "Success")
                        {
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            TempData["Message"] = userregistration.Message;
                            return RedirectToAction("Login");
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message;
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> Dashboard(UserRegistrationModel response)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenType = "Bearer";
                    string token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json");
                    using (var result = await httpClient.PostAsync("https://localhost:44386/api/Account/Dashboard", content))
                    {
                        var apiResponse = await result.Content.ReadAsStringAsync();
                        var userregistration = JsonConvert.DeserializeObject<UserRegistrationModel>(apiResponse);
                        if (userregistration != null)
                        {
                            return View(userregistration);
                        }
                        return RedirectToAction("Login");
                    }
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message;
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes("admin:admin@123");
                    string val = System.Convert.ToBase64String(plainTextBytes);
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

                    using (var result = await httpClient.GetAsync("https://localhost:44330/api/Api/Deatils"))
                    {
                        List<StudentModel> model = new List<StudentModel>();
                        var apiResponse = await result.Content.ReadAsStringAsync();
                        model = JsonConvert.DeserializeObject<List<StudentModel>>(apiResponse);
                        if (model != null)
                        {
                            return View(model);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message;
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Formdata()
        {
            return View();
        }
    }
}

