var o = 0, f = 0, q = 0;

$(document).ready(function () {
    $("#o1").click(function () {
        $("#o1").css('color',"yellow");
        $("#o2,#o3,#o4,#o5").css('color',"#EEEADE");
        o = 1;
    });
    $("#o2").click(function () {
        $("#o1,#o2").css('color',"yellow");
        $("#o3,#o4,#o5").css('color',"#EEEADE");
        o = 2;
    });
    $("#o3").click(function () {
        $("#o1,#o2,#o3").css('color',"yellow");
        $("#o4,#o5").css('color',"#EEEADE");
        o = 3;
    });
    $("#o4").click(function () {
        $("#o1,#o2,#o3,#o4").css('color',"yellow");
        $("#o5").css('color',"#EEEADE");
        o = 4;
    });
    $("#o5").click(function () {
        $("#o1,#o2,#o3,#o4,#o5").css('color',"yellow");
        o = 5;
    });




    $("#f1").click(function () {
        $("#f1").attr('src', '~/image/yellow-star.png');
        $("#f2,#f3,#f4,#f5").css('color',"#EEEADE");
        f = 1;
    });
    $("#f2").click(function () {
        $("#f1,#f2").css('color',"yellow");
        $("#f3,#f4,#f5").css('color',"#EEEADE");
        f = 2;
    });
    $("#f3").click(function () {
        $("#f1,#f2,#f3").css('color',"yellow");
        $("#f4,#f5").css('color',"#EEEADE");
        f = 3;
    });
    $("#f4").click(function () {
        $("#f1,#f2,#f3,#f4").css('color',"yellow");
        $("#f5").css('color',"#EEEADE");
        f = 4;
    });
    $("#f5").click(function () {
        $("#f1,#f2,#f3,#f4,#f5").css('color',"yellow");
        f = 5;
    });




    $("#q1").click(function () {
        $("#q1").css('color',"yellow");
        $("#q2,#q3,#q4,#q5").css('color',"#EEEADE");
        q = 1;
    });
    $("#q2").click(function () {
        $("#q1,#q2").css('color',"yellow");
        $("#q3,#q4,#q5").attr('src', '~/image/white-star.png');
        q = 2;
    });
    $("#q3").click(function () {
        $("#q1,#q2,#q3").css('color',"yellow");
        $("#q4,#q5").css('color',"#EEEADE");
        q = 3;
    });
    $("#q4").click(function () {
        $("#q1,#q2,#q3,#q4").css('color',"yellow");
        $("#q5").css('color',"#EEEADE");
        q = 4;
    });
    $("#q5").click(function () {
        $("#q1,#q2,#q3,#q4,#q5").css('color',"yellow");
        q = 5;
    });

    $(".rate").on('click', function () {
        var ratesp = (o + f + q) / 3;
        //console.log(ratesp);


        //console.log("p " + p);
        //console.log("t" + t);
        //console.log("s" + s);
        x = Math.floor(ratesp)
        // console.log("math fllorr " + x);
        x = ratesp.toFixed(0);
        //console.log("fixed value" + x);
        $("#rateavg").html(x);

        switch (x) {
            case '0':
                $("#a1,#a2,#a3,#a4,#a5").css('color',"yellow");
                break;
            case '1':
                $("#a1").css('color',"yellow");
                $("#a2,#a3,#a4,#a5").css('color',"#EEEADE");
                break;
            case '2':
                $("#a1,#a2").css('color',"yellow");
                $("#a3,#a4,#a5").css('color',"#EEEADE");
                break;
            case '3':
                $("#a1,#a2,#a3").css('color',"yellow");
                $("#a4,#a5").css('color',"#EEEADE");
                break;
            case '4':
                $("#a1,#a2,#a3,#a4").css('color',"yellow");
                $("#a5").css('color',"#EEEADE");
                break;
            case '5':
                $("#a1,#a2,#a3,#a4,#a5").css('color',"yellow");
                break;
            default:
                alert('Nobody Wins!');
        }

    })


});