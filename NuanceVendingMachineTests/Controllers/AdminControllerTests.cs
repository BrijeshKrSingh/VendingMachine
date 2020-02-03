using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NuanceVendingMachine.Controllers;
using NuanceVendingMachine.Dto;
using NuanceVendingMachine.Models;
using NuanceVendingMachine.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NuanceVendingMachineTests.Controllers
{
    public class AdminControllerTests
    {
        private readonly Mock<IMapper> mapperMock;

        public AdminControllerTests()
        {
            mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<ProductDto>>(It.IsAny<IEnumerable<Product>>()))
                .Returns(
                    new List<ProductDto>()
                    {
                        new ProductDto
                        {
                            Id=1,
                            Name="Test",
                             AvailableQuantity=20,
                              Price=100,
                               ProductType="drink",
                                ProductTypeId=1
                        }
                    }
               );
            mapperMock.Setup(x => x.Map<ProductDto>(It.IsAny<Product>()))
               .Returns(
                  
                        new ProductDto
                        {
                            Id=1,
                            Name="Test",
                             AvailableQuantity=20,
                              Price=100,
                               ProductType="drink",
                                ProductTypeId=1
                        }
              );

            mapperMock.Setup(x => x.Map<Product>(It.IsAny<ProductDto>()))
              .Returns(

                       new Product
                       {
                           Id = 1,
                           Stock =100,
                            Title ="water",
                            ProductType = new ProductType
                            {
                                Price = 100,
                                ProductTypeId = 1,
                                TypeName = "Drink"
                            }
                       }
             );

            mapperMock.Setup(x => x.Map<IEnumerable<ProductTypeDto>>(It.IsAny<IEnumerable<ProductType>>()))
               .Returns(
                   new List<ProductTypeDto>()
                   {
                        new ProductTypeDto
                        {
                            Name="drink",
                            Id=1,
                            Price =100
                        }
                   }
              );

        }

        //Index
        [Fact]
        public async Task Index_IsReturning_AllProducts()
        {
            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(x => x.GetProducts()).Verifiable();
            AdminController controller = new AdminController(repositoryMock.Object, mapperMock.Object);

            var result = await controller.Index();

            repositoryMock.Verify(x => x.GetProducts(), Times.Once);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(
                viewResult.ViewData.Model);
            Assert.Single(model);
        }


        //Details
        [Fact]

        public async Task Details_ShouldReturn_NotFound_ForNull()
        {
            var controller = new AdminController(null, null);
            var result = await controller.Details(null) as NotFoundResult;
            Assert.Equal(404, result.StatusCode);

        }


        [Fact]

        public async Task Details_ShouldReturn_NotFound_ForProductNotFound()
        {
            var repositoryMock = new Mock<IProductRepository>();
            Product product = null;
            repositoryMock.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync(product).Verifiable();

            var controller = new AdminController(repositoryMock.Object, null);
            var result = await controller.Details(1) as NotFoundResult;
            repositoryMock.Verify(x => x.GetProductById(1), Times.Once);
            Assert.Equal(404, result.StatusCode);

        }

        [Fact]

        public async Task Details_ShouldReturn_Data_ForValidProduct()
        {
            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync(new Product { Id=1, ProductTypeId=1}).Verifiable();

            var controller = new AdminController(repositoryMock.Object, mapperMock.Object);
            var result = await controller.Details(1);
            repositoryMock.Verify(x => x.GetProductById(1), Times.Once);
           Assert.IsType<ViewResult>(result);
           
        }

        //Create
        [Fact]

        public async Task Create_ShouldCallRepo_GetProducts()
        {
            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(x => x.GetProductTypes()).Verifiable();

            var controller = new AdminController(repositoryMock.Object, mapperMock.Object);

            var result = await controller.Create();

            repositoryMock.Verify(x => x.GetProductTypes(), Times.Once);
             Assert.IsType<ViewResult>(result);
        }

        [Fact]

        public async Task Create_ShouldCallRepo_AddDataAndReturnIndex()
        {
            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(x => x.AddProduct(It.IsAny<Product>())).Verifiable();

            var controller = new AdminController(repositoryMock.Object, mapperMock.Object);
            var productDto = new ProductDto
            {
                ProductTypeId = 1,
                Price = 100,
                AvailableQuantity = 100,
                Id = 1,
                Name = "Water",
                ProductType = "drink"
            };
            var result = await controller.Create(productDto) as RedirectToActionResult;
            repositoryMock.Verify(x => x.AddProduct(It.IsAny<Product>()), Times.Once);
            Assert.Equal("Index", result.ActionName);
        }

       

        // Edit
        [Fact]

        public async Task Edit_ShouldReturn_NotFound_ForNull()
        {
            var controller = new AdminController(null, null);
            var result = await controller.Edit(null) as NotFoundResult;
            Assert.Equal(404, result.StatusCode);

        }


        [Fact]

        public async Task Edit_ShouldReturn_NotFound_ForProductNotFound()
        {
            var repositoryMock = new Mock<IProductRepository>();
            Product product = null;
            repositoryMock.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync(product).Verifiable();

            var controller = new AdminController(repositoryMock.Object, null);
            var result = await controller.Edit(1) as NotFoundResult;
            repositoryMock.Verify(x => x.GetProductById(1), Times.Once);
            Assert.Equal(404, result.StatusCode);

        }

        [Fact]

        public async Task Edit_ShouldReturn_Data_ForValidProduct()
        {
            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync(new Product { Id = 1, ProductType = new ProductType { ProductTypeId = 1 } }).Verifiable();
            repositoryMock.Setup(x => x.GetProductTypes()).ReturnsAsync(new List<ProductType>() { new ProductType { ProductTypeId = 1} }).Verifiable();

            var controller = new AdminController(repositoryMock.Object, mapperMock.Object);
            var result = await controller.Edit(1);
            repositoryMock.Verify(x => x.GetProductById(1), Times.Once);
            repositoryMock.Verify(x => x.GetProductTypes(), Times.Once);
            Assert.IsType<ViewResult>(result);
          
        }


        [Fact]

        public async Task Edit_ShouldCallRepo_AddDataAndReturnIndex()
        {
            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(x => x.UpdateProduct(It.IsAny<Product>())).Verifiable();

            var controller = new AdminController(repositoryMock.Object, mapperMock.Object);
            var productDto = new ProductDto
            {
                ProductTypeId = 1,
                Price = 100,
                AvailableQuantity = 100,
                Id = 1,
                Name = "Water",
                ProductType = "drink"
            };
            var result = await controller.Edit(1, productDto) as RedirectToActionResult;
            repositoryMock.Verify(x => x.UpdateProduct(It.IsAny<Product>()), Times.Once);
            Assert.Equal("Index", result.ActionName);
        }

        //Delete

        [Fact]
        public async Task Delete_ShouldReturn_NotFound_ForNull()
        {
            var controller = new AdminController(null, null);
            var result = await controller.Delete(null) as NotFoundResult;
            Assert.Equal(404, result.StatusCode);

        }


        [Fact]

        public async Task Delete_ShouldReturn_NotFound_ForProductNotFound()
        {
            var repositoryMock = new Mock<IProductRepository>();
            Product product = null;
            repositoryMock.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync(product).Verifiable();

            var controller = new AdminController(repositoryMock.Object, null);
            var result = await controller.Delete(1) as NotFoundResult;
            repositoryMock.Verify(x => x.GetProductById(1), Times.Once);
            Assert.Equal(404, result.StatusCode);

        }

        [Fact]

        public async Task Delete_ShouldReturn_View_ForValidProduct()
        {
            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync(new Product { Id = 1, ProductTypeId = 1 }).Verifiable();

            var controller = new AdminController(repositoryMock.Object, mapperMock.Object);
            var result = await controller.Delete(1);
            repositoryMock.Verify(x => x.GetProductById(1), Times.Once);
            Assert.IsType<ViewResult>(result);
        }

        //delete confirmed
        [Fact]
        public async Task DeleteConfirmed_ShouldCallRepo_ToDelete_Data_And_Redirectto_IndexPage()
        {
            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(x => x.DeleteProduct(It.IsAny<int>())).Verifiable();

            var controller = new AdminController(repositoryMock.Object, mapperMock.Object);
            var result = await controller.DeleteConfirmed(1) as RedirectToActionResult;
            repositoryMock.Verify(x => x.DeleteProduct(1), Times.Once);
            Assert.Equal("Index", result.ActionName);
        }

    }
}
