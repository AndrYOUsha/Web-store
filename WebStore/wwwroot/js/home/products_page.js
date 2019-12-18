//Отправляет запрос на сервер для добавления товара в сессию

var btn_name = "Добавить в корзину";
var btn_diff_name = "Удалить из корзины";

function buyProduct(element, id) {
    $.ajax({
        type: `POST`,
        url: `WorkWithSession`,
        data: { 'id': id },
        success: function (data) {
            $(`#basket_products`).text(data);

            if ($(element).attr('value') == btn_name)
                $(element).attr('value', btn_diff_name);
            else
                $(element).attr('value', btn_name);
        },
        error: function () {
            alert(`Произошёл сбой!`);
        }
    });
}