﻿var dataTable;

$(document).ready(function () {
    var url = window.location.search;

    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    } else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        } else {
            if (url.includes("approved")) {
                loadDataTable("approved");
            } else {
                if (url.includes("pending")) {
                    loadDataTable("pending");
                } else {
                    loadDataTable("all");
                }
            }
        }
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: `/admin/order/getall?status=${status}`,
            dataSrc: function (json) {
                console.log(json);  // Check if the correct data is logged
                return json.data;   // Return the correct data array
            }
        },
        "columns": [
            {data: 'id', "width": "10%"},
            {data: 'name', "width": "15%"},
            {data: 'phoneNumber', "width": "10%"},
            {data: 'applicationUser.email', "width": "20%"},
            {data: 'orderStatus', "width": "10%"},
            {data: 'orderTotal', "width": "10%"},
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/admin/order/details?orderId=${data}" class="btn btn-primary mx-2">
                            <i class="bi bi-pencil-square"></i>
                        </a>
                    </div>`;
                },
                "width": "25%"
            }
        ]
    });
}

