﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OOPS.BLL.Abstract;
using OOPS.WebUI.Models;

namespace OOPS.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPermitService _permitService;
        public HomeController(IPermitService permitService)
        {
            _permitService = permitService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
