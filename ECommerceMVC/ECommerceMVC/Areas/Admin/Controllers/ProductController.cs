using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using ECommerceMVC.DataAccess.Data;
using ECommerceMVC.Domain.Entities;
using ECommerceMVC.Infrastructure.Services.Cloudinary;
using ECommerceMVC.UI.Areas.Admin.ViewModels.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECommerceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ProductController : BaseController
    {
        public ProductController(SignInManager<DbUser> signInManager, UserManager<DbUser> userManager, ICloudinaryService cloudinaryService, RoleManager<IdentityRole> roleManager, INotyfService notyf, IMapper mapper, ECommerceContext context) : base(signInManager, userManager, cloudinaryService, roleManager, notyf, mapper, context)
        {
        }


        private async Task<SelectList> GetCategorySelectListAsync()
        {
            var categories = await _context.DbCategories.ToListAsync();

            // Add a null option to the beginning of the list
            categories.Insert(0, new DbCategory { Id = 0, Name = "No parent category" });

            // Create SelectList with categories
            return new SelectList(categories, "Id", "Name");
        }

        private async Task<SelectList> GetGroupSelectListAsync()
        {
            var groups = await _context.DbGroups.ToListAsync();

            // Add a null option to the beginning of the list
            groups.Insert(0, new DbGroup { Id = 0, Name = "No parent category" });

            // Create SelectList with categories
            return new SelectList(groups, "Id", "Name");
        }



        private async Task SetViewBagsAsync()
        {
            ViewBag.CategoryList = await GetCategorySelectListAsync();
            ViewBag.GroupList = await GetGroupSelectListAsync();
        }


        private async Task<DbProduct?> GetProductByIdAsync(int? id)
        {                 
            return await _context.DbProducts
                       .Include(p => p.DbProductImages).ThenInclude(pi => pi.IdImageNavigation)
                       .Include(p => p.DbProductColors).ThenInclude(pc => pc.IdColorNavigation)
                       .Include(p => p.DbProductSizes).ThenInclude(ps => ps.IdSizeNavigation)
                       .FirstOrDefaultAsync(p => p.Id == id);     
        }



        // GET: Admin/Product
        public async Task<IActionResult> Index()
        {
            var db = await _context.DbProducts
                .Include(p => p.DbProductImages).ThenInclude(pi => pi.IdImageNavigation)
                .Include(p => p.DbProductColors).ThenInclude(pc => pc.IdColorNavigation)
                .Include(p => p.DbProductSizes).ThenInclude(ps => ps.IdSizeNavigation)
                .ToListAsync();
            await SetViewBagsAsync();
            return View(db);
        }


        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var db = await _context.DbProducts
                .Include(p => p.DbProductImages).ThenInclude(pi => pi.IdImageNavigation)
                .Include(p => p.DbProductColors).ThenInclude(pc => pc.IdColorNavigation)
                .Include(p => p.DbProductSizes).ThenInclude(ps => ps.IdSizeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (db == null)
            {
                return NotFound();
            }
            await SetViewBagsAsync();
            return View(db);
        }


        // GET: Admin/Product/Create
        public async Task<IActionResult> Create()
        {

            await SetViewBagsAsync();
            return View();
        }



        // POST: Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {

           

            if (!ModelState.IsValid)
            {
                await SetViewBagsAsync();
                return View(model);

            }


            // Kiểm tra giá trị của IdCategory
            if (model.IdCategory == 0)
            {
                ModelState.AddModelError("IdCategory", "IdCategory must have non-zero values.");
                await SetViewBagsAsync();
                return View(model);
            }

            // Kiểm tra giá trị của IdGroup 
            if (model.IdGroup == 0)
            {
                ModelState.AddModelError("IdGroup", "IdGroup must have non-zero values.");
                await SetViewBagsAsync();
                return View(model);
            }


            // Kiểm tra giá trị của IdCategory
            if (model.Images == null || !model.Images.Any())
            {
                ModelState.AddModelError("Images", "Please select at least one image.");
                await SetViewBagsAsync();
                return View(model);
            }


            var product = _mapper.Map<ProductCreateVM, DbProduct>(model);

            // Thêm sản phẩm vào cơ sở dữ liệu
            await _context.DbProducts.AddAsync(product);
            await _context.SaveChangesAsync();

            // Lưu Colors
            foreach (var color in model.Colors)
            {

                var dbColor = new DbColor
                {
                    Name = color
                };

                await _context.DbColors.AddAsync(dbColor);
                await _context.SaveChangesAsync();


                var productColor = new DbProductColor
                {
                    IdProduct = product.Id,
                    IdColor = dbColor.Id
                };
                await _context.DbProductColors.AddAsync(productColor);
                await _context.SaveChangesAsync();
            }

            // Lưu Images
            foreach (var imageFile in model.Images)
            {
                // Upload image to Cloudinary and get URL
                var result = await _cloudinaryService.UploadFileAsync(imageFile, "ECommerceProducts");

                var image = new DbImage
                {
                    IdPublic = result.Id,
                    Url = result.Url
                };

                await _context.DbImages.AddAsync(image);
                await _context.SaveChangesAsync();

                var productImage = new DbProductImage
                {
                    IdProduct = product.Id,
                    IdImage = image.Id
                };
                await _context.DbProductImages.AddAsync(productImage);
                await _context.SaveChangesAsync();
            }

            // Lưu Sizes
            foreach (var size in model.Sizes)
            {

                var dbSize = new DbSize
                {
                    Name = size
                };

                await _context.DbSizes.AddAsync(dbSize);
                await _context.SaveChangesAsync();

                var productSize = new DbProductSize
                {
                    IdProduct = product.Id,
                    IdSize = dbSize.Id
                };
                await _context.DbProductSizes.AddAsync(productSize);
                await _context.SaveChangesAsync();
            }

            _notyf.Success("Create products successfully.");
            return RedirectToAction(nameof(Index));
        }



        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.DbProducts
             .Include(p => p.DbProductImages).ThenInclude(pi => pi.IdImageNavigation)
             .Include(p => p.DbProductColors).ThenInclude(pc => pc.IdColorNavigation)
             .Include(p => p.DbProductSizes).ThenInclude(ps => ps.IdSizeNavigation)
             .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Mapping DbProduct thành ProductCreateVM để hiển thị trong form
            var model = _mapper.Map<DbProduct, ProductCreateVM>(product);

            // Lấy danh sách màu sắc và kích cỡ từ DbProductColors và DbProductSizes
            model.Colors = product.DbProductColors.Select(pc => pc.IdColorNavigation.Name).ToList();
            model.Sizes = product.DbProductSizes.Select(ps => ps.IdSizeNavigation.Name).ToList();


            // Lấy danh sách ảnh từ DbProductImages và ánh xạ sang ViewModel tương ứng
            model.UrlImages = product.DbProductImages.Select(pi => pi.IdImageNavigation.Url).ToList();

            // Truyền dữ liệu cần thiết đến view
            await SetViewBagsAsync(); // Nếu cần set các ViewBag khác
            return View(model);
        }



        // POST: Admin/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductCreateVM model)
        {

            var product = await GetProductByIdAsync(model.Id);

            if (product == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                if (model.Images == null || !model.Images.Any()) model.UrlImages = product.DbProductImages.Select(pi => pi.IdImageNavigation.Url).ToList();
                                  
                await SetViewBagsAsync();
                return View(model);
            }


            // Kiểm tra giá trị của IdCategory
            if (model.IdCategory == 0)
            {
                if (model.Images == null || !model.Images.Any()) model.UrlImages = product.DbProductImages.Select(pi => pi.IdImageNavigation.Url).ToList();
                ModelState.AddModelError("IdCategory", "IdCategory must have non-zero values.");
                await SetViewBagsAsync();
                return View(model);
            }

            // Kiểm tra giá trị của IdGroup 
            if (model.IdGroup == 0)
            {
                if (model.Images == null || !model.Images.Any()) model.UrlImages = product.DbProductImages.Select(pi => pi.IdImageNavigation.Url).ToList();
                ModelState.AddModelError("IdGroup", "IdGroup must have non-zero values.");
                await SetViewBagsAsync();
                return View(model);
            }



            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                   
                    product.IdCategory = model.IdCategory;
                    product.IdGroup = model.IdGroup;
                    product.Name = model.Name;
                    product.Price = model.Price;                   
                    product.Quantity = model.Quantity;
                    product.CreatedAt = model.CreatedAt;
                    product.RemovedAt  = model.RemovedAt;
                    product.Description = model.Description;


                    _context.Update(product);
                    

                    // Cập nhật Colors
                    var existingColors = product.DbProductColors.ToList();
                    var newColors = model.Colors.Except(existingColors.Select(ec => ec.IdColorNavigation.Name)).ToList();
                    foreach (var color in newColors)
                    {
                        var dbColor = new DbColor { Name = color };
                        await _context.DbColors.AddAsync(dbColor);
                        await _context.SaveChangesAsync();
                        var productColor = new DbProductColor
                        {
                            IdProduct = product.Id,
                            IdColor = dbColor.Id
                        };
                        await _context.DbProductColors.AddAsync(productColor);
                     

                    }

                    var removedColors = existingColors.Where(ec => !model.Colors.Contains(ec.IdColorNavigation.Name)).ToList();
                    if (removedColors.Any())
                    {
                        _context.DbProductColors.RemoveRange(removedColors);
                       
                    }
                   



                    if (model.Images != null && model.Images.Any())
                    {
                        // Cập nhật Images
                        var removedImages = product.DbProductImages.ToList();
                        if (removedImages.Any())
                        {
                            foreach (var removedImage in removedImages)
                            {
                                if (removedImage.IdImageNavigation.IdPublic != null)
                                {
                                    await _cloudinaryService.DeleteImageAsync(removedImage.IdImageNavigation.IdPublic);
                                }
                            }
                            _context.DbProductImages.RemoveRange(removedImages);
                            
                        }
                       
                        foreach (var imageFile in model.Images)
                        {
                            var result = await _cloudinaryService.UploadFileAsync(imageFile, "ECommerceProducts");

                            var image = new DbImage
                            {
                                IdPublic = result.Id,
                                Url = result.Url
                            };
                            await _context.DbImages.AddAsync(image);
                            await _context.SaveChangesAsync();

                            var productImage = new DbProductImage
                            {
                                IdProduct = product.Id,
                                IdImage = image.Id
                            };
                            await _context.DbProductImages.AddAsync(productImage);
                            
                        }
                    }


                    // Cập nhật Sizes
                    var existingSizes = product.DbProductSizes.ToList();
                    var newSizes = model.Sizes.Except(existingSizes.Select(es => es.IdSizeNavigation.Name)).ToList();
                    foreach (var size in newSizes)
                    {
                        var dbSize = new DbSize { Name = size };
                        await _context.DbSizes.AddAsync(dbSize);
                        await _context.SaveChangesAsync();
                        var productSize = new DbProductSize
                        {
                            IdProduct = product.Id,
                            IdSize = dbSize.Id
                        };
                        await _context.DbProductSizes.AddAsync(productSize);                  
                    }
                   
                    var removedSizes = existingSizes.Where(es => !model.Sizes.Contains(es.IdSizeNavigation.Name)).ToList();
                    if (removedSizes.Any())
                    {
                        _context.DbProductSizes.RemoveRange(removedSizes);                      
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    _notyf.Success("Delete Category Successfuly");
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {

                    await transaction.RollbackAsync();
                    if (model.Images == null || !model.Images.Any()) model.UrlImages = product.DbProductImages.Select(pi => pi.IdImageNavigation.Url).ToList();
                    await SetViewBagsAsync();
                    _notyf.Error($"An error occurred while editing the product: {ex.Message}");
                    return View(model);
                }
            }
        }


        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var db = await _context.DbProducts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (db == null)
            {
                return NotFound();
            }

            return View(db);
        }



        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            // Tìm sản phẩm trong cơ sở dữ liệu
            var product = await _context.DbProducts.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Xóa màu sắc của sản phẩm
            var productColors = _context.DbProductColors.Where(pc => pc.IdProduct == id);
            _context.DbProductColors.RemoveRange(productColors);

            // Xóa ảnh của sản phẩm
            var productImages = _context.DbProductImages.Where(pi => pi.IdProduct == id);
            _context.DbProductImages.RemoveRange(productImages);

            // Xóa kích thước của sản phẩm
            var productSizes = _context.DbProductSizes.Where(ps => ps.IdProduct == id);
            _context.DbProductSizes.RemoveRange(productSizes);

            // Xóa sản phẩm chính
            _context.DbProducts.Remove(product);

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            _notyf.Success("Delete product successfully.");
            return RedirectToAction(nameof(Index));
        }
    }
}
