function Delete(url, divId, sizeId, sizeName) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        // Hide the corresponding div based on the provided ID
                        $('#' + divId).hide();

                        // Move the removed size to the Add Sizes section
                        var newSizeHtml = '<div class="col-4 size-item" id="size-' + sizeId + '">' +
                            '<input class="form-check-input" type="checkbox" id="' + sizeName + '" value="true" name="MyCheckboxes_' + sizeId + '">' +
                            '<label class="form-check-label" style="color:black" for="' + sizeName + '">' + sizeName + '</label>' +
                            '</div>';
                        $('#add-sizes-section').append(newSizeHtml);

                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function () {
                    toastr.error("An error occurred while deleting the size.");
                }
            });
        }
    });
}