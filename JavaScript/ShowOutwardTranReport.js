var config = {
    DownloadOutwardTranRecord: document.getElementById('DownloadOutwardTranRecord').getAttribute('data-url'),
    ShowOutwardTranReport: document.getElementById('ShowOutwardTranReport').getAttribute('data-url'),
};

document.getElementById("downloadForm").addEventListener("submit", function () {
    var startDate = document.getElementById("startDate").value;
    var endDate = document.getElementById("endDate").value;
    var action = config.DownloadOutwardTranRecord + '?StartDate=' + encodeURIComponent(startDate) + '&EndDate=' + encodeURIComponent(endDate);
    this.action = action;
});

document.addEventListener('DOMContentLoaded', function () {
    document.getElementById("getAmortizationSummary").addEventListener("click", function () {
        getAmortizationSummary();
    });
    getAmortizationSummary();
    function getAmortizationSummary() {
        $("#loading-dialog").show();
        document.getElementById('body').innerHTML = "";
        document.getElementById('body').innerHTML =
            '<table id="example2" class="table table-striped table-bordered dt-responsive nowrap display" style="width:100%" cellspacing="0"></thread></table>'
        //giveallbranch()
        //var serialNumber = 1;
        //var token = '@Html.AntiForgeryToken()';
        //token = $(token).val();
        $.ajax({
            url: config.ShowOutwardTranReport,
            data: {"StartDate": $("#startDate").val(), "EndDate": $("#endDate").val() },
            type: 'POST',
            dataType: 'json',
            async: true,
            success: function (response) {
                const queryString = 'StartDate=' + document.getElementById("startDate").value + '&EndDate=' + document.getElementById("endDate").value;
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
                                title: 'Direction',
                                data: 'direction',
                                render: function (data, type, row, meta) {
                                    return row.direction
                                }
                            },
                            {
                                title: 'Batch_Id',
                                data: 'batch_Id',
                                render: function (data, type, row, meta) {
                                    return row.batch_Id
                                }
                            },
                            {
                                title: 'Transaction_Id',
                                data: 'transaction_Id',
                                render: function (data, type, row, meta) {
                                    return row.transaction_Id
                                }
                            },
                            {
                                title: 'Date',
                                data: 'date',
                                render: function (data, type, row, meta) {
                                    var date = new Date(data);
                                    var formattedDate = date.toLocaleDateString();
                                    return formattedDate;
                                }
                            },
                            {
                                title: 'Amount',
                                data: 'amount',
                                render: function (data, type, row, meta) {

                                    //return row.amount
                                    return parseFloat(data).toFixed(2);

                                }
                            },
                            {
                                title: 'Debit_Status',
                                data: 'debit_Status',
                                render: function (data, type, row, meta) {
                                    return row.debit_Status


                                }
                            },
                            {
                                title: 'Debtor_Iso',
                                data: 'debtor_Iso',
                                render: function (data, type, row, meta) {
                                    return row.debtor_Iso
                                }
                            },
                            {
                                title: 'Credit_Status',
                                data: 'credit_Status',
                                render: function (data, type, row, meta) {
                                    return row.credit_Status
                                }
                            },
                            {
                                title: 'Creditor_Iso_Id',
                                data: 'creditor_Iso_Id',
                                render: function (data, type, row, meta) {
                                    return row.creditor_Iso_Id
                                }
                            },
                            {
                                title: 'Debtor_Bank',
                                data: 'debtor_Bank',
                                render: function (data, type, row, meta) {
                                    return row.debtor_Bank
                                }
                            },
                            {
                                title: 'Debtor_Branch',
                                data: 'debtor_Branch',
                                render: function (data, type, row, meta) {
                                    return row.debtor_Branch
                                }
                            },
                            {
                                title: 'Debtor_Account',
                                data: 'debtor_Account',
                                render: function (data, type, row, meta) {
                                    return row.debtor_Account
                                }
                            },
                            {
                                title: 'Debtor_Name',
                                data: 'debtor_Name',
                                render: function (data, type, row, meta) {
                                    return row.debtor_Name
                                }
                            },
                            {
                                title: 'Creditor_Bank',
                                data: 'creditor_Bank',
                                render: function (data, type, row, meta) {
                                    return row.creditor_Bank
                                }
                            },
                            {
                                title: 'Creditor_Branch',
                                data: 'creditor_Branch',
                                render: function (data, type, row, meta) {
                                    return row.creditor_Branch
                                }
                            },
                            {
                                title: 'Creditor_Account',
                                data: 'creditor_Account',
                                render: function (data, type, row, meta) {
                                    return row.creditor_Account
                                }
                            },
                            {
                                title: 'Creditor_Name',
                                data: 'creditor_Name',
                                render: function (data, type, row, meta) {
                                    return row.creditor_Name
                                }
                            },
                            {
                                title: 'Charge_Amount',
                                data: 'charge_Amount',
                                render: function (data, type, row, meta) {
                                    return row.charge_Amount
                                }
                            },
                            {
                                title: 'Charge_Liability',
                                data: 'charge_Liability',
                                render: function (data, type, row, meta) {
                                    return row.charge_Liability
                                }
                            },
                            {
                                title: 'Session_No',
                                data: 'session_No',
                                render: function (data, type, row, meta) {
                                    return row.session_No
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
                    $("#loading-dialog").hide();
                });

            }


        })

    }
});