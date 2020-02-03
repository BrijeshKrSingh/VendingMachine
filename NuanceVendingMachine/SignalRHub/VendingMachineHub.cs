using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using NuanceVendingMachine.Dto;
using NuanceVendingMachine.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NuanceVendingMachine.SignalRHub
{
    public class VendingMachineHub: Hub
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public VendingMachineHub(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task SaleItem(int id)
        {
            var product = await _repository.GetProductById(id);
            if (product?.Stock > 0)
            {
                product.Stock -= 1;
                await _repository.UpdateProduct(product).ConfigureAwait(false);
                
                await SendMessage();
            }
           
        }
        
        public async Task SendMessage()
        {
            var products = await _repository.GetProducts().ConfigureAwait(false);
            var productDto = _mapper.Map<IEnumerable<ProductDto>>(products);

            await Clients.All.SendAsync("ReceiveMessage", productDto?.Where(x=>x?.AvailableQuantity>0));
        }


        public Task SendMessageToGroups(string message)
        {
            List<string> groups = new List<string>() { "SignalR Users" };
            return Clients.Groups(groups).SendAsync("ReceiveMessage", message);
        }


        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await SendMessage();
            await base.OnConnectedAsync();
        }
      

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }

     
    }
}
