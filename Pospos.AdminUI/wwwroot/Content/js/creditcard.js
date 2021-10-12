
function clickclear(thisfield, defaulttext) {
    if (thisfield.value == defaulttext) {
        thisfield.value = "";
    }
}
function clickrecall(thisfield, defaulttext) {
    if (thisfield.value == "") {
        thisfield.value = defaulttext;
    }
}

$(document).ready(function () {
    $('#myTabs a:first').tab('show');

    $("#btnPay").on("click", function (e) {
        $(this).closest("form")[0].submit();
        $(this).prop('disabled', true)
        $("#waitImage").css("display", "block");

    });
    $("#btnBkm").on("click", function (e) {
        e.PreventDefault();
        $(this).closest("form")[0].submit();
        $(this).prop('disabled', true)
        $("#waitImage").css("display", "block");
    });
    $("#btnRemittance").on("click", function (e) {
        e.PreventDefault();
        $(this).closest("form")[0].submit();
        $(this).prop('disabled', true)
        $("#waitImage").css("display", "block");
    });
    $("#CardNumber1").on('keyup', function (e) {
        //regex
        if (this.value != this.value.replace(/[^0-9]/g, '')) {
            this.value = this.value.replace(/[^0-9]/g, '');
        }
        //show card image
        if ($("#CardNumber1").val().charAt(0) == "4") {
            //visa
            $("#VisaOrMaster").html("<img src=\"/Content/images/visa.png\">");
            $("#CreditCardType").val("");
            $("#CreditCardType").val("Visa");
        }
        else if ($("#CardNumber1").val().charAt(0) == "5") {
            //mastercard
            $("#VisaOrMaster").html("<img src=\"/Content/images/mastercard.png\">");
            $("#CreditCardType").val("");
            $("#CreditCardType").val("MasterCard");
        }
        else if ($("#CardNumber1").val().charAt(0) == "3") {
            //mastercard
            $("#VisaOrMaster").html("<img src=\"/Content/images/amex.png\">");
            $("#CreditCardType").val("");
            $("#CreditCardType").val("Amex");
        }
        else {
            //default
            $("#CreditCardType").val("");
            $("#VisaOrMaster").html("");
        }

        //jump to other textbox
        if ($("#CardNumber1").val().length == 4) {

            //alert($("#CardNumber1").val().length);
            $("#CardNumber2").focus();
        }
    });
    $("#CardNumber2").on('keydown', function (e) {
        //jump to prev. textbox
        var Prevcode = (e.keyCode ? e.keyCode : e.which);
        if (Prevcode == 8 && this.value.length == 0) {
            //alert(this.value.length);
            $("#CardNumber1").focus();

        }
    });
    $("#CardNumber2").on('keyup', function (e) {
        //regex
        if (this.value != this.value.replace(/[^0-9]/g, '')) {
            this.value = this.value.replace(/[^0-9]/g, '');
        }

        if ($("#CardNumber1").val().length != 4) {
            $("#taksitTablosu").html("");
            $('#solkartdiv').css("background-image", "url(/Content/images/card_sample.png)");
            $('#sagkartdiv').css("background-image", "url(/Content/images/card_sample.png)");
        }
        else if ($("#CardNumber2").val().length < 2) {
            $("#taksitTablosu").html("");
            $('#solkartdiv').css("background-image", "url(/Content/images/card_sample.png)");
            $('#sagkartdiv').css("background-image", "url(/Content/images/card_sample.png)");
        }
        else if ($("#CardNumber2").val().length == 2) {
            $("#taksitTablosu").show();
            $.ajax({
                url: '/PaymentTest/InstallmentTable',
                type: 'POST',
                cache: 'false',
                data: { binCode: $("#CardNumber1").val() + $("#CardNumber2").val() },
                success: function (result) {
                    $("#taksitTablosu").html("");
                    $("#taksitTablosu").replaceWith(result);
                },
                error: function (request, status, err) {
                    $("#taksitTablosu").html("");
                }

            });
            $.ajax({
                url: '/PaymentTest/GetBackground',
                type: 'POST',
                data: { binCode: $("#CardNumber1").val() + $("#CardNumber2").val() },
                success: function (result) {
                    $('#solkartdiv').css("background-image", "url(" + result + ")");
                    $('#sagkartdiv').css("background-image", "url(" + result + ")");
                },
                error: function (request, status, err) {
                    $('#solkartdiv').css("background-image", "url(/Content/images/card_sample.png)");
                    $('#sagkartdiv').css("background-image", "url(/Content/images/card_sample.png)");
                }

            });
        }
        //jump to other textbox
        if ($("#CardNumber2").val().length == 4) {
            $("#CardNumber3").focus();
        }
    });
    $("#CardNumber3").on('keydown', function (e) {
        //jump to prev. textbox
        var Prevcode = (e.keyCode ? e.keyCode : e.which);
        if (Prevcode == 8 && this.value.length == 0) {
            //alert(this.value.length);
            $("#CardNumber2").focus();

        }
    });
    $("#CardNumber3").on('keyup', function (e) {

        //regex
        if (this.value != this.value.replace(/[^0-9]/g, '')) {
            this.value = this.value.replace(/[^0-9]/g, '');
        }

        //jump to other textbox
        if ($("#CardNumber3").val().length == 4) {
            $("#CardNumber4").focus();
        }
    });
    $("#CardNumber4").on('keydown', function (e) {
        //jump to prev. textbox
        var Prevcode = (e.keyCode ? e.keyCode : e.which);
        if (Prevcode == 8 && this.value.length == 0) {
            //alert(this.value.length);
            $("#CardNumber3").focus();

        }
    });
    $("#CardNumber4").on('keyup', function (e) {
        //regex
        if (this.value != this.value.replace(/[^0-9]/g, '')) {
            this.value = this.value.replace(/[^0-9]/g, '');
        }
    });
});
