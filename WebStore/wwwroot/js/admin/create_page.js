//Сценарии для страницы Create.cshtml контроллера AdminController.cs

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

//Добавляет форму для загруки картинки на странице создания товара
function addFormFile(element) {
    $(element).parent(":last").append(
        `<div class="form-group ml-1 ml-sm-5 w-100 row">
                <input type="file" name="uploadedFiles" class="form-control-file col-6 col-md-4" />
                <input type="button" onclick="deleteFormFile(this)" class="btn btn-outline-danger ml-1 col-4 col-md-3" value="Удалить форму" />
            </div>`);
}

//Удаляет форму для загруки картинки на странице создания товара
function deleteFormFile(element) {
    $(element).parent().remove();
}
