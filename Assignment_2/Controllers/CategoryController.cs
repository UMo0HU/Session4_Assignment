using Assignment_2.Data;
using Assignment_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment_2.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            ViewData["Categories"] = _context.Categories.ToList();
            return View();
        }

        public IActionResult CreateForm()
        {
            return View();
        }

        public IActionResult CreateCategory(Category category)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the category: " + ex.Message);
                }
            }
            return View("CreateForm", category);
        }

        public IActionResult EditForm(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            return View(category);
        }

        public IActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var categoryToUpdate = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
                    if (categoryToUpdate != null)
                    {
                        categoryToUpdate.Name = category.Name;
                        categoryToUpdate.Description = category.Description;
                        _context.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the category: " + ex.Message);
                }
            }
            return View("EditForm", category);
        }

        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            ViewBag.Category = _context.Categories.FirstOrDefault(c => c.Id == id);
            return View();
        }
    }
}
