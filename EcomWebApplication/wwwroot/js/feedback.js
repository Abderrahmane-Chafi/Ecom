var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/FeedBack/GetAll"
        },
        "columns": [
            { "data": "applicationUser.userName", "widht": "15%" },
            { "data": "date", "widht": "15%" },
            { "data": "comment", "widht": "15%" },
        ]
    });
}

