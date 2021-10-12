$(function () {
    $(".checkBonusHolder").css("display", "none");
    $("#PointContainer").css("display", "none");

    $("#CardNumber1").on('keyup', function (e) {
        checkCardNumber();
    });
    $("#CardNumber2").on('keyup', function (e) {
        checkCardNumber();
    });
    $("#CardNumber3").on('keyup', function (e) {
        checkCardNumber();
    });
    $("#CardNumber4").on('keyup', function (e) {
        checkCardNumber();
    });

    $(document).on('change', 'input[Id="RewardUseAllPoint"]', function (e) {

        var cardNumber1 = $("#CardNumber1").val();
        var cardNumber2 = $("#CardNumber2").val();
        var cardNumber3 = $("#CardNumber3").val();
        var cardNumber4 = $("#CardNumber4").val();
        var expireMonth = $("#ExpireMonth").val();
        var expireYear = $("#ExpireYear").val();
        var cardholderName = $("#CardholderName").val();
        var cardCode = $("#CardCode").val();
        var rewardUseAllPoint;

        if (this.checked) {
            rewardUseAllPoint = "on";
        }
        switch (bankId) {
            case '1'://Yapı Kredi
                if (validationCardNumber(cardNumber1) && validationCardNumber(cardNumber2) && validationCardNumber(cardNumber3) && validationCardNumber(cardNumber4) && validationExpireDate(expireMonth) && validationExpireDate(expireYear)) {
                    moneyPointInstallment(cardNumber1, cardNumber2, cardNumber3, cardNumber4, cardholderName, expireMonth, expireYear, cardCode, rewardUseAllPoint);
                }
                else {
                    $("#frmData").submit();
                }
                break;
            case '2'://Akbank
                if (validationCardNumber(cardNumber1) && validationCardNumber(cardNumber2) && validationCardNumber(cardNumber3) && validationCardNumber(cardNumber4) && validationExpireDate(expireMonth) && validationExpireDate(expireYear) && validationCardHolder(cardholderName)) {
                    moneyPointInstallment(cardNumber1, cardNumber2, cardNumber3, cardNumber4, cardholderName, expireMonth, expireYear, cardCode, rewardUseAllPoint);
                }
                else {
                    $("#frmData").submit();
                }
                break;
            case '3'://Garanti
                if (validationCardNumber(cardNumber1) && validationCardNumber(cardNumber2) && validationCardNumber(cardNumber3) && validationCardNumber(cardNumber4) && validationExpireDate(expireMonth) && validationExpireDate(expireYear)) {
                    moneyPointInstallment(cardNumber1, cardNumber2, cardNumber3, cardNumber4, cardholderName, expireMonth, expireYear, cardCode, rewardUseAllPoint);
                }
                else {
                    $("#frmData").submit();
                }
                break;
            case '4'://İş Bank
                if (validationCardNumber(cardNumber1) && validationCardNumber(cardNumber2) && validationCardNumber(cardNumber3) && validationCardNumber(cardNumber4) && validationExpireDate(expireMonth) && validationExpireDate(expireYear)) {
                    moneyPointInstallment(cardNumber1, cardNumber2, cardNumber3, cardNumber4, cardholderName, expireMonth, expireYear, cardCode, rewardUseAllPoint);
                }
                else {
                    $("#frmData").submit();
                }
                break;
            case '6'://Halkbank
                if (validationCardNumber(cardNumber1) && validationCardNumber(cardNumber2) && validationCardNumber(cardNumber3) && validationCardNumber(cardNumber4) && validationExpireDate(expireMonth) && validationExpireDate(expireYear)) {
                    moneyPointInstallment(cardNumber1, cardNumber2, cardNumber3, cardNumber4, cardholderName, expireMonth, expireYear, cardCode, rewardUseAllPoint);
                }
                else {
                    $("#frmData").submit();
                }
                break;
            case '7'://HSCBC
                if (validationCardNumber(cardNumber1) && validationCardNumber(cardNumber2) && validationCardNumber(cardNumber3) && validationCardNumber(cardNumber4) && validationExpireDate(expireMonth) && validationExpireDate(expireYear)) {
                    moneyPointInstallment(cardNumber1, cardNumber2, cardNumber3, cardNumber4, cardholderName, expireMonth, expireYear, cardCode, rewardUseAllPoint);
                }
                else {
                    $("#frmData").submit();
                }
                break;
        }
    });

    $("#CheckBonus").on("click", function (e) {
        var cardNumber1 = $("#CardNumber1").val();
        var cardNumber2 = $("#CardNumber2").val();
        var cardNumber3 = $("#CardNumber3").val();
        var cardNumber4 = $("#CardNumber4").val();
        var expireMonth = $("#ExpireMonth").val();
        var expireYear = $("#ExpireYear").val();
        var cardholderName = $("#CardholderName").val();
        var cardCode = $("#CardCode").val();

        switch (bankId) {
            case '1'://Yapı Kredi
                if (validationCardNumber(cardNumber1) && validationCardNumber(cardNumber2) && validationCardNumber(cardNumber3) && validationCardNumber(cardNumber4) && validationExpireDate(expireMonth) && validationExpireDate(expireYear)) {
                    moneyPoint(cardNumber1, cardNumber2, cardNumber3, cardNumber4, cardholderName, expireMonth, expireYear, cardCode);
                }
                else {
                    $("#frmData").submit();
                }
                break;
            case '2'://Akbank
                if (validationCardNumber(cardNumber1) && validationCardNumber(cardNumber2) && validationCardNumber(cardNumber3) && validationCardNumber(cardNumber4) && validationExpireDate(expireMonth) && validationExpireDate(expireYear) && validationCardHolder(cardholderName)) {
                    moneyPoint(cardNumber1, cardNumber2, cardNumber3, cardNumber4, cardholderName, expireMonth, expireYear, cardCode);
                }
                else {
                    $("#frmData").submit();
                }
                break;
            case '3'://Garanti
                if (validationCardNumber(cardNumber1) && validationCardNumber(cardNumber2) && validationCardNumber(cardNumber3) && validationCardNumber(cardNumber4) && validationExpireDate(expireMonth) && validationExpireDate(expireYear)) {
                    moneyPoint(cardNumber1, cardNumber2, cardNumber3, cardNumber4, cardholderName, expireMonth, expireYear, cardCode);
                }
                else {
                    $("#frmData").submit();
                }
                break;
            case '4'://İş Bank
                if (validationCardNumber(cardNumber1) && validationCardNumber(cardNumber2) && validationCardNumber(cardNumber3) && validationCardNumber(cardNumber4) && validationExpireDate(expireMonth) && validationExpireDate(expireYear)) {
                    moneyPoint(cardNumber1, cardNumber2, cardNumber3, cardNumber4, cardholderName, expireMonth, expireYear, cardCode);
                }
                else {
                    $("#frmData").submit();
                }
                break;
            case '6'://Halkbank
                if (validationCardNumber(cardNumber1) && validationCardNumber(cardNumber2) && validationCardNumber(cardNumber3) && validationCardNumber(cardNumber4) && validationExpireDate(expireMonth) && validationExpireDate(expireYear)) {
                    moneyPoint(cardNumber1, cardNumber2, cardNumber3, cardNumber4, cardholderName, expireMonth, expireYear, cardCode);
                }
                else {
                    $("#frmData").submit();
                }
                break;
            case '7'://HSCBC
                if (validationCardNumber(cardNumber1) && validationCardNumber(cardNumber2) && validationCardNumber(cardNumber3) && validationCardNumber(cardNumber4) && validationExpireDate(expireMonth) && validationExpireDate(expireYear)) {
                    moneyPoint(cardNumber1, cardNumber2, cardNumber3, cardNumber4, cardholderName, expireMonth, expireYear, cardCode);
                }
                else {
                    $("#frmData").submit();
                }
                break;
        }
    });
});
function moneyPoint(cardNumber1, cardNumber2, cardNumber3, cardNumber4, cardholderName, expireMonth, expireYear, cardCode) {
    $.ajax({
        url: '/Payment/MoneyPoint',
        type: 'POST',
        cache: 'false',
        data: { CardNumber1: cardNumber1, CardNumber2: cardNumber2, CardNumber3: cardNumber3, CardNumber4: cardNumber4, CardholderName: cardholderName, ExpireMonth: expireMonth, ExpireYear: expireYear, CardCode: cardCode },
        success: function (result) {
            $("#CreditCardMoneyPoint").css("display", "block");
            $(".message-error").css("display", "none");
            $("#CreditCardMoneyPoint").html("");
            $("#CreditCardMoneyPoint").append(result);
        },
        error: function (request, status, err) {
            $("#CreditCardMoneyPoint").css("display", "none");
            $("#CreditCardMoneyPoint").html("");
        }
    });
}
function moneyPointInstallment(cardNumber1, cardNumber2, cardNumber3, cardNumber4, cardholderName, expireMonth, expireYear, cardCode, rewardUseAllPoint) {
    $.ajax({
        url: '/Payment/MoneyPointInstallment',
        type: 'POST',
        cache: 'false',
        data: { CardNumber1: cardNumber1, CardNumber2: cardNumber2, CardNumber3: cardNumber3, CardNumber4: cardNumber4, CardholderName: cardholderName, ExpireMonth: expireMonth, ExpireYear: expireYear, CardCode: cardCode, RewardUseAllPoint: rewardUseAllPoint },
        success: function (result) {
            //$("#CreditCardMoneyPoint").css("display", "block");
            //$(".message-error").css("display", "none");
            //$("#CreditCardMoneyPoint").html("");
            //$("#CreditCardMoneyPoint").append(result);

            $("#taksitTablosu").html("");
            $("#taksitTablosu").replaceWith(result);
        },
        error: function (request, status, err) {
            //$("#CreditCardMoneyPoint").css("display", "none");
            //$("#CreditCardMoneyPoint").html("");
            $("#taksitTablosu").html("");
        }
    });
}
function validationCardNumber(data) {
    if (data == 0 || data == '' || data == 'undefinided' || data == null || data.length < 4) {
        return false;
    }
    else {
        return true;
    }
}
function validationExpireDate(data) {
    if (data == 0 || data == '' || data == 'undefinided' || data == null) {
        return false;
    }
    else {
        return true;
    }
}
function validationCardHolder(data) {
    if (data == 0 || data == '' || data == 'undefinided' || data == null) {
        return false;
    }
    else {
        return true;
    }
}
function validationCvc(data) {
    if (data == 0 || data == '' || data == 'undefinided' || data == null) {
        return false;
    }
    else {
        return true;
    }
}
var bankId;

function checkCardNumber() {
    if ($("#CardNumber1").val().length == 4 &&
        $("#CardNumber2").val().length == 4 &&
        $("#CardNumber3").val().length == 4 &&
        $("#CardNumber4").val().length == 4) {
        $.ajax({
            url: '/Payment/CheckBinCode',
            type: 'POST',
            data: { binCode: $("#CardNumber1").val() + $("#CardNumber2").val() },
            success: function (data) {
                if (data == 0 || data == '' || data == 'undefinided' || data == null) {
                    $(".checkBonusHolder").css("display", "none");
                }
                else {
                    bankId = data;
                    $(".checkBonusHolder").css("display", "block");
                }
            },
            error: function (request, status, err) {
                $(".checkBonusHolder").css("display", "none");
            }
        });
    }
    else {
        $(".checkBonusHolder").css("display", "none");
    }
};