function changeStatus(id, status) {
    Swal.fire({
        title: 'Kayıt durumunu değiştirmek istediğinize emin misiniz?',
        showDenyButton: false,
        showCancelButton: true,
        confirmButtonText: 'Evet, Değiştir',
        cancelButtonText: 'İptal',
    })
        .then((willDelete) => {
            if (willDelete.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    datatype: "json",
                    url: '/users/active-passive-user',
                    data: {
                        Id: id,
                        IsActive: status,
                        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
                    },
                    success: function (data) {
                        if (data.success) {
                            swal.fire('Başarılı', 'Kayıt başarıyla güncellendi?', 'success');
                            $('#customers').DataTable().ajax.reload();
                        }
                        else {
                            swal.fire('Hata Oluştu!', 'İşlem sırasında hata oluştu?', 'warning');
                            return false;
                        }
                    }
                });
            } else {
                swal.fire("İşlem iptal edildi.");
            }
        });
}

function changeApprove(id, status) {
    Swal.fire({
        title: 'Kayıt durumunu değiştirmek istediğinize emin misiniz?',
        showDenyButton: false,
        showCancelButton: true,
        confirmButtonText: 'Evet, Değiştir',
        cancelButtonText: 'İptal',
    })
        .then((willDelete) => {
            if (willDelete.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    datatype: "json",
                    url: '/users/approve-user',
                    data: {
                        Id: id,
                        IsApproved: status,
                        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
                    },
                    success: function (data) {
                        if (data.success) {
                            swal.fire('Başarılı', 'Kayıt başarıyla güncellendi?', 'success');
                            $('#customers').DataTable().ajax.reload();
                        }
                        else {
                            swal.fire('Hata Oluştu!', 'İşlem sırasında hata oluştu?', 'warning');
                            return false;
                        }
                    }
                });
            } else {
                swal.fire("İşlem iptal edildi.");
            }
        });
}

function openChangePasswordModal(id) {
    var url = "/change-password-from-admin?Id=" + id;

    let token = $('input[name="__RequestVerificationToken"]').val();
    let headers = { "RequestVerificationToken": token };

    $.ajax({
        type: 'GET',
        datatype: "json",
        url: url,
        headers: headers,
        success: function (data) {
            $('#modal-change-password-data').html(data);

            $('#modal-change-password').modal('show');
        },
        error: function (xhr, textStatus, errorThrown) {
            alert(xhr.responseText);
        }
    });
}

$(document).ready(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $('#customers').DataTable({
        "processing": true,
        "serverSide": true,
        "pageLength": 25,
        "headers": {
            'CSRFToken': token
        },
        "ajax": {
            "url": "/customers",
            "type": "POST",
            "data": {
                __RequestVerificationToken: token
            }
        },
        "columns": [
            { "data": "Id", "name": "Id", "autoWidth": true },
            {
                
                "data": "Name", "name": "Name", "autoWidth": true,
                render: function () {
                    return Html.Encode(row.Name);
                }
            },
            {
                "data": "Surname", "name": "Surname", "autoWidth": true,
            },
            { "data": "CompanyName", "name": "CompanyId", "autoWidth": true },
            {
                "data": null, "name": "CityId", "autoWidth": true,
                "render": function (data, type, row) {
                    return row.CityName + " / " + row.DistrictName
                }
            },
            { "data": "EMailAddress", "name": "EMailAddress", "autoWidth": true },
            {
                "data": "IsApproved", "name": "IsApproved", "autoWidth": true,
                "render": function (data, type, row) {
                    if (row.IsApproved == true) {
                        return 'Onaylı'
                    } else {
                        return 'Onay Bekliyor'
                    }
                }
            },
            {
                "data": "IsActive", "name": "IsActive", "autoWidth": true,
                "render": function (data, type, row) {
                    if (row.IsActive == true) {
                        return 'Aktif'
                    } else {
                        return 'Pasif'
                    }
                }
            },
            { "data": "InsertDate", "name": "InsertDate", "autoWidth": true },
            {
                'data': null,
                'render': function (data, type, row) {

                    return '<a href="/users/create-update?editId=' + row.Id + '" class="btn btn-outline-danger btn-sm"><em class="fa fa-edit"></em> Düzenle</a>';

                },
                "orderable": false
            },
            {
                'data': null,
                'render': function (data, type, row) {

                    var buttons = "";

                    if (row.IsActive == true) {
                        buttons = '<a href="#customers" onclick="changeStatus(' + row.Id + ', false)" class="btn btn-outline-success btn-sm"><em class="fa fa-check-square"></em> Pasifleştir</a>&nbsp;&nbsp;';
                    }
                    else {
                        buttons = '<a href="#customers" onclick="changeStatus(' + row.Id + ', true)" class="btn btn-outline-warning btn-sm"><em class="fa fa-square"></em> Aktifleştir</a>&nbsp;&nbsp;';
                    }

                    return buttons;
                },
                "orderable": false
            },
            {
                'data': null,
                'render': function (data, type, row) {

                    var buttons = "";

                    if (row.IsApproved) {
                        buttons = '<a href="#customers" onclick="changeApprove(' + row.Id + ', false)" class="btn btn-outline-secondary btn-sm"><em class="fa fa-check-square"></em> Onayı Kaldır</a>&nbsp;&nbsp;';
                    }
                    else {
                        buttons = '<a href="#customers" onclick="changeApprove(' + row.Id + ', true)" class="btn btn-outline-secondary btn-sm"><em class="fa fa-square"></em> Onayla</a>&nbsp;&nbsp;';
                    }

                    return buttons;
                },
                "orderable": false
            },
            {
                'data': null,
                'render': function (data, type, row) {

                    return '<a href="#customers" onclick="openChangePasswordModal(' + row.Id + ')" class="btn btn-outline-primary btn-sm"><em class="fa fa-key"></em> Şifre Değiştir</a>';

                },
                "orderable": false
            }
        ],
        columnDefs: [{
            targets: 8, render: function (data) {
                return moment(data).format('DD/MM/YYYY');
            }
        }],
        "language": {
            "url": "/js/datatables-tr.json"
        }
    });
});