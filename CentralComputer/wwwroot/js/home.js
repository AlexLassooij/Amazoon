let speed = 5, opacityLimit = 0.02;
let animDone = true;
function fadeAnim(time, object, out=false){
    if(!out){
        let fadeInterval = setInterval(function(){
            if(parseFloat(object.css("opacity")) >= 0.98){
                animDone = true;
                clearInterval(fadeInterval);
            }
            object.css("opacity", parseFloat(object.css("opacity")) + speed / 100);
        }, time)
    }
    else {
        animDone = false;
        let fadeInterval = setInterval(function(){
            if(parseFloat(object.css("opacity")) <= opacityLimit){
                clearInterval(fadeInterval);
            }
            object.css("opacity", parseFloat(object.css("opacity")) - speed / 100);
        }, time)
    }
}

function changeBackCard(img, color, productImgArr, productNameArr, productPriceArr){
    fadeAnim(10, $("#bodyDiv"), true);
    fadeAnim(10, $(".dataImg"), true);
    fadeAnim(10, $(".dataName"), true);
    fadeAnim(10, $(".dataPrice"), true);
    setTimeout(function(){
        
        $("#bodyDiv").css("background-image", `url(${img})`);
        $("body").css("background-color",color);

        $(".dataImg").eq(0).attr("src", productImgArr[0])
        $(".dataImg").eq(1).attr("src", productImgArr[1])
        $(".dataImg").eq(2).attr("src", productImgArr[2])
        
        $(".dataName").eq(0).html(productNameArr[0])
        $(".dataName").eq(1).html(productNameArr[1])
        $(".dataName").eq(2).html(productNameArr[2])

        $(".dataPrice").eq(0).html(productPriceArr[0])
        $(".dataPrice").eq(1).html(productPriceArr[1])
        $(".dataPrice").eq(2).html(productPriceArr[2])

        fadeAnim(10, $("#bodyDiv"), false);
        fadeAnim(10, $(".dataImg"), false);
        fadeAnim(10, $(".dataName"), false);
        fadeAnim(10, $(".dataPrice"), false);
        //$("#bodyDiv").fadeIn(3000);
        /*$(".data").fadeIn(3000);
        $(".dataImg").fadeIn(3000);
        $(".dataName").fadeIn(3000);
        $(".dataPrice").fadeIn(3000);*/
    }, 1200);
}

let backArr = [];
let colorArr = ["#c6f4e0", "#ebebeb", "#daefe6"];
let imgArr = [
    ["imgs/tv.png", "imgs/speakers.png", "imgs/ps5.png"],
    ["imgs/teddy.png", "imgs/mg.png", 'imgs/wilson.png'],
    ["imgs/instantpot.png", "imgs/k.png", "imgs/chess.png"]
];
let productName = [
    ['55" Samsung TV', 'LG Surround Speakers', 'Sony Playstation 5'],
    ['Cool Teddy Bear', 'TaoTronics Massage Gun', 'Wilson Volleyball'],
    ['Instant Pot', 'Keurig K95 Coffee Maker', 'Ambassador Chess Set']
];
let productPrice = [
    ["$1599.99", "$599.99", "$799.99"],
    ["$39.99", "$99.99","I'm sorry, Wilson!"],
    ["$199.99","$198.99","$89.99"]
];

for(let i = 1; i < 4; i++){
    backArr.push(`imgs/backcard${i}.jpg`);
}

let i = 2;
setInterval(function(){
    if(animDone){
        let img = backArr[i-1];
        let color = colorArr[i-1]
        let productImgArr = [imgArr[0][i-1],imgArr[1][i-1],imgArr[2][i-1]];
        let productNameArr = [productName[0][i-1],productName[1][i-1],productName[2][i-1]];
        let productPriceArr = [productPrice[0][i-1],productPrice[1][i-1],productPrice[2][i-1]];
        changeBackCard(img, color, productImgArr, productNameArr, productPriceArr);
        i+=1
        if(i > 3){
            i = 1;
        }
    } 
}, 8000);