using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Project.DTO;
using Project.Models;
using PagedList;
namespace Project.Controllers
{
    public class ProductController : Controller
    {
        private readonly NorthWindContext _context;
        public ProductController(NorthWindContext context)
        {
            _context = context;
        }
        public ActionResult Home() {
            List<Product>  lastProducts = _context.Products.OrderByDescending(p => p.ProductId).Take(5).ToList();
            return View(lastProducts);
        }
        public IActionResult Index(string s, int? page)
        {
            var products = _context.Products.ToList(); // Lấy danh sách sản phẩm
             switch (s)
            {
                case "sortAbysid":
                    products = _context.Products.OrderBy(p => p.SupplierId).ToList();
                   break;
               case "sortDbysid":
                   products = _context.Products.OrderByDescending(p => p.SupplierId).ToList();
                   break;
               case "sortAbycid":
                    products = _context.Products.OrderBy(p => p.CategoryId).ToList();
                   break;
               case "sortDbycid":
                 products = _context.Products.OrderByDescending(p => p.CategoryId).ToList();
                   break;
             case "sortAByPrice":
                    products = _context.Products.OrderBy(p => p.UnitPrice).ToList();
                    break;
               case "sortDByPrice":
                   products = _context.Products.OrderByDescending(p => p.UnitPrice).ToList();
                   break;
                default:
                   products = _context.Products.ToList();
                   break;
            }
            int pageSize = 10; // Số sản phẩm trên mỗi trang
            int pageNumber = (page ?? 1); // Trang mặc định

            // Tính toán số trang và sản phẩm trên mỗi trang
            int totalProducts = products.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            // Lấy danh sách sản phẩm cho trang hiện tại
            var pagedProducts = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.TotalPages = totalPages; // Truyền thông tin phân trang đến View
            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.Sort = s;
            return View(pagedProducts);
        }
        public IActionResult Order(int id)
        {
            Product p = _context.Products.FirstOrDefault(p => p.ProductId == id);
            return View(p); 
        }
       
        public IActionResult ListByCategory(string categoryId)
        {
        
            List<Product> product = new List<Product>();
            if (!string.IsNullOrEmpty(categoryId))
            {
                if(int.TryParse(categoryId, out int cid))
                {
                    product =_context.Products.Where(p => p.CategoryId  == cid).ToList();
                }
            }
            else
            {
                product = _context.Products.ToList();
            }
            ViewBag.CategoryId = categoryId;
            return View(product);

        }
        public IActionResult Detail(int id)
        {
            Product product = _context.Products.FirstOrDefault(x => x.ProductId == id);
            return View(product);
        }
        public IActionResult DoDelete(int id)
        {
            Product p = _context.Products.FirstOrDefault(x => x.ProductId == id);
            List<OrderDetail> od = _context.OrderDetails.Where(x => x.ProductId == id).ToList();
            if (p != null && od != null)
            {
                foreach (var orderDetail in od)
                {
                    _context.OrderDetails.Remove(orderDetail);
                }
                _context.Products.Remove(p);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");

        }
        public IActionResult Update(int id)
        {
            Product p = _context.Products.FirstOrDefault(p => p.ProductId == id);
         
            return View(p);
        }
        [HttpPost]
        public IActionResult DoUpdate([FromForm] ProductDTO product)
        {
            Product p = _context.Products.FirstOrDefault(p => p.ProductId == product.Id);
            if (p != null)
            {
                p.ProductName = product.name;
                p.UnitPrice = (decimal)product.price;
                p.SupplierId = product.supplierId;
                p.CategoryId = product.categoryId;
                p.QuantityPerUnit = product.quantityPerUnit;
                _context.Products.Update(p);
                _context.SaveChanges();
               return RedirectToAction("Detail", new { Id = p.ProductId });
            }
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create1([FromForm] ProductDTO product)
        {
            Product p = new Product();
            p.ProductName = product.name;
            p.UnitPrice = (decimal)product.price;
            p.SupplierId = product.supplierId;
            p.CategoryId = product.categoryId;
            p.QuantityPerUnit = product.quantityPerUnit;
            _context.Products.Add(p);
            _context.SaveChanges();
            return RedirectToAction("Index");   
        }
        public IActionResult SortBySid()
        {
            List<Product> products = _context.Products.OrderByDescending(p => p.SupplierId).ToList();
            return View(products);
        }
        public IActionResult Search(string search)
        {
            var products = new List<Product>();

            if (!string.IsNullOrEmpty(search))
            {
                if (int.TryParse(search, out int id))
                {
                    products = _context.Products.Where(p => p.ProductId == id).ToList();
                    if (products.Count == 0)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    products = _context.Products.Where(p => p.ProductName.StartsWith(search)).ToList();
                    if (products.Count == 0)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
            return View(products);
        }
    }
}
