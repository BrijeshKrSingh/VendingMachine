using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuanceVendingMachine.Dto;
using NuanceVendingMachine.Models;
using NuanceVendingMachine.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuanceVendingMachine.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;


        public AdminController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var products = await _repository.GetProducts().ConfigureAwait(false);
            return View( _mapper.Map<IEnumerable<ProductDto>>(products));
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repository.GetProductById(id.Value).ConfigureAwait(false);
            if (product == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ProductDto>(product));
        }

        // GET: Admin/Create
        public async Task<IActionResult> Create()
        {
           // var products = await _repository.GetProducts().ConfigureAwait(false);

            var productTypes = await _repository.GetProductTypes().ConfigureAwait(false);
            var productTypeDto = _mapper.Map<IEnumerable<ProductTypeDto>>(productTypes);

            ViewData["ProductTypeId"] = new SelectList(productTypeDto, "Id", "Name");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,AvailableQuantity,ProductTypeId")] ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(productDto);
                await _repository.AddProduct(product).ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            var productTypes = await _repository.GetProductTypes().ConfigureAwait(false);
            var productTypeDto = _mapper.Map<IEnumerable<ProductTypeDto>>(productTypes);
            ViewData["ProductTypeId"] = new SelectList(productTypeDto, "Id", "Name", productDto.ProductTypeId);
            return View(productDto);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repository.GetProductById(id.Value).ConfigureAwait(false);
            if (product == null)
            {
                return NotFound();
            }
            var productDto = _mapper.Map<ProductDto>(product);
            var productTypes = await _repository.GetProductTypes().ConfigureAwait(false);
            var productTypeDto = _mapper.Map<IEnumerable<ProductTypeDto>>(productTypes);
            ViewData["ProductTypeId"] = new SelectList(productTypeDto, "Id", "Name", productDto.ProductTypeId);
            return View(_mapper.Map<ProductDto>(product));
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AvailableQuantity,ProductTypeId")] ProductDto productDto)
        {
            if (id != productDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var product = _mapper.Map<Product>(productDto);
                    await _repository.UpdateProduct(product);
                    
                }
                catch
                { 
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            var productTypes = await _repository.GetProductTypes().ConfigureAwait(false);
            var productTypeDto = _mapper.Map<IEnumerable<ProductTypeDto>>(productTypes);
            ViewData["ProductTypeId"] = new SelectList(productTypeDto, "Id", "Name", productDto.ProductTypeId);
            return View(productDto);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repository.GetProductById(id.Value).ConfigureAwait(false);
            if (product == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ProductDto>(product));
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteProduct(id);
           
            return RedirectToAction(nameof(Index));
        }

    }
}
