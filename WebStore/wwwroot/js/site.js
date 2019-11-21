// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var indx = 0;

//Создаёт строки для добавления характеристик
$(document).ready(function () {
    $('#btnCreatePC').click(function () {
        if (indx === 0)
            $('#tableCreatePC > thead:last').append(
                `<tr id="tableHeaders">
                <th class="col-3">Артикль</th >
                <th class="col-3">Брэнд</th>
                <th class="col-3">Ветвь</th>
                <th class="col-3">Цвет</th>
                <th class="col-3">Название</th>
                <th class="col-3">Пол</th>
                <th class="col-3">Размер</th>
                <th class="col-3">Размер международный</th>
                <th class="col-3">Размер в строку</th>
                <th class="col-3">Тип</th>
                <th class="col-3">Количество</th>
                </tr >`
            );
        $('#tableCreatePC > tbody:last').append(
            ` <tr id="stroke_` + indx + `">
              <td class="col-3"><input type="text" name="Characteristic[` + indx + `].Article" /></td >
              <td class="col-3"><input type="text" name="Characteristic[` + indx + `].Brand" /></td>
              <td class="col-3"><input type="text" name="Characteristic[` + indx + `].Brunch" /></td>
              <td class="col-3"><input type="text" name="Characteristic[` + indx + `].Color" /></td>
              <td class="col-3"><input type="text" name="Characteristic[` + indx + `].FullName" /></td>
              <td class="col-3"><input type="text" name="Characteristic[` + indx + `].Gender" /></td>
              <td class="col-3"><input type="text" name="Characteristic[` + indx + `].Size" /></td>
              <td class="col-3"><input type="text" name="Characteristic[` + indx + `].SizeISS" /></td>
              <td class="col-3"><input type="text" name="Characteristic[` + indx + `].SizeString" /></td>
              <td class="col-3"><input type="text" name="Characteristic[` + indx + `].Type" /></td>
              <td class="col-3"><input type="text" name="Characteristic[` + +indx++ + `].Count" /></td>
              <td class="col-3"><input type="button" id="delete_item" class="btn btn-outline-danger" value="Удалить строку" /></td>
              </tr >`
        );
    });
});

//Удаляет строку таблицы и меняет имена, чтобы индекс начинался от 0 и увеличивался на один в дальнейшем
$(document).on('click', '#delete_item', function () {
    $(this).parent().parent().remove();

    recalculateItems();

    if (indx === 0)
        $('#tableHeaders').remove();
});

function recalculateItems() {
    let index = 0;
    let lenght_tr = $('#tableCreatePC > tbody > tr').length;
    let lenght_td = $('#tableCreatePC > tbody > tr:eq(0)').find(':text').length;
    let start;
    let end;
    let str;


    let inputs = new Array(lenght_tr);

    for (let i = 0; i < inputs.length; i++) {

        inputs[i] = new Array(lenght_td);

        for (let j = 0; j < inputs[i].length; j++) {
            inputs[i][j] = $('#tableCreatePC > tbody > tr:eq(' + i + ')').find(':text:eq(' + j + ')').attr('name');

            start = inputs[i][j].indexOf('[') + 1;
            end = inputs[i][j].indexOf(']');
            str = inputs[i][j].slice(start, end);

            inputs[i][j] = inputs[i][j].replace(str, index);

            $('#tableCreatePC > tbody > tr:eq(' + i + ')').find(':text:eq(' + j + ')').attr('name', inputs[i][j]);
        }
        index = +index + 1;
    }
    updateIndex(index);
}

function updateIndex(newIndex) {
    indx = newIndex;
}