var button = document.getElementById("address");
var target = document.getElementById("form");
var a = true;

function display() {
    if (a) {
        target.setAttribute("class", "show");
        button.setAttribute("class", "hide");
        a = false;
    }
    else {
        target.setAttribute("class", "hide");
        a = true;
    }
}

var tab1 = document.getElementById("Setupservice");
var tab2 = document.getElementById("SchedulePlan");
var tab3 = document.getElementById("YourDetails");
var tab4 = document.getElementById("MakePayment");
function goto() {
    document.getElementById("sp1").classList.add("active");
    tab1.classList.remove("active");
    tab2.setAttribute("class", "active");
}
function gotoaddress() {
    document.getElementById("yd").classList.add("active");
    tab2.setAttribute("class", "hide");
    tab3.setAttribute("class", "active");
}
function gotopay() {
    document.getElementById("mp").classList.add("active");
    tab3.setAttribute("class", "hide");
    tab4.setAttribute("class", "active");
}






var b, time = 3, price = 54;
function change1() {

    if (b == 1) {
        document.getElementById("extra").innerHTML = "";
        
        document.getElementById("1").classList.remove("circle-color");
        time = time - 0.5;
        price = price - 9;
        return b = 0;
    }


    else {
        document.getElementById("extra").innerHTML = "<p>inside cabinet</p><p>30 mins</p>";
        
        document.getElementById("1").classList.add("circle-color");
        time = time + 0.5;
        price = price + 9;
        return b = 1;
    }



}

var c, fri;
function change2() {

    if (c == 1) {
        document.getElementById("extra1").innerHTML = "";
        
        document.getElementById("2").classList.remove("circle-color");
        time = time - 0.5;
        price = price - 9;
        return c = 0;
    }


    else {
        document.getElementById("extra1").innerHTML = "<p>inside fridge</p><p>30 mins</p>";
        
        document.getElementById("2").classList.add("circle-color");
        time = time + 0.5;
        price = price + 9;
        return c = 1;
    }

}

var d, io;
function change3() {
    if (d == 1) {
        document.getElementById("extra2").innerHTML = "";
        
        document.getElementById("3").classList.remove("circle-color");
        time = time - 0.5;
        price = price - 9;
        return d = 0;
    }


    else {
        document.getElementById("extra2").innerHTML = "<p>inside oven</p><p>30 mins</p>";
        document.getElementById("3").classList.add("circle-color");
        time = time + 0.5;
        price = price + 9;
        return d = 1;
    }

}

var e, lw;
function change4() {
    if (e == 1) {
        document.getElementById("extra3").innerHTML = "";
        document.getElementById("4").classList.remove("circle-color");
        time = time - 0.5;
        price = price - 9;
        return e = 0;
    }


    else {
        document.getElementById("extra3").innerHTML = "<p>Laundry wash</p><p>30 mins</p>";
        document.getElementById("4").classList.add("circle-color");
        time = time + 0.5;
        price = price + 9;
        return e = 1;
    }

}
var f, iw;
function change5() {
    if (f == 1) {
        document.getElementById("extra4").innerHTML = "";
        document.getElementById("5").classList.remove("circle-color");
        time = time - 0.5;
        price = price - 9;
        return f = 0;
    }


    else {
        document.getElementById("extra4").innerHTML = "<p>Interior windows</p><p>30 mins</p>";
        document.getElementById("5").classList.add("circle-color");
        time = time + 0.5;
        price = price + 9;
        return f = 1;

    }


}

function total_time() {
    document.getElementById("total_time").innerHTML = time;
}

function total_price() {
    document.getElementById("sub_total").innerHTML = price;
}

function total_price_1() {
    document.getElementById("total_price").innerHTML = price;
}