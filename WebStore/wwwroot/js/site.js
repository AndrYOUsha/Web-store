// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    if (document.cookie.indexOf(`quantity_products`) > 0)
        $(`#basket_products`).text(getCookie('quantity_products'));
});

function getCookie(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

function pushme(element) {
    $.ajax({
        type: `POST`,
        url: `Home/WorkWithSession`,
        data: { 'purchase': 'data' },
        success: function (data) {
            $(`#basket_products`).text(data);
        },
        error: function () {
            alert(`Произошёл сбой!`);
        }
    });
}

