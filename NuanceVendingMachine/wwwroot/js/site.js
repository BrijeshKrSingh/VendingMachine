
//setup connection with SignalR hub uses signalr.js
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/VendingMachineHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();
connection.start().catch(err => console.error(err.toString()));

class Coin {
    constructor(cent, qty) {
        this.Cent = cent;
        this.Quantity = qty;
    }
}

//for testing purpose TODO: Need to delete
class Product {
    constructor(id, title, stock, price) {
        this.id = id;
        this.title = title;
            this.stock = stock;
            this.price = price;
    }
}


var app = new Vue({
    el: '#app',
    data: {
        message: 'Please insert the coin to buy items',
       products:[],
       //products: [new Product(1, 'beer', 12, 100), new Product(2, 'coke', 2, 150), new Product(3, 'chips', 4, 200), new Product(4, 'chips', 4, 200)],
        allCoins: { availableCoins: [5, 10, 20, 50, 100], changeCoins: [] },
        totalInsertedCoins: 0,
        totalChangeCoins: 0,
        lowOnCoins: false
    },
    methods: {
        insertCoin: function (coinValue) {
            //reset it
            this.message = "";
            this.totalChangeCoins = 0;
            this.allCoins.changeCoins = [];
            this.totalInsertedCoins = this.totalInsertedCoins + coinValue;
        },
        selectProduct: function (productId, salePrice) {
            connection.invoke("SaleItem", productId).catch(err => console.error(err.toString()));
            this.totalChangeCoins = this.totalInsertedCoins - salePrice;
            var balance = this.totalChangeCoins;
            this.totalInsertedCoins = 0;
            this.allCoins.changeCoins = this.getCoinCount(balance);
            this.message= 'Please insert the coin to buy items';
        },
        cancel: function () {
            this.totalChangeCoins = this.totalInsertedCoins;
            this.totalInsertedCoins = 0;
            this.allCoins.changeCoins = this.getCoinCount(this.totalChangeCoins);
            this.message = 'Please insert the coin to buy items';
        },

        getCoinCount: function (balance) {
            var returnValue = new Array(5);
            var mdata = this.allCoins;
            for (i = mdata.availableCoins.length - 1; i >= 0; i--) {
                if (balance > 0) {
                    var qty = parseInt(balance / mdata.availableCoins[i]);
                    if (qty > 0) {
                        var coin = new Coin(mdata.availableCoins[i], qty);
                        returnValue[i] = coin;
                        balance = balance % mdata.availableCoins[i];
                    }
                    else {
                        var coin = new Coin(mdata.availableCoins[i], 0);
                        returnValue[i] = coin;
                    }

                }
                else {
                    var coin = new Coin(mdata.availableCoins[i], 0);
                    returnValue[i] = coin;
                }


            }
            return returnValue;
        }
    },

    beforeCreate: function () {
        var mdata = this;
        connection.on("ReceiveMessage", (message) => {
            mdata.products = message;

        });

    },

    created: function () {
        //show app
        document.getElementById('app').classList.remove("notready");
    }
});
