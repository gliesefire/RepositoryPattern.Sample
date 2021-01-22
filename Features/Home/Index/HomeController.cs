using Microsoft.AspNetCore.Mvc;
using System;

namespace RepositoryPattern.Sample.Features.Home.Index
{
    public class HomeController : Controller
    {
        private readonly DbContext _context;

        public HomeController(DbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task<IActionResult> IndexAsync()
        {
            var isSuccess = await _context.Homes.AddAsync(DateTime.Now);
            isSuccess = isSuccess && await _context.Errors.AddAsync("Hello World");

            if(isSuccess)
                await _context.SaveChanges();
            return View();
        }
    }
}
