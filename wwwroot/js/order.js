let orderDict = {}

function updateOrder(id, changeAmount, maxAmount) {
    if (!(id in orderDict)) {
        if (changeAmount === 1) {
            orderDict[id] = 1;
        }
    }
    else if (orderDict[id] == 1 && changeAmount === -1) {
        delete orderDict[id];
    }
    else {
        if (changeAmount === 1 && orderDict[id] === maxAmount) {
            changeAmount = 0;
        }
        orderDict[id] += changeAmount;
    }

    let orderString = ""
    for (const [item, quantity] of Object.entries(orderDict)) {
        orderString += `${item} ${quantity},`
    }
    orderString = orderString.slice(0, -1);
    $("#itemQuantity").attr("value", orderString);
    console.log(orderDict);
}

function increment(id, maxAmount) {
    console.log(`${id}`)
    updateOrder(id, 1, maxAmount);
    document.getElementById(id).stepUp();
}
function decrement(id, maxAmount) {
    console.log(`${id}`)
    updateOrder(id, -1, maxAmount);
    document.getElementById(id).stepDown();
}

