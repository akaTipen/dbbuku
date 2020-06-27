var oTable;

function loadData() {
    if (typeof oTable == 'undefined') {
        var handlerUrl = $.rootDir + 'api/API_Transaksi';
        oTable = $('#tblTransaksi').DataTable({
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
                { "data": "NamaBuku" },
                { "data": "Jumlah" },
                { "data": "Total" },
            ],
        });
    }
    else {
        oTable.ajax.reload();
    }
}

$(function () {
    loadData();
    GetBuku();

    $("#TansaksiForm").submit(function () {
        saveData();
        return false;
    })
});

function saveData() {
    var txtJumlah = $("input[id$='txtJumlah']");

    var o = {};
    o.BukuId = $('#ddlBuku').val();
    o.Jumlah = txtJumlah.val();

    $.ajax({
        type: "POST",
        url: $.rootDir + "api/API_Transaksi",
        data: JSON.stringify(o),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data, status, xhr) {
            alert("Berhasil Transaksi")
            $("input[id$='txtJumlah']").val("");
            var ddlBuku = $("select[id$='ddlBuku']");
            ddlBuku.attr('selectedIndex', 0);

            loadData();
        },
        error: function (data, status, xhr) {
            alert('Error')
        },
    });
}

function GetBuku() {
    var ddlBuku = $("select[id$='ddlBuku']");
    ddlBuku.empty().append('<option value="">' + SELECT + '</option>');

    $.ajax({
        type: "Get",
        url: $.rootDir + "Transaksi/GetBuku",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var val = data[i].ID;
                var text = data[i].Nama;
                ddlBuku.append(new Option(text, val));
            }

            //if (BUSBU != 0)
            //    ddlBUSBU.val(BUSBU).change();
            //else
            ddlBuku.attr('selectedIndex', 0);
        }
    });
}