using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Geekbank.Web.Controllers
{
    public class AdminController : Controller
    {
        private IConfigurationRoot _config;

        public AdminController(IConfigurationRoot config)
        {
            _config = config;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowConfig(string dumpKey)
        {
            if (dumpKey != _config["DumpConfigKey"]) return Unauthorized();
            return View(_config.AsEnumerable());
        }
    }
}
