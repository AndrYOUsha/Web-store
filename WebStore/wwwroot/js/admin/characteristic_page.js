
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

