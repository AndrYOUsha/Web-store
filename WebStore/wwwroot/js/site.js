// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var indx = 0;

//Создаёт строки и шапку для добавления характеристик
$('#btnCreatePC').click(function () {
    if (indx === 0)
        $('#tableCreatePC > thead').append(
            `<tr id="tableHeaders">
                <th>Артикль</th >
                <th>Брэнд</th>
                <th>Ветвь</th>
                <th>Цвет</th>
                <th>Название</th>
                <th>Пол</th>
                <th>Размер</th>
                <th>Размер международный</th>
                <th>Размер в строку</th>
                <th>Тип</th>
                <th>Количество</th>
                </tr >`
        );
    $('#tableCreatePC > tbody:last').append(
        ` <tr id="stroke_` + indx + `">
              <td><input style="width: 100px" class="col form-control" type="text" name="Characteristic[` + indx + `].Article" /></td >
              <td><input style="width: 100px" class="col form-control" type="text" name="Characteristic[` + indx + `].Brand" /></td>
              <td><input style="width: 100px" class="col form-control" type="text" name="Characteristic[` + indx + `].Brunch" /></td>
              <td><input style="width: 100px" class="col form-control" type="text" name="Characteristic[` + indx + `].Color" /></td>
              <td><input style="width: 100px" class="col form-control" type="text" name="Characteristic[` + indx + `].FullName" /></td>
              <td>
                  <select name="Characteristic[` + indx + `].Gender" style="width: 100px" class="col form-control">
                      <option value="">Значение не выбрано</option>
                      <option value="Male">Male</option>
                      <option value="Female">Female</option>
                      <option value="Unisex">Unisex</option>
                  </select>
              </td>
              <td><input style="width: 100px" class="col form-control" type="text" name="Characteristic[` + indx + `].Size" /></td>
              <td>
                  <select name="Characteristic[` + indx + `].SizeISS" style="width: 100px" class="col form-control">
                      <option value="">Значение не выбрано</option>
                      <option value="XS">XS</option>
                      <option value="S">S</option>
                      <option value="M">M</option>
                      <option value="L">L</option>
                      <option value="XL">XL</option>
                      <option value="XXL">XXL</option>
                      <option value="XXXL">XXXL</option>
                  </select>
              </td>
              <td><input style="width: 100px" class="col form-control" type="text" name="Characteristic[` + indx + `].SizeString" /></td>
              <td><input style="width: 100px" class="col form-control" type="text" name="Characteristic[` + indx + `].Type" /></td>
              <td><input style="width: 100px" class="col form-control" type="text" name="Characteristic[` + +indx++ + `].Count" /></td>
              <td><input type="button" id="delete_item" class="btn btn-outline-danger" value="Удалить строку" /></td>
              </tr >`
    );
});

//Удаляет строку таблицы и удаляет шапку таблицы, если нет элементов для заполнения
$(document).on('click', '#delete_item', function () {
    $(this).parent().parent().remove();
    
    recalculateItems();

    if (indx === 0)
        $('#tableHeaders').remove();
});

//Меняет имена, чтобы индекс начинался от 0 и увеличивался на один в дальнейшем
function recalculateItems() {
    let index = 0;
    let lenght_tr = $('#tableCreatePC > tbody > tr').length;
    let lenght_td = $('#tableCreatePC > tbody > tr:eq(0)').find('[name]').length;
    let start;
    let end;
    let str;


    let inputs = new Array(lenght_tr);

    for (let i = 0; i < inputs.length; i++) {

        inputs[i] = new Array(lenght_td);

        for (let j = 0; j < inputs[i].length; j++) {
            inputs[i][j] = $('#tableCreatePC > tbody > tr:eq(' + i + ')').find('[name]:eq(' + j + ')').attr('name');

            start = inputs[i][j].indexOf('[') + 1;
            end = inputs[i][j].indexOf(']');
            str = inputs[i][j].slice(start, end);

            inputs[i][j] = inputs[i][j].replace(str, index);

            $('#tableCreatePC > tbody > tr:eq(' + i + ')').find('[name]:eq(' + j + ')').attr('name', inputs[i][j]);
        }
        index = +index + 1;
    }
    updateIndex(index);
}

//Обновляет индекс элемента
function updateIndex(newIndex) {
    indx = newIndex;
}

//Создаёт инпуты и вносит в них данные для редактирования из существующих в таблице и удалется две кнопки "Редактировать" и "Удалить"
var dataHtml;
function editCharacteristic(element) {
    dataHtml = $('#tableCharacteristic').html();
    $(element).parent().parent().find(".item").html(function () {
        let newhtml;
        if ($(this).data('name') == 'Gender') {
            newhtml = `<select name="characteristic.${$(this).data("name")}" style="width: 100px" class="form-control">
                          <option value="${$(this).text()}">${$(this).text()}</option>
                          <option value="">Значение не выбрано</option>
                          <option value="Male">Male</option>
                          <option value="Female">Female</option>
                          <option value="Unisex">Unisex</option>
                      </select>`;
        }
        else if ($(this).data('name') == 'SizeISS') {
            newhtml = `<select name="characteristic.${$(this).data("name")}" style="width: 100px" class="form-control">
                           <option value="${$(this).text()}">${$(this).text()}</option>
                           <option value="">Значение не выбрано</option>
                           <option value="XS">XS</option>
                           <option value="S">S</option>
                           <option value="M">M</option>
                           <option value="L">L</option>
                           <option value="XL">XL</option>
                           <option value="XXL">XXL</option>
                           <option value="XXXL">XXXL</option>
                       </select>`;
        }
        else {
            newhtml = `<input style="width: 100px" class="col form-control" type="text" name="characteristic.${$(this).data("name")}" value="${$(this).text()}"/>`;
        }
        return newhtml;
    });
    $("input[value*='Редактировать']").remove();
    $("#tableCharacteristic > tbody > tr > td:contains('Удалить')").remove();
    $('#cancelCharacteristic').fadeIn(0);
    $('#updateCharacteristic').fadeIn(0);
    $('#backCharacteristic').fadeOut(0);
    $('#addCharacteristic').fadeOut(0);
}

//Отменяет редактирование
function cancelEdit() {
    $('#tableCharacteristic').html(dataHtml);
    $('#cancelCharacteristic').fadeOut(0);
    $('#updateCharacteristic').fadeOut(0);
    $('#backCharacteristic').fadeIn(0);
    $('#addCharacteristic').fadeIn(0);
}

//Добавляет форму для загруки картинки на странице создания товара
function addFormFile(element) {
    $(element).parent(":last").append(
           `<div class="form-group ml-5 row">
                <input type="file" name="uploadedFiles" class="form-control-file col-5" />
                <input type="button" onclick="deleteFormFile(this)" class="btn btn-outline-danger ml-1 col-2" value="Удалить форму" />
            </div>`);
}

//Удаляет форму для загруки картинки на странице создания товара
function deleteFormFile(element) {
    $(element).parent().remove();
}
