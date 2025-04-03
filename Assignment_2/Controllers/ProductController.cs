using Assignment_2.Data;
using Assignment_2.Models;
using Assignment_2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Assignment_2.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductController(AppDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            this._context = context;
            this._hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            ProductsViewModel productsViewModel = new ProductsViewModel();
            productsViewModel.products = _context.Products.Include(c => c.Category).ToList();
            return View(productsViewModel);
        }

        public IActionResult CreateForm()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        
        public IActionResult CreateProduct(Product product)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var fileName = string.Empty;
                    if (product.ClientFile != null)
                    {
                        string myUpload = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                        fileName = product.ClientFile.FileName;
                        string fullPath = Path.Combine(myUpload, fileName);
                        product.ClientFile.CopyTo(new FileStream(fullPath, FileMode.Create));
                        product.Img = fileName;
                    }
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the product: " + ex.Message);
                }
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View("CreateForm", product);
        }

        public IActionResult EditForm(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            ViewBag.categories = _context.Categories.ToList();
            return View(product);
        }

        public IActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var productToUpdate = _context.Products.FirstOrDefault(p => p.Id == product.Id);
                    if (productToUpdate != null)
                    {
                        productToUpdate.Name = product.Name;
                        productToUpdate.Price = product.Price;
                        productToUpdate.Description = product.Description;
                        if (product.ClientFile != null)
                        {
                            string myUpload = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                            string fullPath = Path.Combine(myUpload, product.ClientFile.FileName);
                            product.ClientFile.CopyTo(new FileStream(fullPath, FileMode.Create));
                            productToUpdate.Img = product.ClientFile.FileName;
                        }
                        productToUpdate.CategoryId = product.CategoryId;
                        _context.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the product: " + ex.Message);
                }
            }
            ViewBag.categories = _context.Categories.ToList();
            return View("EditForm", product);
        }

        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if(product.Img != null)
            {
                var ImgPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", product.Img);
                System.IO.File.Delete(ImgPath);
            }
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            ViewBag.Product = _context.Products.FirstOrDefault(p => p.Id == id);
            return View();
        }
    }
}
