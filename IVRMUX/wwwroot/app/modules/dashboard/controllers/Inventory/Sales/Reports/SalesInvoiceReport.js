
(function () {
    'use strict';
    angular
        .module('app')
        .controller('SalesInvoiceReportController', SalesInvoiceReportController);
    SalesInvoiceReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout','$filter'];
    function SalesInvoiceReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {

        $scope.isRequiredmi = function () {

            return !$scope.get_mi_list.some(function (options) {
                return options.miid;
            });

        };

        $scope.all_miid = function (ac) {
            $scope.allmi = ac;
            var toggleStatus = $scope.allmi;
            angular.forEach($scope.get_mi_list, function (itm) {
                itm.miid = toggleStatus;
            });
        };

        $scope.disposeQFlg = false;
        $scope.locationflg = false;
        $scope.selectionflag = "year";
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------
        $scope.onclickchange = function () {
            $scope.coYear ='';
            $scope.invmlO_Id ='';
            $scope.get_locations =[];
            $scope.selectionflag ="year";
            $scope.loaddata($scope.MI_Id, $scope.selectionflag)
        }
        //==========
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = paginationformasters;
        $scope.obj = {};
        $scope.loaddata = function () {
          
            $scope.search = "";
            var pageid = 1;
            apiService.getURI("SalesInvoiceReport/getloaddata", pageid).
                then(function (promise) {

                    $scope.get_mi_list = promise.get_mi_list;
                   

                });
        };



        $scope.reschedule1 = function (obj) {

            $scope.ttl = 0;
          
            //$('#popup111').modal('show');

            $scope.printstudents = [];
            angular.forEach(obj.LIST, function (dd) {
                $scope.ttl = $scope.ttl + dd.INVACI_CheckInQty;
                $scope.printstudents.push(dd);
                $scope.dcdate = dd.UpdatedDate;
                $scope.imagename = dd.MI_Logo;
                $scope.miname = dd.MI_Name;
            })

            $scope.tempto = [];
            angular.forEach($scope.printstudents, function (ddd) {

                if ($scope.tempto.length == 0) {
                    $scope.tempto.push({ INVMST_Id: ddd.INVMST_Id, INVMS_StoreName: ddd.INVMS_StoreName });
                }
                else if ($scope.tempto.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.tempto, function (opq1) {
                        if (opq1.INVMST_Id == ddd.INVMST_Id) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.tempto.push({ INVMST_Id: ddd.INVMST_Id, INVMS_StoreName: ddd.INVMS_StoreName });
                    }

                }
            })

            $scope.temptorecby = [];
            $scope.temptorecbyname = '';
            angular.forEach($scope.printstudents, function (ddd) {

                if ($scope.temptorecby.length == 0) {
                    $scope.temptorecby.push({ INVACI_ReceivedBy: ddd.INVACI_ReceivedBy });
                    $scope.temptorecbyname = ddd.INVACI_ReceivedBy;
                }
                else if ($scope.temptorecby.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.temptorecby, function (opq1) {
                        if (opq1.INVACI_ReceivedBy == ddd.INVACI_ReceivedBy) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.temptorecby.push({ INVACI_ReceivedBy: ddd.INVACI_ReceivedBy });
                        $scope.temptorecbyname = $scope.temptorecbyname + ' , ' + ddd.INVACI_ReceivedBy;
                    }

                }
            })



            $scope.decno = obj.INVACI_No;


            $('#popup1113').modal('show');

        }

        $scope.edit = function (item) {
            $scope.mainttl = 0;
            $scope.get_SaleItemDetails = [];
            $scope.get_SaleItemTax = [];
            $scope.ttl = 0;
            $scope.griddtl = false;
            $scope.editS = true;
            $scope.INVMSL_Id = item.INVMSL_Id;
            $scope.invmS_StoreName = item.INVMS_StoreName;
            $scope.client = item.ISMMCLT_ClientName;
            $scope.INVMSL_SalesValue = item.INVMSL_SalesValue;

            $scope.INVMSL_SalesDate = item.INVMSL_SalesDate;
            $scope.INVMSL_Remarks = item.INVMSL_Remarks;

            $scope.INVMSL_SalesNo = item.INVMSL_SalesNo;
            $scope.INVMSL_SalesNo = item.INVMSL_SalesNo;
            $scope.INVMSL_SalesDate = item.INVMSL_SalesDate;

            $scope.INVMSL_TotTaxAmt = item.INVMSL_TotTaxAmt;
            $scope.INVMSL_TotDiscount = item.INVMSL_TotDiscount;
            $scope.INVMSL_TotalAmount = item.INVMSL_TotalAmount;
            $scope.INVMSL_Remarks = item.INVMSL_Remarks;
            $scope.ISMMCLT_Address = item.ISMMCLT_Address;
            $scope.ISMMCLT_Desc = item.ISMMCLT_Desc;
            $scope.MI_Id = item.MI_Id;
            var avstock = 0;
            var data = {
                "INVMSL_Id": $scope.INVMSL_Id,
                "MI_Id": $scope.MI_Id
            };
            apiService.create("SalesInvoiceReport/getitemDetail", data).
                then(function (promise) {
                    debugger;
                    $scope.griddtl = true;
                    $scope.get_SaleItemDetails = promise.get_SaleItemDetails;
                    $scope.get_SaleItemTax = promise.get_SaleItemTax;

                    $scope.imagename = promise.instlist[0].MI_Logo;
                    $scope.miname = promise.instlist[0].MI_Name;

                    angular.forEach($scope.get_SaleItemDetails, function (ee) {

                        $scope.ttl = $scope.ttl + ee.invtsL_Amount;
                    });

                    $scope.masterTax = [];

                    angular.forEach($scope.get_SaleItemTax, function (ddd) {
                        if ($scope.masterTax.length == 0) {
                            $scope.masterTax.push({ INVMT_Id: ddd.invmT_Id, INVMT_TaxName: ddd.invmT_TaxName });

                        }
                        else if ($scope.masterTax.length > 0) {
                            var already_cnt = 0;
                            angular.forEach($scope.masterTax, function (opq1) {
                                if (opq1.INVMT_Id == ddd.invmT_Id) {
                                    already_cnt += 1;
                                }
                            })
                            if (already_cnt == 0) {
                                $scope.masterTax.push({ INVMT_Id: ddd.invmT_Id, INVMT_TaxName: ddd.invmT_TaxName });
                            }

                        }
                    })

                    angular.forEach($scope.masterTax, function (ff) {
                        var taxttl = 0
                        angular.forEach($scope.get_SaleItemTax, function (gg) {
                            if (ff.INVMT_Id == gg.invmT_Id) {
                                taxttl = taxttl + gg.invtslT_TaxAmt;
                            }


                        })
                        $scope.mainttl = $scope.mainttl + taxttl;
                        ff.Tl = taxttl;

                    })

                    $scope.mainttl = $scope.mainttl + $scope.ttl;

                    $scope.words = $scope.amountinwords($scope.mainttl);
                    
                });
            $('#popup111').modal('show');
        };


        $scope.reschedule = function (obj) {

            $scope.ttl = 0;
      
            //$('#popup111').modal('show');

            $scope.printstudents = [];
            angular.forEach(obj.LIST, function (dd) {
                $scope.ttl = $scope.ttl + dd.INVACO_CheckOutQty;
                $scope.printstudents.push(dd);
                $scope.dcdate = dd.UpdatedDate;

                $scope.imagename = dd.MI_Logo;
                $scope.miname = dd.MI_Name;

            })

            $scope.tempto = [];
            angular.forEach($scope.printstudents, function (ddd) {

                if ($scope.tempto.length == 0) {
                    $scope.tempto.push({ INVMLO_Id: ddd.INVMLO_Id, INVMLO_LocationRoomName: ddd.INVMLO_LocationRoomName });
                }
                else if ($scope.tempto.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.tempto, function (opq1) {
                        if (opq1.INVMLO_Id == ddd.INVMLO_Id) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.tempto.push({ INVMLO_Id: ddd.INVMLO_Id, INVMLO_LocationRoomName: ddd.INVMLO_LocationRoomName });
                    }

                }
            })

            $scope.temptorecby = [];
            $scope.temptorecbyname = '';
            angular.forEach($scope.printstudents, function (ddd) {

                if ($scope.temptorecby.length == 0) {
                    $scope.temptorecby.push({ INVACO_ReceivedBy: ddd.INVACO_ReceivedBy });
                    $scope.temptorecbyname = ddd.INVACO_ReceivedBy;
                }
                else if ($scope.temptorecby.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.temptorecby, function (opq1) {
                        if (opq1.INVACO_ReceivedBy == ddd.INVACO_ReceivedBy) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.temptorecby.push({ INVACO_ReceivedBy: ddd.INVACO_ReceivedBy });
                        $scope.temptorecbyname = $scope.temptorecbyname + ' , ' + ddd.INVACO_ReceivedBy;
                    }

                }
            })



            $scope.decno = obj.INVACO_No;


            $('#popup111').modal('show');

        }
        $scope.tempdc = [];
        //   ========================================== Assets Report
        $scope.getreport = function () {
            $scope.tempdc = [];
            $scope.submitted = true;
            debugger;
            $scope.get_salesdetails = [];
            if ($scope.myForm.$valid) {
                $scope.miarray = [];
                angular.forEach($scope.get_mi_list, function (rr) {
                    if (rr.miid == true) {
                        $scope.miarray.push({ MI_Id: rr.MI_Id });
                    }

                })
                $scope.start_Date = $filter('date')($scope.startdate, "yyyy-MM-dd");
                $scope.end_Date = $filter('date')($scope.enddate, "yyyy-MM-dd");
                if ($scope.bw_dates == true) {
                    $scope.sDate = $scope.start_Date;
                    $scope.eDate = $scope.end_Date;
                }
                else {
                    $scope.sDate = "";
                    $scope.eDate = "";
                }

                console.log($scope.miarray);
                var data = {};



                data = {
                    get_mi_arraylist: $scope.miarray,
                    "startdate": $scope.sDate,
                    "enddate": $scope.eDate,
                    "bwdateflag": $scope.bw_dates,
                };

                apiService.create("SalesInvoiceReport/savedetails", data).then(function (promise) {
                
                    if (promise.get_salesdetails.length > 0) {
                        $scope.get_salesdetails = promise.get_salesdetails;
                         
                        $scope.get_salesdetails = $scope.get_salesdetails;

                                $scope.tempdc = [];
                        angular.forEach($scope.get_salesdetails, function (ddd) {

                            if ($scope.tempdc.length == 0) {
                                $scope.tempdc.push({ INVMSL_Id: ddd.INVMSL_Id, INVMSL_SalesNo: ddd.INVMSL_SalesNo, INVMSL_SalesDate: ddd.INVMSL_SalesDate, INVMS_StoreName: ddd.INVMS_StoreName, ISMMCLT_ClientName: ddd.ISMMCLT_ClientName, ISMMCLT_Address: ddd.ISMMCLT_Address, ISMMCLT_Desc: ddd.ISMMCLT_Desc });
                                    }
                                    else if ($scope.tempdc.length > 0) {
                                        var already_cnt = 0;
                                        angular.forEach($scope.tempdc, function (opq1) {
                                            if (opq1.INVMSL_Id == ddd.INVMSL_Id) {
                                                already_cnt += 1;
                                            }
                                        })

                                        if (already_cnt == 0) {
                                            $scope.tempdc.push({ INVMSL_Id: ddd.INVMSL_Id, INVMSL_SalesNo: ddd.INVMSL_SalesNo, INVMSL_SalesDate: ddd.INVMSL_SalesDate, INVMS_StoreName: ddd.INVMS_StoreName});
                                        }

                                    }
                                })

                                $scope.mainarray = [];
                                angular.forEach($scope.tempdc, function (ss) {
                                    var listdat = [];
                                    angular.forEach($scope.get_salesdetails, function (ff) {
                                        if (ss.INVMSL_Id == ff.INVMSL_Id) {
                                            listdat.push(ff);
                                        }

                                    })
                                    ss.LIST = listdat;
                                })
                         
                           
                        }
                        else {
                            swal("No record found.... !");
                        }
                    
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.onrdochange = function () {

            $scope.get_AssetsReport = [];
            $scope.tempdc = [];
        }
        //======================================Print & Export to Excel
        $scope.exportToExcel = function (export_id) {
            var exportHref = "";
            exportHref = Excel.tableToExcel(export_id, 'printARL');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.printDataout = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.printDatain = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 10000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.printData = function () {
            var innerContents = "";
         
       
                innerContents = document.getElementById("printARL").innerHTML;
     
           


            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        };
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
        $scope.searchValue1 = '';

        //======Amount Convert into word=========//
        $scope.amountinwords = function convertNumberToWords(atotalc) {
            var words = new Array();
            words[0] = '';
            words[1] = 'One';
            words[2] = 'Two';
            words[3] = 'Three';
            words[4] = 'Four';
            words[5] = 'Five';
            words[6] = 'Six';
            words[7] = 'Seven';
            words[8] = 'Eight';
            words[9] = 'Nine';
            words[10] = 'Ten';
            words[11] = 'Eleven';
            words[12] = 'Twelve';
            words[13] = 'Thirteen';
            words[14] = 'Fourteen';
            words[15] = 'Fifteen';
            words[16] = 'Sixteen';
            words[17] = 'Seventeen';
            words[18] = 'Eighteen';
            words[19] = 'Nineteen';
            words[20] = 'Twenty';
            words[30] = 'Thirty';
            words[40] = 'Forty';
            words[50] = 'Fifty';
            words[60] = 'Sixty';
            words[70] = 'Seventy';
            words[80] = 'Eighty';
            words[90] = 'Ninety';
            atotalc = atotalc.toString();
            var atemp = atotalc.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++ , j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                atotalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        atotalc = n_array[i] * 10;
                    } else {
                        atotalc = n_array[i];
                    }
                    if (atotalc != 0) {
                        words_string += words[atotalc] + " ";
                    }
                    if ((i == 1 && atotalc != 0) || (i == 0 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && atotalc != 0) || (i == 2 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && atotalc != 0) || (i == 4 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && atotalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && atotalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }
        //==============End==============//








    }
})();