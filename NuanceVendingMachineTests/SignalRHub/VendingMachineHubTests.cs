using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NuanceVendingMachine.Dto;
using NuanceVendingMachine.Models;
using NuanceVendingMachine.Repository;
using NuanceVendingMachine.SignalRHub;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NuanceVendingMachineTests.SignalRHub
{
    public class VendingMachineHubTests
    {
        [Fact]
        public async Task SendMessageTest()
        {
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            //Mock Repository and Mapper
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(x => x.GetProducts()).Verifiable();
            var mapperMock = new Mock<IMapper>();
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

            var hub = new VendingMachineHub(mockRepo.Object, mapperMock.Object)
            {
                Clients = mockClients.Object

            };

            // act
            await hub.SendMessage();


            // assert
            mockRepo.Verify(x => x.GetProducts(), Times.Once);
            mockClients.Verify(clients => clients.All, Times.Once);
        }

        [Fact]
        public async Task SaleItem_NotCallingSendMessage_ForUnvailableProduct()
        {
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            Product returnType = null;
            //Mock Repository and Mapper
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync(returnType).Verifiable();
            var hub = new VendingMachineHub(mockRepo.Object, null)
            {
                Clients = mockClients.Object

            };
            // act
            await hub.SaleItem(1);

            // assert
            mockRepo.Verify(x => x.GetProductById(1), Times.Once);
            mockClients.Verify(clients => clients.All, Times.Never);

        }

        [Fact]
        public async Task SaleItem_NotCallingSendMessage_ForProductHavingStockZero()
        {
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            //Mock Repository and Mapper
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync(new Product()).Verifiable();
            var hub = new VendingMachineHub(mockRepo.Object, null)
            {
                Clients = mockClients.Object

            };
            // act
            await hub.SaleItem(1);

            // assert
            mockRepo.Verify(x => x.GetProductById(1), Times.Once);
            mockClients.Verify(clients => clients.All, Times.Never);

        }

        [Fact]
        public async Task SaleItem_CallingSendMessage_ForProductHavingStockMoreThanZero()
        {
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            //Mock Repository and Mapper
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync(new Product() { Stock=1}).Verifiable();
            mockRepo.Setup(x => x.UpdateProduct(It.IsAny<Product>())).Verifiable();
            mockRepo.Setup(x => x.GetProducts()).Verifiable();

            var mapperMock = new Mock<IMapper>();
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
            var hub = new VendingMachineHub(mockRepo.Object, mapperMock.Object)
            {
                Clients = mockClients.Object

            };
            // act
            await hub.SaleItem(1);

            // assert
            mockRepo.Verify(x => x.GetProductById(1), Times.Once);
            mockRepo.Verify(x => x.UpdateProduct(It.IsAny<Product>()), Times.Once);
            mockRepo.Verify(x => x.GetProducts(), Times.Once);
            mockClients.Verify(clients => clients.All, Times.Once);

        }

    }
}
