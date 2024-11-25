var config = {
    DownloadReconciledReport: document.getElementById('DownloadReconciledReport').getAttribute('data-url'),
    ReconcileData: document.getElementById('ReconcileData').getAttribute('data-url'),
    ReconciledReport: document.getElementById('ReconciledReport').getAttribute('data-url')
};

//document.getElementById("downloadForm").addEventListener("submit", function () {
//    $('#loading-dialog').show();
//    var endDate = document.getElementById("endDate").value;
//    var action = config.DownloadReconciledReport + '?&EndDate=' + encodeURIComponent(endDate);
//    this.action = action;
//    $('#loading-dialog').hide();
//});

document.getElementById("downloadForm").addEventListener("submit", function (event) {
    event.preventDefault(); // Prevent the default form submission
    /*$('#loading-dialog').show();*/ // Show the loading dialog
    showLoadingDialog();

    var endDate = document.getElementById("endDate").value;
    var action = config.DownloadReconciledReport + '?EndDate=' + encodeURIComponent(endDate);

    // AJAX request to get the file
    $.ajax({
        url: action,
        method: 'GET',
        xhrFields: {
            responseType: 'blob' // Set the response type to blob for file download
        },
        success: function (data, status, xhr) {
            var filename = "";
            var disposition = xhr.getResponseHeader('Content-Disposition');

            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
            }

            var a = document.createElement('a');
            var url = window.URL.createObjectURL(data);
            a.href = url;
            a.download = filename || "download.xlsx";
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
            a.remove();
        },
        error: function () {
            alert("There was an error downloading the file.");
        },
        complete: function () {
            /* $('#loading-dialog').hide(); */// Hide the loading dialog
            hideLoadingDialog();
        }
    });
});

//$(document).ready(function () {
//    $('#btnReconc').click(function () {
//        // Show loading spinner
//        $('#loading-dialog').show();

//        var tableData = $('#example2').DataTable().data().toArray();
//        $.ajax({
//            url: config.ReconcileData,
//            type: 'POST',
//            contentType: 'application/json',
//            data: JSON.stringify(tableData),
//            success: function (response) {
//                console.log(response);
//                // Hide loading spinner on success
//                $('#loading-dialog').hide();
//                // Handle success response
//                alert('Data reconciled successfully!');
//                window.location.href = "ReconciledReport";
//            },
            //error: function (xhr, status, error) {
            //    // Hide loading spinner on error
            //    $('#loading-dialog').hide();
            //    // Handle error response
                //console.error(error);
//            }
//        });
//    });
//});

$(document).ready(function () {
    $('#btnReconc').click(function () {
        // Show loading spinner
        /* $('#loading-dialog').show();*/
        showLoadingDialog();

        var endDate = $('#endDate').val();

        var requestData = {
            EndDate: endDate
        };
        console.log(requestData);

        $.ajax({
            url: '/Home/ReconcileData', // Update with your actual controller route
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(requestData),
            success: function (response) {
                console.log(response);
                // Hide loading spinner on success
                /*$('#loading-dialog').hide();*/
                hideLoadingDialog();
                // Handle success response
                alert('Data reconciled successfully!');
                window.location.href = "ReconciledReport";
            },
            error: function (xhr, status, error) {
                // Hide loading spinner on error
                /*$('#loading-dialog').hide();*/
                hideLoadingDialog();
                // Handle error response
                console.error("Error:", error);
                console.error("Status:", status);
                console.error("XHR:", xhr.responseText);
                alert("Error: " + error);
            }
        });
    });
});


