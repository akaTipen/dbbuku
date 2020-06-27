var oTable;

function loadData() {
    if (typeof oTable == 'undefined') {
        var handlerUrl = $.rootDir + 'api/API_Buku';
        oTable = $('#tblBuku').DataTable({
            "order": [],
            "select": {
                style: 'single'
            },
            "jQueryUI": true,
            "ajax": {
                type: "GET",
                url: handlerUrl,
                dataSrc: ''
            },
            "columns": [
                { "data": "ID" },
                { "data": "Nama" },
                { "data": "Harga" },
            ],
        });
    }
    else {
        oTable.ajax.reload();
    }
}

$(function () {
    loadData();

    $("#BukuForm").submit(function () {
        saveData();
        return false;
    })
});

function addData() {
    $('#modalTitleBuku').html('Add Buku');

    viewAddEditData('');

    DisableInputControl("form-input", false);
}

function editData() {
    $('#modalTitleBuku').html('Edit Buku');

    viewAddEditData('1');

    DisableInputControl("form-input", false);
}

function viewAddEditData(pID) {
    $("input[id$='txtName']").val("");
    $("input[id$='txtHarga']").val("");

    var hf = $("input[id$='hf']");

    if (pID == '') {
        hf.val(pID);
    }
    else {
        var count = oTable.rows({ selected: true }).count();
        if (count > 0) {
            var data = oTable.rows({ selected: true }).data();
            pID = data[0]["ID"];

            hf.val(pID);
            $.ajax({
                type: "GET",
                url: $.rootDir + "api/API_Buku/" + pID,
                async: true,
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (typeof (data) != "undefined") {
                        var msg = data;

                        var txtName = $("input[id$='txtName']");
                        txtName.val(msg.Nama);

                        var txtHarga = $("input[id$='txtHarga']");
                        txtHarga.val(msg.Harga);
                    }
                },
                error: GetDataError
            });
        }
        else {
            alert(SELECT_DATA);
            return;
        }
    }

    $('#modalBuku').modal('show');
}

function GetDataError(xhr, status, error) {
    alert(FETCHING_FAILED)
}

function deleteData() {
    var a = confirm("Yakin hapus?");
    if (a) {
        var count = oTable.rows({ selected: true }).count();
        if (count > 0) {
            var data = oTable.rows({ selected: true }).data();
            var pID = data[0]["ID"];

            $.ajax({
                type: "DELETE",
                url: $.rootDir + "api/API_Buku/" + pID,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    oTable.ajax.reload();
                    alert('Hapus berhasil');
                },
                error: function (data, status, xhr) {
                    alert('Error')
                },
            });
        } else {
            alert('error')
        }
    }
}

function saveData() {
    var hf = $("input[id$='hf']");
    var txtName = $("input[id$='txtName']");
    var txtHarga = $("input[id$='txtHarga']");

    var o = {};
    var Purl = '';
    var PType = '';
    var message = '';
    if (hf.val() == '') {
        o.Id = 0;
        Purl = $.rootDir + "api/API_Buku";
        PType = "POST";
        message = 'Berhasil Submit';
    }
    else {
        o.Id = hf.val();
        Purl = $.rootDir + "api/API_Buku/" + hf.val();
        PType = "PUT";
        message = 'Berhasil Update';
    }

    o.Nama = txtName.val();
    o.Harga = txtHarga.val();

    $.ajax({
        type: PType,
        url: Purl,
        data: JSON.stringify(o),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data, status, xhr) {
            alert(message)
            closeDialog();
        },
        error: function (data, status, xhr) {
            alert('Error')
        },
    });
}

function GetDataSuccess(data, status, xhr) {
    if (typeof (data) != "undefined") {
        var msg = data;

        var txtName = $("input[id$='txtName']");
        txtName.val(msg.BUSBUName);

        var cbxStatus = $("input[id$='cbxStatus']");
        if (msg.Status == true)
            cbxStatus.prop('checked', true);
        else
            cbxStatus.prop('checked', false);
    }
};

function GetDataError(xhr, status, error) {
    alert(FETCHING_FAILED, 0);
}

function closeDialog() {
    $('#modalBuku').modal('hide');

    loadData();
}