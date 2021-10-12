$.ajax({
    type: 'POST',
    url: $frm.attr('action'),
    data: $frm.serialize(),
    success: function (data, textStatus, xhr) {
        $('#Info').html(data);
        $('#ajaxResult').hide().html('Kayıt Eklendi').fadeIn(300, function () {
            var e = this;
            setTimeout(function () { $(e).fadeOut(400); }, 2500);
        });
        dlg.dialog('close');
        dlg.empty();
    },
    error: function (xhr, status) {
        if (xhr.status == 400)
            dlg.html(xhr.responseText, xhr.status);
        else if (xhr.status == 404) {
            dlg.dialog('close');
            dlg.empty();
        }
        else
            displayError(xhr.responseText, xhr.status);
    }
});