function getAmortizationSummary() {
    /*$("#loading-dialog").show();*/
    showLoadingDialog();
    document.getElementById('body').innerHTML = "";
    document.getElementById('body').innerHTML =
        '<table id="example2" class="table table-striped table-bordered dt-responsive nowrap display" style="width:100%" cellspacing="0"></thread></table>'
    //giveallbranch()
    //var serialNumber = 1;
    //var token = '@Html.AntiForgeryToken()';
    //token = $(token).val();
    $.ajax({
        url: config.ReconciledReport,
        //data: { __RequestVerificationToken: token, "StartDate": $("#startDate").val(), "EndDate": $("#endDate").val() },
        data: { "EndDate": $("#endDate").val() },
        type: 'POST',
        dataType: 'json',
        async: true,
        success: function (response) {
            //const queryString = 'StartDate=' + document.getElementById("startDate").value + '&EndDate=' + document.getElementById("endDate").value;
            const queryString = '&EndDate=' + document.getElementById("endDate").value;
            //"param1=value1&param2=value2";
            //var action = "/User/DownloadExcel?" + queryString;
            //document.getElementById("AbdheshDynamic").innerHTML = '<form id="downloadForm" action="' + action + '" enctype="multipart/form-data" method="post"><button type = "submit" class="btn" style = "background-color:#018db0;color:white;" id = "btnExport" ><span class="badge bg-teal">.xlsx </span><i class="fa fa-arrow-circle-o-down" style = "color:white"> </i> Download Table Data</button></form>'
            console.log(response);

            //document.getElementById('body').style.display = 'block';


            $(function () {
                $('#example1').DataTable()
                $('#example2').DataTable({
                    bDestroy: true,
                    autoWidth: true,
                    processing: true,
                    searchDelay: 150,
                    aoColumnDefs: [{
                        aTargets: [0]
                    }],
                    aaSorting: [[0, 'asc']],
                    //Paging
                    iDisplayLength: 10,
                    lengthMenu: [[10, 25, 50, 100, 500, -1], [10, 25, 50, 100, 500, 'All']],
                    data: response,
                    columns: [
                        {
                            title: 'SNO',
                            //data: null,
                            render: function (data, type, row, meta) {
                                // Assuming meta.row is the row index
                                //return meta.row + 1;
                                //return serialNumber++;
                                return meta.row + meta.settings._iDisplayStart + 1;



                            }
                        },
                        {
                            title: 'Reconciled Status',
                            data: 'reconStatus',
                            render: function (data, type, row, meta) {
                                return row.reconStatus


                            }
                        },
                        {
                            title: 'Tran Date',
                            data: 'traN_DATE',
                            render: function (data, type, row, meta) {
                                /*return row.traN_DATE*/

                                var date = new Date(data);
                                var formattedDate = date.toLocaleDateString();
                                return formattedDate;
                            }
                        },
                        {
                            title: 'Value Date',
                            data: 'valuE_DATE',
                            render: function (data, type, row, meta) {
                                /*return row.valuE_DATE*/

                                var date = new Date(data);
                                var formattedDate = date.toLocaleDateString();
                                return formattedDate;

                            }
                        },
                        {
                            title: 'Part Tran Type',
                            data: 'parT_TRAN_TYPE',
                            render: function (data, type, row, meta) {
                                return row.parT_TRAN_TYPE
                            }
                        },
                        {
                            title: 'Tran Particular',
                            data: 'traN_PARTICULAR',
                            render: function (data, type, row, meta) {
                                return row.traN_PARTICULAR
                            }
                        },
                        {
                            title: 'Tran Particular 2',
                            data: 'traN_PARTICULAR_2',
                            render: function (data, type, row, meta) {
                                return row.traN_PARTICULAR_2
                            }
                        },
                        {
                            title: 'Tran Id',
                            data: 'transactioN_ID',
                            render: function (data, type, row, meta) {

                                return row.transactioN_ID

                            }
                        },
                        {
                            title: 'Tran Amount',
                            data: 'traN_AMT',
                            render: function (data, type, row, meta) {
                                // return row.traN_AMT
                                return parseFloat(data).toFixed(2);
                                //var formattedAmount = parseFloat(data).toFixed(2);
                                //// Example condition: Change color to red if amount is negative
                                //var color = parseFloat(data) < 1000 ? 'red' : 'black';
                                //return '<span style="color: ' + color + '">' + formattedAmount + '</span>';


                            }
                        },
                        {
                            title: 'Entry User Id',
                            data: 'entrY_USER_ID',
                            render: function (data, type, row, meta) {
                                return row.entrY_USER_ID
                            }
                        },
                        {
                            title: 'Posted User Id',
                            data: 'pstD_USER_ID',
                            render: function (data, type, row, meta) {
                                return row.pstD_USER_ID
                            }
                        },
                        {
                            title: 'Posted Time',
                            data: 'pstD_DATE',
                            render: function (data, type, row, meta) {
                                /*return row.pstD_DATE*/

                                var date = new Date(data);
                                var formattedDate = date.toLocaleDateString();
                                return formattedDate;
                            }
                        }

                    ],
                    scrollY: "425px",
                    scrollX: true,
                    scrollCollapse: true,
                    fixedColumns: {
                        leftColumns: 3,
                        rightColumns: 1
                    },
                    //Output
                    dom: 'Blfrtip',
                    buttons: [
                        'excel', 'print'
                    ],
                })
                hideLoadingDialog();
                /*$("#loading-dialog").hide();*/
            });

        },
        error: function (xhr, status, error) {
            // Hide loading spinner on error
            /*$('#loading-dialog').hide();*/
            hideLoadingDialog();
            // Handle error response
            console.error(error);
        }

    })

}

function showLoadingDialog() {
    document.getElementById('loadingDialog').classList.remove('hidden');
}

// Function to hide loading dialog
function hideLoadingDialog() {
    document.getElementById('loadingDialog').classList.add('hidden');
}