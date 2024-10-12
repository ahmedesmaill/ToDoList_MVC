using Microsoft.AspNetCore.Mvc;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class Todoitems : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        public IActionResult Index()
        {
            ViewBag.UserName = Request.Cookies["UserName"];
            var items = dbContext.Items.ToList();
            return View(model: items);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string Name)
        {

           CookieOptions options=new CookieOptions();
            options.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("UserName", $"{Name}", options)  ;
            return RedirectToAction(nameof(Index));
        }
        public IActionResult CreateNew()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateNew(Item item,IFormFile FileUrl)
        {
            if (FileUrl.Length > 0) 
            {
             var fileName = FileUrl.FileName;
                var filePath=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Pdfs", fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                FileUrl.CopyTo(stream);
            }

            item.FileUrl = fileName;

            //return FileUrl.FileName;
            }
            dbContext.Items.Add(item);
            dbContext.SaveChanges();
            TempData["Success"] = $"{item.Title} Created Successfully";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int ItemId)
        {

            var item = dbContext.Items.Find(ItemId);

            
            return View(model:item);


        }
        [HttpPost]
        public IActionResult Edit(Item item)
        {
            dbContext.Items.Update(item);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));


        }
        public IActionResult Delete(int ItemId)
        {
            dbContext.Items.Remove(new() { Id = ItemId });
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
