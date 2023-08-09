var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll"
        },
        "columns": [
            { "data": "id", "widht": "5%" },
            { "data": "name", "widht": "25%" },
            { "data": "phoneNumber", "widht": "15%" },
            { "data": "applicationUser.email", "widht": "15%" },
            { "data": "orderStatus", "widht": "15%" },
            { "data": "orderTotal", "widht": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Order/Details?orderId=${data}" class="btn btn-primary mx-2">
                            <i class="bi bi-pencil-square"></i>
                        </a>
                    </div>
                    `
                },
                "widht": "5%"
            },
        ]
    });
}

