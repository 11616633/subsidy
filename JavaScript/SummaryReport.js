// Initialize Flatpickr
//flatpickr("#reportDate", {
//    dateFormat: "d M Y",
//    maxDate: "today"
//});

//document.getElementById('exportButton').addEventListener('click', function () {
//    var table = document.getElementById('reconciliationTable');
//    var ws = XLSX.utils.table_to_sheet(table);
//    var wb = XLSX.utils.book_new();
//    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
//    XLSX.writeFile(wb, 'SummaryReport.xlsx');
//});

document.addEventListener('DOMContentLoaded', function () {
    // Initialize Flatpickr
    flatpickr("#reportDate", {
        dateFormat: "d M Y",
        maxDate: "today"
    });

    // Check if search button exists
    var searchButton = document.getElementById('searchButton');
    if (searchButton) {
        // Event listener for search button click
        searchButton.addEventListener('click', function (event) {
            event.preventDefault(); // Prevent default form submission
            var reportDate = document.getElementById('reportDate').value;

            if (reportDate) {
                // Show loading dialog
                showLoadingDialog();

                // Simulate form submission
                setTimeout(() => {
                    this.closest('form').submit();
                }, 1000); // Simulate delay before submitting form (replace with actual submission logic)
            } else {
                // Show error dialog if date is not selected
                showErrorDialog('Please select a date!');
            }
        });
    }

    // Check if export button exists
    var exportButton = document.getElementById('exportButton');
    if (exportButton) {
        // Event listener for export button click
        exportButton.addEventListener('click', function () {
            var table = document.getElementById('reconciliationTable');
            var ws = XLSX.utils.table_to_sheet(table);
            var wb = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
            XLSX.writeFile(wb, 'SummaryReport.xlsx');
        });
    }

    // Optional: Check if view details button exists
    var viewDetailsButton = document.getElementById('viewDetailsButton');
    if (viewDetailsButton) {
        // Event listener for view details button click
        viewDetailsButton.addEventListener('click', function () {
            // Handle view details action here
        });
    }

    // Function to show loading dialog
    function showLoadingDialog() {
        document.getElementById('loadingDialog').classList.remove('hidden');
    }

    // Function to hide loading dialog
    function hideLoadingDialog() {
        document.getElementById('loadingDialog').classList.add('hidden');
    }

    // Function to show error dialog
    function showErrorDialog(message) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: message
        });
    }
});


