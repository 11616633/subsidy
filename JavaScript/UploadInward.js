$(document).ready(function () {
    $('#uploadForm').submit(function (event) {
        event.preventDefault(); // Prevent default form submission

        var formData = new FormData(this);

        console.log('Form submitted'); // Debugging log
        showLoadingDialog(); // Show loading dialog

        $.ajax({
            url: $('#uploadForm').attr('action'),
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (response) {
                hideLoadingDialog(); // Hide loading dialog
                if (response.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Success',
                        text: response.message,
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.reload(); // Reload page or redirect
                        }
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: response.message,
                        confirmButtonText: 'OK'
                    });
                }
            },
            error: function (xhr, status, error) {
                hideLoadingDialog(); // Hide loading dialog
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'An unexpected error occurred.',
                    confirmButtonText: 'OK'
                });
            }
        });
    });
});

// Function to show loading dialog
function showLoadingDialog() {
    document.getElementById('loadingDialog').classList.remove('hidden');
}

// Function to hide loading dialog
function hideLoadingDialog() {
    document.getElementById('loadingDialog').classList.add('hidden');
}