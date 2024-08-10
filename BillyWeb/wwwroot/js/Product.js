﻿var dtbl;
(function () {
    debugger
    loadtable();
})();

function loadtable() {
    debugger
    var dtbl = $('#liststbl').DataTable({
        "ajax": {
            url: '/Admin/Product/GetAll',
            
        },
        "columns": [
            { data: 'title', "width": "20%" },
            { data: 'author', "width": "15%" },
            { data: 'isbn', "width": "15%" },
            { data: 'price', "width": "15%" },
            { data: 'categoryID', "width": "15%" },
            {
                data: 'id',
                "render": function (data) { 
                    return `<div class="w-76 btn-group" role="group">
		                    <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> Edit   </a> 
		                    <a onclick=del('/admin/product/delete/${data}') class="btn btn-danger mx-2"><i class="bi bi-trash-fill"></i> Delete   </a> 
	                        </div>`
                },
                "Width": "20%"
            }
        ]
    });
}

function del(url) { 

Swal.fire({
    title: "Are you sure?",
    text: "You won't be able to revert this!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#3085d6",
    cancelButtonColor: "#d33",
    confirmButtonText: "Yes, delete it!"
}).then((result) => {
    if (result.isConfirmed) {
        $.ajax({
            url: url,
            type: 'DELETE',
            success: function (data) {
                dtbl.ajax.reload();
                toastr.success(data.Message)
            },
            icon: "success"
        });
    }
});
}