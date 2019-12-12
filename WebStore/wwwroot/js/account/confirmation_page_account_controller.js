//Сценарии для страницы Confirmation.cshtml контроллера AccountController.cs 

var second = 15;

function timer() {
    let timerId;
    $("#timerConfirmationPage").text(+second--);

    if (second < 0) {
        $("#confirmationPageBtn").prop("disabled", false);
        $("#timerConfirmationPage").parent().remove();
        clearTimeout(timerId);
    }
    else
        timerId = setTimeout(timer, 1000);
}

$(document).ready(timer());
